from __future__ import annotations

import json
import sys
from collections import Counter
from dataclasses import asdict, dataclass
from datetime import datetime
from pathlib import Path
from typing import Any, Callable

import cv2
from ultralytics import YOLO

SUPPORTED_IMAGE_EXTENSIONS = {".jpg", ".jpeg", ".png", ".bmp", ".webp"}
SUPPORTED_VIDEO_EXTENSIONS = {".mp4", ".avi", ".mov", ".mkv", ".wmv", ".m4v"}


@dataclass(slots=True)
class Detection:
    class_id: int
    class_name: str
    confidence: float
    bbox: dict[str, float]


@dataclass(slots=True)
class ImagePredictionResult:
    source_path: str
    annotated_path: str
    metadata_path: str
    image_size: tuple[int, int]
    detections: list[Detection]
    label_counts: dict[str, int]


@dataclass(slots=True)
class VideoPredictionResult:
    source_path: str
    annotated_video_path: str
    preview_image_path: str
    metadata_path: str
    frame_count_processed: int
    frames_with_detections: int
    fps: float
    resolution: tuple[int, int]
    label_counts: dict[str, int]
    writer_codec: str


class YoloInferenceService:
    def __init__(self, model_path: str | Path, confidence: float = 0.35, iou: float = 0.45):
        self.model_path = resolve_runtime_path(model_path)
        if not self.model_path.exists():
            raise FileNotFoundError(f"Model nebyl nalezen: {self.model_path}")

        self.confidence = float(confidence)
        self.iou = float(iou)
        self.model = YOLO(str(self.model_path))
        self.class_names = self.model.names

    def predict_image(self, image_path: str | Path, output_dir: str | Path) -> ImagePredictionResult:
        image_file = Path(image_path).expanduser().resolve()
        if not image_file.exists():
            raise FileNotFoundError(f"Obrázek nebyl nalezen: {image_file}")
        if image_file.suffix.lower() not in SUPPORTED_IMAGE_EXTENSIONS:
            raise ValueError(f"Nepodporovaný formát obrázku: {image_file.suffix}")

        output_directory = Path(output_dir).expanduser().resolve()
        output_directory.mkdir(parents=True, exist_ok=True)

        results = self.model.predict(source=str(image_file), conf=self.confidence, iou=self.iou, verbose=False)
        result = results[0]
        detections = self._extract_detections(result)
        annotated_frame = result.plot()

        annotated_path = output_directory / f"{image_file.stem}_detected{image_file.suffix}"
        metadata_path = output_directory / f"{image_file.stem}_detections.json"

        if not cv2.imwrite(str(annotated_path), annotated_frame):
            raise RuntimeError(f"Nepodařilo se uložit anotovaný obrázek do {annotated_path}")

        label_counts = dict(Counter(detection.class_name for detection in detections))
        image_height, image_width = annotated_frame.shape[:2]
        metadata = {
            "created_at": datetime.now().isoformat(timespec="seconds"),
            "model_path": str(self.model_path),
            "confidence": self.confidence,
            "iou": self.iou,
            "source_path": str(image_file),
            "annotated_path": str(annotated_path),
            "image_size": {"width": image_width, "height": image_height},
            "num_detections": len(detections),
            "label_counts": label_counts,
            "detections": [asdict(detection) for detection in detections],
        }
        metadata_path.write_text(json.dumps(metadata, indent=2, ensure_ascii=False), encoding="utf-8")

        return ImagePredictionResult(
            source_path=str(image_file),
            annotated_path=str(annotated_path),
            metadata_path=str(metadata_path),
            image_size=(image_width, image_height),
            detections=detections,
            label_counts=label_counts,
        )

    def predict_video(
        self,
        video_path: str | Path,
        output_dir: str | Path,
        progress_callback: Callable[[int, int], None] | None = None,
    ) -> VideoPredictionResult:
        video_file = Path(video_path).expanduser().resolve()
        if not video_file.exists():
            raise FileNotFoundError(f"Video nebylo nalezeno: {video_file}")
        if video_file.suffix.lower() not in SUPPORTED_VIDEO_EXTENSIONS:
            raise ValueError(f"Nepodporovaný formát videa: {video_file.suffix}")

        output_directory = Path(output_dir).expanduser().resolve()
        output_directory.mkdir(parents=True, exist_ok=True)

        capture = cv2.VideoCapture(str(video_file))
        if not capture.isOpened():
            raise RuntimeError(f"Nepodařilo se otevřít video: {video_file}")

        total_frames = max(int(capture.get(cv2.CAP_PROP_FRAME_COUNT)), 0)
        fps = capture.get(cv2.CAP_PROP_FPS) or 0.0
        if fps <= 0:
            fps = 25.0

        width = int(capture.get(cv2.CAP_PROP_FRAME_WIDTH)) or 0
        height = int(capture.get(cv2.CAP_PROP_FRAME_HEIGHT)) or 0
        if width <= 0 or height <= 0:
            ok, sample_frame = capture.read()
            if not ok or sample_frame is None:
                capture.release()
                raise RuntimeError("Video neobsahuje čitelné snímky.")
            height, width = sample_frame.shape[:2]
            capture.set(cv2.CAP_PROP_POS_FRAMES, 0)

        writer, output_video_path, codec = self._create_video_writer(
            output_directory / f"{video_file.stem}_detected.mp4",
            fps,
            (width, height),
        )
        preview_image_path = output_directory / f"{video_file.stem}_preview.jpg"
        metadata_path = output_directory / f"{video_file.stem}_detections.json"

        processed_frames = 0
        frames_with_detections = 0
        label_counter: Counter[str] = Counter()

        try:
            while True:
                ok, frame = capture.read()
                if not ok or frame is None:
                    break

                results = self.model.predict(source=frame, conf=self.confidence, iou=self.iou, verbose=False)
                result = results[0]
                detections = self._extract_detections(result)
                annotated_frame = result.plot()

                if processed_frames == 0:
                    cv2.imwrite(str(preview_image_path), annotated_frame)

                if detections:
                    frames_with_detections += 1
                    label_counter.update(detection.class_name for detection in detections)

                writer.write(annotated_frame)
                processed_frames += 1

                if progress_callback is not None:
                    progress_callback(processed_frames, total_frames)
        finally:
            capture.release()
            writer.release()

        if processed_frames == 0:
            raise RuntimeError("Video se nepodařilo zpracovat, nebyly načteny žádné snímky.")

        metadata: dict[str, Any] = {
            "created_at": datetime.now().isoformat(timespec="seconds"),
            "model_path": str(self.model_path),
            "confidence": self.confidence,
            "iou": self.iou,
            "source_path": str(video_file),
            "annotated_video_path": str(output_video_path),
            "preview_image_path": str(preview_image_path),
            "frame_count_processed": processed_frames,
            "frames_with_detections": frames_with_detections,
            "fps": fps,
            "duration_seconds": round(processed_frames / fps, 2) if fps else None,
            "resolution": {"width": width, "height": height},
            "label_counts": dict(label_counter),
            "writer_codec": codec,
        }
        metadata_path.write_text(json.dumps(metadata, indent=2, ensure_ascii=False), encoding="utf-8")

        return VideoPredictionResult(
            source_path=str(video_file),
            annotated_video_path=str(output_video_path),
            preview_image_path=str(preview_image_path),
            metadata_path=str(metadata_path),
            frame_count_processed=processed_frames,
            frames_with_detections=frames_with_detections,
            fps=fps,
            resolution=(width, height),
            label_counts=dict(label_counter),
            writer_codec=codec,
        )

    def _extract_detections(self, result: Any) -> list[Detection]:
        detections: list[Detection] = []
        boxes = getattr(result, "boxes", None)
        if boxes is None:
            return detections

        for box in boxes:
            class_id = int(box.cls[0].item()) if box.cls is not None else -1
            confidence = float(box.conf[0].item()) if box.conf is not None else 0.0
            xyxy = box.xyxy[0].tolist() if box.xyxy is not None else [0.0, 0.0, 0.0, 0.0]
            class_name = self.class_names.get(class_id, str(class_id))
            detections.append(
                Detection(
                    class_id=class_id,
                    class_name=class_name,
                    confidence=confidence,
                    bbox={
                        "x_min": float(xyxy[0]),
                        "y_min": float(xyxy[1]),
                        "x_max": float(xyxy[2]),
                        "y_max": float(xyxy[3]),
                    },
                )
            )

        return detections

    @staticmethod
    def _create_video_writer(base_output_path: Path, fps: float, resolution: tuple[int, int]):
        fourcc_factory = getattr(cv2, "VideoWriter_fourcc")
        attempts = [
            (base_output_path.with_suffix(".mp4"), "mp4v"),
            (base_output_path.with_suffix(".avi"), "XVID"),
            (base_output_path.with_suffix(".avi"), "MJPG"),
        ]
        width, height = resolution

        for output_path, codec in attempts:
            writer = cv2.VideoWriter(
                str(output_path),
                fourcc_factory(*codec),
                fps,
                (width, height),
            )
            if writer.isOpened():
                return writer, output_path, codec
            writer.release()

        raise RuntimeError("Nepodařilo se vytvořit video writer pro výstupní soubor.")


def get_runtime_base_dir() -> Path:
    if getattr(sys, "frozen", False):
        return Path(sys.executable).resolve().parent
    return Path(__file__).resolve().parent


def get_bundle_data_dir() -> Path | None:
    bundle_dir = getattr(sys, "_MEIPASS", None)
    return Path(bundle_dir).resolve() if bundle_dir else None


def resolve_runtime_path(path: str | Path, base_dir: str | Path | None = None) -> Path:
    candidate = Path(path).expanduser()
    if candidate.is_absolute():
        return candidate.resolve()

    search_dirs: list[Path] = []
    if base_dir is not None:
        search_dirs.append(Path(base_dir).expanduser().resolve())

    bundle_dir = get_bundle_data_dir()
    if bundle_dir is not None:
        search_dirs.append(bundle_dir)

    search_dirs.extend([get_runtime_base_dir(), Path.cwd().resolve()])

    unique_dirs: list[Path] = []
    for directory in search_dirs:
        if directory not in unique_dirs:
            unique_dirs.append(directory)

    for directory in unique_dirs:
        resolved_candidate = (directory / candidate).resolve()
        if resolved_candidate.exists():
            return resolved_candidate

    return (unique_dirs[0] / candidate).resolve() if unique_dirs else candidate.resolve()


def discover_model_files(base_dir: str | Path) -> list[str]:
    search_dirs = [Path(base_dir).expanduser().resolve(), get_runtime_base_dir(), Path.cwd().resolve()]
    bundle_dir = get_bundle_data_dir()
    if bundle_dir is not None:
        search_dirs.insert(1, bundle_dir)

    discovered: list[str] = []
    seen: set[Path] = set()
    for directory in search_dirs:
        if not directory.exists():
            continue
        for path in directory.glob("*.pt"):
            resolved = path.resolve()
            if resolved not in seen:
                seen.add(resolved)
                discovered.append(str(resolved))

    return sorted(discovered)


def is_image_file(path: str | Path) -> bool:
    return Path(path).suffix.lower() in SUPPORTED_IMAGE_EXTENSIONS


def is_video_file(path: str | Path) -> bool:
    return Path(path).suffix.lower() in SUPPORTED_VIDEO_EXTENSIONS

