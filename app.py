from __future__ import annotations

import os
import queue
import threading
import traceback
import webbrowser
from pathlib import Path
import tkinter as tk
from tkinter import filedialog, messagebox, ttk
from tkinter.scrolledtext import ScrolledText

from PIL import Image, ImageTk

from inference_service import (
    SUPPORTED_IMAGE_EXTENSIONS,
    SUPPORTED_VIDEO_EXTENSIONS,
    YoloInferenceService,
    discover_model_files,
    get_runtime_base_dir,
    is_image_file,
    is_video_file,
)

APP_TITLE = "Detektor objektů YOLO - Desktop GUI"
PREVIEW_SIZE = (520, 360)


class YoloDetectionApp:
    def __init__(self, root: tk.Tk):
        self.root = root
        self.root.title(APP_TITLE)
        self.root.geometry("1280x840")
        self.root.minsize(1100, 760)

        self.project_dir = get_runtime_base_dir()
        self.output_dir_default = self.project_dir / "gui_outputs"
        self.output_dir_default.mkdir(exist_ok=True)

        self.progress_queue: queue.Queue[tuple] = queue.Queue()
        self.service_lock = threading.Lock()
        self.cached_service: YoloInferenceService | None = None
        self.cached_service_key: tuple[str, float, float] | None = None
        self.is_busy = False
        self.last_image_result_path: str | None = None
        self.last_video_result_path: str | None = None
        self.last_metadata_path: str | None = None

        self.model_path_var = tk.StringVar(value=self._default_model_path())
        self.output_dir_var = tk.StringVar(value=str(self.output_dir_default))
        self.confidence_var = tk.DoubleVar(value=0.35)
        self.iou_var = tk.DoubleVar(value=0.45)
        self.status_var = tk.StringVar(value="Připraveno. Vyber model a vstupní soubor.")
        self.image_source_var = tk.StringVar()
        self.video_source_var = tk.StringVar()
        self.video_progress_var = tk.StringVar(value="Video není zpracováváno.")

        self._preview_refs: list[ImageTk.PhotoImage] = []
        self._action_buttons: list[ttk.Button] = []
        self._build_style()
        self._build_layout()
        self.root.after(100, self._poll_progress_queue, None)

    def _build_style(self) -> None:
        style = ttk.Style()
        if "clam" in style.theme_names():
            style.theme_use("clam")

        style.configure("Title.TLabel", font=("Segoe UI", 18, "bold"))
        style.configure("Subtitle.TLabel", font=("Segoe UI", 10))
        style.configure("Section.TLabelframe", padding=12)
        style.configure("Section.TLabelframe.Label", font=("Segoe UI", 10, "bold"))
        style.configure("Accent.TButton", padding=(12, 8))
        style.configure("Status.TLabel", padding=(8, 6), relief="groove")

    def _build_layout(self) -> None:
        self.root.columnconfigure(0, weight=1)
        self.root.rowconfigure(2, weight=1)

        header = ttk.Frame(self.root, padding=(16, 14, 16, 8))
        header.grid(row=0, column=0, sticky="ew")
        header.columnconfigure(0, weight=1)

        ttk.Label(header, text=APP_TITLE, style="Title.TLabel").grid(row=0, column=0, sticky="w")
        ttk.Label(
            header,
            text="Pohodlné desktop GUI pro detekci objektů v obrázcích i videu přes YOLO model.",
            style="Subtitle.TLabel",
        ).grid(row=1, column=0, sticky="w", pady=(4, 0))

        settings = ttk.LabelFrame(self.root, text="Nastavení detekce", style="Section.TLabelframe", padding=12)
        settings.grid(row=1, column=0, sticky="ew", padx=16, pady=(0, 10))
        for column in (1, 3):
            settings.columnconfigure(column, weight=1)

        ttk.Label(settings, text="YOLO model (.pt):").grid(row=0, column=0, sticky="w", padx=(0, 8), pady=4)
        self.model_combo = ttk.Combobox(settings, textvariable=self.model_path_var, values=self._available_models(), state="normal")
        self.model_combo.grid(row=0, column=1, sticky="ew", pady=4)
        self._register_button(ttk.Button(settings, text="Procházet…", command=self._browse_model)).grid(row=0, column=2, sticky="ew", padx=(8, 12), pady=4)

        ttk.Label(settings, text="Výstupní složka:").grid(row=0, column=3, sticky="w", padx=(0, 8), pady=4)
        ttk.Entry(settings, textvariable=self.output_dir_var).grid(row=0, column=4, sticky="ew", pady=4)
        self._register_button(ttk.Button(settings, text="Vybrat…", command=self._browse_output_dir)).grid(row=0, column=5, sticky="ew", padx=(8, 0), pady=4)

        ttk.Label(settings, text="Confidence threshold:").grid(row=1, column=0, sticky="w", padx=(0, 8), pady=4)
        confidence_scale = ttk.Scale(settings, from_=0.05, to=0.95, variable=self.confidence_var, command=self._update_slider_labels)
        confidence_scale.grid(row=1, column=1, sticky="ew", pady=4)
        self.confidence_label = ttk.Label(settings, width=6, text=f"{self.confidence_var.get():.2f}")
        self.confidence_label.grid(row=1, column=2, sticky="w", pady=4)

        ttk.Label(settings, text="IOU threshold:").grid(row=1, column=3, sticky="w", padx=(0, 8), pady=4)
        iou_scale = ttk.Scale(settings, from_=0.05, to=0.95, variable=self.iou_var, command=self._update_slider_labels)
        iou_scale.grid(row=1, column=4, sticky="ew", pady=4)
        self.iou_label = ttk.Label(settings, width=6, text=f"{self.iou_var.get():.2f}")
        self.iou_label.grid(row=1, column=5, sticky="w", pady=4)

        notebook = ttk.Notebook(self.root)
        notebook.grid(row=2, column=0, sticky="nsew", padx=16, pady=(0, 10))

        self.image_tab = ttk.Frame(notebook, padding=12)
        self.video_tab = ttk.Frame(notebook, padding=12)
        notebook.add(self.image_tab, text="Obrázek")
        notebook.add(self.video_tab, text="Video")

        self._build_image_tab()
        self._build_video_tab()

        status_bar = ttk.Label(self.root, textvariable=self.status_var, style="Status.TLabel", anchor="w")
        status_bar.grid(row=3, column=0, sticky="ew", padx=16, pady=(0, 14))

    def _build_image_tab(self) -> None:
        self.image_tab.columnconfigure(0, weight=1)
        self.image_tab.rowconfigure(1, weight=1)
        self.image_tab.rowconfigure(2, weight=1)

        input_frame = ttk.LabelFrame(self.image_tab, text="Vstupní obrázek", style="Section.TLabelframe")
        input_frame.grid(row=0, column=0, sticky="ew", pady=(0, 10))
        input_frame.columnconfigure(1, weight=1)

        ttk.Label(input_frame, text="Soubor:").grid(row=0, column=0, sticky="w", padx=(0, 8), pady=4)
        ttk.Entry(input_frame, textvariable=self.image_source_var).grid(row=0, column=1, sticky="ew", pady=4)
        self._register_button(ttk.Button(input_frame, text="Vybrat obrázek…", command=self._browse_image)).grid(row=0, column=2, sticky="ew", padx=(8, 8), pady=4)
        self._register_button(ttk.Button(input_frame, text="Detekovat", style="Accent.TButton", command=self._start_image_processing)).grid(row=0, column=3, sticky="ew", pady=4)
        self._register_button(ttk.Button(input_frame, text="Otevřít výstup", command=self._open_last_image_result)).grid(row=0, column=4, sticky="ew", padx=(8, 0), pady=4)

        preview_frame = ttk.Frame(self.image_tab)
        preview_frame.grid(row=1, column=0, sticky="nsew", pady=(0, 10))
        preview_frame.columnconfigure(0, weight=1)
        preview_frame.columnconfigure(1, weight=1)
        preview_frame.rowconfigure(0, weight=1)

        original_group = ttk.LabelFrame(preview_frame, text="Originál", style="Section.TLabelframe")
        original_group.grid(row=0, column=0, sticky="nsew", padx=(0, 6))
        original_group.columnconfigure(0, weight=1)
        original_group.rowconfigure(0, weight=1)
        self.image_original_preview = ttk.Label(original_group, anchor="center")
        self.image_original_preview.grid(row=0, column=0, sticky="nsew")

        result_group = ttk.LabelFrame(preview_frame, text="Detekovaný výstup", style="Section.TLabelframe")
        result_group.grid(row=0, column=1, sticky="nsew", padx=(6, 0))
        result_group.columnconfigure(0, weight=1)
        result_group.rowconfigure(0, weight=1)
        self.image_result_preview = ttk.Label(result_group, anchor="center")
        self.image_result_preview.grid(row=0, column=0, sticky="nsew")

        bottom_frame = ttk.Frame(self.image_tab)
        bottom_frame.grid(row=2, column=0, sticky="nsew")
        bottom_frame.columnconfigure(0, weight=3)
        bottom_frame.columnconfigure(1, weight=2)
        bottom_frame.rowconfigure(0, weight=1)

        detections_group = ttk.LabelFrame(bottom_frame, text="Detekce", style="Section.TLabelframe")
        detections_group.grid(row=0, column=0, sticky="nsew", padx=(0, 6))
        detections_group.columnconfigure(0, weight=1)
        detections_group.rowconfigure(0, weight=1)

        self.detection_tree = ttk.Treeview(
            detections_group,
            columns=("class", "confidence", "bbox"),
            show="headings",
            height=8,
        )
        self.detection_tree.heading("class", text="Třída")
        self.detection_tree.heading("confidence", text="Confidence")
        self.detection_tree.heading("bbox", text="Bounding box")
        self.detection_tree.column("class", width=160, anchor="w")
        self.detection_tree.column("confidence", width=90, anchor="center")
        self.detection_tree.column("bbox", width=360, anchor="w")
        self.detection_tree.grid(row=0, column=0, sticky="nsew")
        image_tree_scroll = ttk.Scrollbar(detections_group, orient="vertical", command=self.detection_tree.yview)
        image_tree_scroll.grid(row=0, column=1, sticky="ns")
        self.detection_tree.configure(yscrollcommand=image_tree_scroll.set)

        image_summary_group = ttk.LabelFrame(bottom_frame, text="Shrnutí", style="Section.TLabelframe")
        image_summary_group.grid(row=0, column=1, sticky="nsew", padx=(6, 0))
        image_summary_group.columnconfigure(0, weight=1)
        image_summary_group.rowconfigure(0, weight=1)

        self.image_summary_text = ScrolledText(image_summary_group, wrap="word", height=8, font=("Consolas", 10))
        self.image_summary_text.grid(row=0, column=0, sticky="nsew")
        self.image_summary_text.insert("1.0", "Po zpracování se zde zobrazí stručné shrnutí detekcí a cesty k uloženým souborům.")
        self.image_summary_text.configure(state="disabled")

    def _build_video_tab(self) -> None:
        self.video_tab.columnconfigure(0, weight=3)
        self.video_tab.columnconfigure(1, weight=2)
        self.video_tab.rowconfigure(1, weight=1)

        input_frame = ttk.LabelFrame(self.video_tab, text="Vstupní video", style="Section.TLabelframe")
        input_frame.grid(row=0, column=0, columnspan=2, sticky="ew", pady=(0, 10))
        input_frame.columnconfigure(1, weight=1)

        ttk.Label(input_frame, text="Soubor:").grid(row=0, column=0, sticky="w", padx=(0, 8), pady=4)
        ttk.Entry(input_frame, textvariable=self.video_source_var).grid(row=0, column=1, sticky="ew", pady=4)
        self._register_button(ttk.Button(input_frame, text="Vybrat video…", command=self._browse_video)).grid(row=0, column=2, sticky="ew", padx=(8, 8), pady=4)
        self._register_button(ttk.Button(input_frame, text="Detekovat video", style="Accent.TButton", command=self._start_video_processing)).grid(row=0, column=3, sticky="ew", pady=4)
        self._register_button(ttk.Button(input_frame, text="Otevřít výstup", command=self._open_last_video_result)).grid(row=0, column=4, sticky="ew", padx=(8, 0), pady=4)

        preview_group = ttk.LabelFrame(self.video_tab, text="Náhled prvního anotovaného snímku", style="Section.TLabelframe")
        preview_group.grid(row=1, column=0, sticky="nsew", padx=(0, 6))
        preview_group.columnconfigure(0, weight=1)
        preview_group.rowconfigure(0, weight=1)
        self.video_preview_label = ttk.Label(preview_group, anchor="center")
        self.video_preview_label.grid(row=0, column=0, sticky="nsew")

        info_group = ttk.LabelFrame(self.video_tab, text="Průběh a výsledek", style="Section.TLabelframe")
        info_group.grid(row=1, column=1, sticky="nsew", padx=(6, 0))
        info_group.columnconfigure(0, weight=1)
        info_group.rowconfigure(3, weight=1)

        ttk.Label(info_group, textvariable=self.video_progress_var).grid(row=0, column=0, sticky="w", pady=(0, 8))
        self.video_progress = ttk.Progressbar(info_group, orient="horizontal", mode="determinate")
        self.video_progress.grid(row=1, column=0, sticky="ew", pady=(0, 12))

        action_row = ttk.Frame(info_group)
        action_row.grid(row=2, column=0, sticky="ew", pady=(0, 10))
        action_row.columnconfigure(0, weight=1)
        action_row.columnconfigure(1, weight=1)
        self._register_button(ttk.Button(action_row, text="Otevřít složku s výsledky", command=self._open_output_folder)).grid(row=0, column=0, sticky="ew", padx=(0, 4))
        self._register_button(ttk.Button(action_row, text="Otevřít metadata JSON", command=self._open_last_metadata)).grid(row=0, column=1, sticky="ew", padx=(4, 0))

        self.video_summary_text = ScrolledText(info_group, wrap="word", height=12, font=("Consolas", 10))
        self.video_summary_text.grid(row=3, column=0, sticky="nsew")
        self.video_summary_text.insert("1.0", "Po dokončení se zde zobrazí informace o počtu snímků, zvoleném kodeku a detekovaných třídách.")
        self.video_summary_text.configure(state="disabled")

    def _register_button(self, button: ttk.Button) -> ttk.Button:
        self._action_buttons.append(button)
        return button

    def _available_models(self) -> list[str]:
        models = discover_model_files(self.project_dir)
        preferred = str((self.project_dir / "trained-model.pt").resolve())
        if preferred in models:
            models.remove(preferred)
            models.insert(0, preferred)
        return models

    def _default_model_path(self) -> str:
        models = self._available_models()
        return models[0] if models else ""

    def _browse_model(self) -> None:
        file_path = filedialog.askopenfilename(
            title="Vyber YOLO model",
            filetypes=[("PyTorch model", "*.pt"), ("All files", "*.*")],
            initialdir=str(self.project_dir),
        )
        if file_path:
            self.model_path_var.set(file_path)
            self._refresh_model_options(file_path)

    def _browse_output_dir(self) -> None:
        directory = filedialog.askdirectory(title="Vyber výstupní složku", initialdir=self.output_dir_var.get() or str(self.project_dir))
        if directory:
            self.output_dir_var.set(directory)

    def _browse_image(self) -> None:
        file_path = filedialog.askopenfilename(
            title="Vyber obrázek",
            filetypes=[("Obrázky", self._tk_file_pattern(SUPPORTED_IMAGE_EXTENSIONS)), ("All files", "*.*")],
            initialdir=str(self.project_dir),
        )
        if file_path:
            self.image_source_var.set(file_path)
            self._render_preview(self.image_original_preview, file_path)

    def _browse_video(self) -> None:
        file_path = filedialog.askopenfilename(
            title="Vyber video",
            filetypes=[("Videa", self._tk_file_pattern(SUPPORTED_VIDEO_EXTENSIONS)), ("All files", "*.*")],
            initialdir=str(self.project_dir),
        )
        if file_path:
            self.video_source_var.set(file_path)

    def _start_image_processing(self) -> None:
        source = self.image_source_var.get().strip()
        if not source:
            messagebox.showwarning(APP_TITLE, "Nejdřív vyber obrázek.")
            return
        if not is_image_file(source):
            messagebox.showerror(APP_TITLE, "Vybraný soubor není podporovaný obrázek.")
            return
        if not self._validate_common_inputs():
            return

        self._set_busy(True, "Načítám model a zpracovávám obrázek…")
        worker = threading.Thread(target=self._process_image_worker, args=(source,), daemon=True)
        worker.start()

    def _start_video_processing(self) -> None:
        source = self.video_source_var.get().strip()
        if not source:
            messagebox.showwarning(APP_TITLE, "Nejdřív vyber video.")
            return
        if not is_video_file(source):
            messagebox.showerror(APP_TITLE, "Vybraný soubor není podporované video.")
            return
        if not self._validate_common_inputs():
            return

        self.video_progress.configure(mode="determinate", maximum=100, value=0)
        self.video_progress_var.set("Připravuji zpracování videa…")
        self._set_busy(True, "Načítám model a zpracovávám video…")
        worker = threading.Thread(target=self._process_video_worker, args=(source,), daemon=True)
        worker.start()

    def _process_image_worker(self, source: str) -> None:
        try:
            service = self._get_or_create_service()
            result = service.predict_image(source, self.output_dir_var.get().strip())
            self.progress_queue.put(("image_done", result))
        except Exception as exc:  # noqa: BLE001
            self.progress_queue.put(("error", f"Obrázek se nepodařilo zpracovat: {exc}\n\n{traceback.format_exc()}"))

    def _process_video_worker(self, source: str) -> None:
        try:
            service = self._get_or_create_service()
            result = service.predict_video(
                source,
                self.output_dir_var.get().strip(),
                progress_callback=lambda done, total: self.progress_queue.put(("video_progress", done, total)),
            )
            self.progress_queue.put(("video_done", result))
        except Exception as exc:  # noqa: BLE001
            self.progress_queue.put(("error", f"Video se nepodařilo zpracovat: {exc}\n\n{traceback.format_exc()}"))

    def _get_or_create_service(self) -> YoloInferenceService:
        model_path = self.model_path_var.get().strip()
        confidence = round(float(self.confidence_var.get()), 2)
        iou = round(float(self.iou_var.get()), 2)
        key = (model_path, confidence, iou)

        with self.service_lock:
            if self.cached_service is None or self.cached_service_key != key:
                self.cached_service = YoloInferenceService(model_path=model_path, confidence=confidence, iou=iou)
                self.cached_service_key = key
            return self.cached_service

    def _poll_progress_queue(self, _tick=None) -> None:
        try:
            while True:
                item = self.progress_queue.get_nowait()
                self._handle_queue_item(item)
        except queue.Empty:
            pass
        finally:
            self.root.after(100, self._poll_progress_queue, None)

    def _handle_queue_item(self, item: tuple) -> None:
        kind = item[0]

        if kind == "image_done":
            result = item[1]
            self.last_image_result_path = result.annotated_path
            self.last_metadata_path = result.metadata_path
            self._render_preview(self.image_original_preview, result.source_path)
            self._render_preview(self.image_result_preview, result.annotated_path)
            self._populate_detection_tree(result.detections)
            summary = self._format_image_summary(result)
            self._set_text(self.image_summary_text, summary)
            self._set_busy(False, f"Hotovo: obrázek zpracován a uložen do {result.annotated_path}")
            return

        if kind == "video_progress":
            done, total = item[1], item[2]
            if total > 0:
                self.video_progress.configure(mode="determinate", maximum=total, value=done)
                percent = (done / total) * 100
                self.video_progress_var.set(f"Zpracováno {done}/{total} snímků ({percent:.1f} %)")
            else:
                self.video_progress.configure(mode="indeterminate")
                self.video_progress.start(8)
                self.video_progress_var.set(f"Zpracováno {done} snímků…")
            return

        if kind == "video_done":
            result = item[1]
            self.last_video_result_path = result.annotated_video_path
            self.last_metadata_path = result.metadata_path
            self.video_progress.stop()
            self.video_progress.configure(mode="determinate", maximum=max(result.frame_count_processed, 1), value=result.frame_count_processed)
            self.video_progress_var.set(f"Hotovo: {result.frame_count_processed} snímků zpracováno.")
            self._render_preview(self.video_preview_label, result.preview_image_path)
            summary = self._format_video_summary(result)
            self._set_text(self.video_summary_text, summary)
            self._set_busy(False, f"Hotovo: video uloženo do {result.annotated_video_path}")
            return

        if kind == "error":
            self.video_progress.stop()
            self._set_busy(False, "Zpracování skončilo chybou.")
            messagebox.showerror(APP_TITLE, item[1])

    def _populate_detection_tree(self, detections) -> None:
        for item in self.detection_tree.get_children():
            self.detection_tree.delete(item)

        for detection in detections:
            bbox = detection.bbox
            bbox_text = f"({bbox['x_min']:.0f}, {bbox['y_min']:.0f}) – ({bbox['x_max']:.0f}, {bbox['y_max']:.0f})"
            self.detection_tree.insert(
                "",
                "end",
                values=(detection.class_name, f"{detection.confidence:.2f}", bbox_text),
            )

    def _format_image_summary(self, result) -> str:
        classes_text = ", ".join(f"{label}: {count}" for label, count in sorted(result.label_counts.items())) or "Žádné detekce"
        return (
            f"Zdroj: {result.source_path}\n"
            f"Výstup: {result.annotated_path}\n"
            f"Metadata: {result.metadata_path}\n"
            f"Rozlišení: {result.image_size[0]}×{result.image_size[1]}\n"
            f"Počet detekcí: {len(result.detections)}\n"
            f"Třídy: {classes_text}\n"
        )

    def _format_video_summary(self, result) -> str:
        classes_text = ", ".join(f"{label}: {count}" for label, count in sorted(result.label_counts.items())) or "Žádné detekce"
        return (
            f"Zdroj: {result.source_path}\n"
            f"Výstupní video: {result.annotated_video_path}\n"
            f"Preview: {result.preview_image_path}\n"
            f"Metadata: {result.metadata_path}\n"
            f"Rozlišení: {result.resolution[0]}×{result.resolution[1]}\n"
            f"FPS: {result.fps:.2f}\n"
            f"Zpracované snímky: {result.frame_count_processed}\n"
            f"Snímky s detekcí: {result.frames_with_detections}\n"
            f"Použitý kodek: {result.writer_codec}\n"
            f"Třídy: {classes_text}\n"
        )

    def _set_text(self, widget: ScrolledText, value: str) -> None:
        widget.configure(state="normal")
        widget.delete("1.0", tk.END)
        widget.insert("1.0", value)
        widget.configure(state="disabled")

    def _render_preview(self, target: ttk.Label, image_path: str) -> None:
        path = Path(image_path)
        if not path.exists():
            target.configure(image="", text="Náhled není dostupný.")
            return

        with Image.open(path) as source_image:
            image = source_image.copy()
        image.thumbnail(PREVIEW_SIZE)
        photo = ImageTk.PhotoImage(image)
        self._preview_refs.append(photo)
        if len(self._preview_refs) > 8:
            self._preview_refs = self._preview_refs[-8:]
        target.configure(image=photo, text="")
        target.image = photo

    def _validate_common_inputs(self) -> bool:
        model_path = self.model_path_var.get().strip()
        output_dir = self.output_dir_var.get().strip()
        if self.is_busy:
            messagebox.showinfo(APP_TITLE, "Počkej na dokončení aktuální úlohy.")
            return False
        if not model_path:
            messagebox.showerror(APP_TITLE, "Vyber YOLO model.")
            return False
        if not Path(model_path).exists():
            messagebox.showerror(APP_TITLE, "Zvolený model neexistuje.")
            return False
        if not output_dir:
            messagebox.showerror(APP_TITLE, "Vyber výstupní složku.")
            return False
        Path(output_dir).mkdir(parents=True, exist_ok=True)
        return True

    def _set_busy(self, value: bool, status_message: str) -> None:
        self.is_busy = value
        self.status_var.set(status_message)
        state = "disabled" if value else "normal"
        for button in self._action_buttons:
            button.configure(state=state)

    def _refresh_model_options(self, extra_model_path: str | None = None) -> None:
        models = self._available_models()
        if extra_model_path and extra_model_path not in models:
            models.insert(0, extra_model_path)
        self.model_combo.configure(values=models)

    def _update_slider_labels(self, _value=None) -> None:
        self.confidence_label.configure(text=f"{self.confidence_var.get():.2f}")
        self.iou_label.configure(text=f"{self.iou_var.get():.2f}")

    def _open_last_image_result(self) -> None:
        self._open_path(self.last_image_result_path, "Nejdřív zpracuj obrázek.")

    def _open_last_video_result(self) -> None:
        self._open_path(self.last_video_result_path, "Nejdřív zpracuj video.")

    def _open_last_metadata(self) -> None:
        self._open_path(getattr(self, "last_metadata_path", None), "Metadata zatím nejsou k dispozici.")

    def _open_output_folder(self) -> None:
        self._open_path(self.output_dir_var.get().strip(), "Výstupní složka není nastavena.")

    def _open_path(self, path: str | None, empty_message: str) -> None:
        if not path:
            messagebox.showinfo(APP_TITLE, empty_message)
            return

        if not Path(path).exists():
            messagebox.showerror(APP_TITLE, f"Soubor nebo složka neexistuje:\n{path}")
            return

        try:
            if os.name == "nt":
                os.startfile(path)  # type: ignore[attr-defined]
            else:
                webbrowser.open(Path(path).as_uri())
        except Exception as exc:  # noqa: BLE001
            messagebox.showerror(APP_TITLE, f"Nepodařilo se otevřít cestu:\n{path}\n\n{exc}")

    @staticmethod
    def _tk_file_pattern(extensions: set[str]) -> str:
        return " ".join(f"*{extension}" for extension in sorted(extensions))


def main() -> None:
    root = tk.Tk()
    app = YoloDetectionApp(root)
    app._update_slider_labels()
    root.mainloop()


if __name__ == "__main__":
    main()

