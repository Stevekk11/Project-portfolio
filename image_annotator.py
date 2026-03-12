"""
Image Annotator using YOLOv8

This script annotates images from a folder using the YOLOv8 model
and maps detected classes: bus -> autobus_pid, train -> tramvaj_pid
"""

from ultralytics import YOLO
import cv2
import os
import json
import logging
from pathlib import Path
from typing import Dict, List, Tuple

# Set up logging
logging.basicConfig(
    level=logging.INFO,
    format='%(asctime)s - %(levelname)s - %(message)s'
)
logger = logging.getLogger(__name__)

# Class mappings for different sources
CLASS_MAPPINGS = {
    'pid': {
        'bus': 'autobus_pid',
        'train': 'tramvaj_pid'
    },
    'other': {
        'bus': 'other_bus',
        'train': 'other_tram'
    }
}


class ImageAnnotator:
    def __init__(self, model_name='yolov8n.pt', confidence_threshold=0.5, class_mapping=None):
        """
        Initialize the image annotator with YOLOv8 model.

        Args:
            model_name: Name of the YOLOv8 model to use (default: yolov8n.pt - nano model)
                        Other options: yolov8s.pt, yolov8m.pt, yolov8l.pt, yolov8x.pt
            confidence_threshold: Confidence threshold for detections (0-1)
            class_mapping: Optional dictionary mapping original class names to custom ones
        """
        self.model_name = model_name
        self.confidence_threshold = confidence_threshold
        self.model = None
        self.class_names = {}
        self.class_mapping = class_mapping if class_mapping else CLASS_MAPPINGS['other']

        self._load_model()

    def _load_model(self):
        """Load the YOLOv8 model."""
        try:
            logger.info(f"Loading YOLOv8 model: {self.model_name}")
            self.model = YOLO(self.model_name)

            # Get class names from model
            self.class_names = self.model.names
            logger.info(f"Model loaded successfully with {len(self.class_names)} classes")
            logger.info(f"Available classes: {self.class_names}")
        except Exception as e:
            logger.error(f"Failed to load YOLOv8 model: {e}")
            raise

    def _map_class_name(self, class_name: str) -> str:
        """
        Map detected class name to custom naming convention.

        Args:
            class_name: Original class name from model

        Returns:
            Mapped class name if in mapping, otherwise None (to filter out unwanted classes)
        """
        class_name_lower = class_name.lower().strip()
        return self.class_mapping.get(class_name_lower, None)

    def annotate_image(self, image_path: str, save_annotated=True, output_dir=None) -> Dict:
        """
        Annotate a single image using YOLOv8.

        Args:
            image_path: Path to the image file
            save_annotated: If True, saves the annotated image with bounding boxes
            output_dir: Directory to save annotated images (if None, uses same as input)

        Returns:
            Dictionary containing detection results with mapped class names
        """
        try:
            if not os.path.exists(image_path):
                logger.error(f"Image file not found: {image_path}")
                return {'image': image_path, 'detections': [], 'error': 'File not found'}

            logger.info(f"Processing: {image_path}")

            # Run inference
            results = self.model(image_path, conf=self.confidence_threshold, verbose=False)
            result = results[0]

            # Extract detections
            detections = []
            for box in result.boxes:
                class_id = int(box.cls[0])
                original_class_name = self.class_names.get(class_id, 'unknown')
                mapped_class_name = self._map_class_name(original_class_name)

                # Skip detections that are not in our mapping (only bus and train)
                if mapped_class_name is None:
                    logger.debug(f"  Skipped: {original_class_name} (not in target classes)")
                    continue

                detection = {
                    'class_id': class_id,
                    'class_name': original_class_name,
                    'class_name_mapped': mapped_class_name,
                    'confidence': float(box.conf[0]),
                    'bbox': {
                        'x_min': float(box.xyxy[0][0]),
                        'y_min': float(box.xyxy[0][1]),
                        'x_max': float(box.xyxy[0][2]),
                        'y_max': float(box.xyxy[0][3])
                    }
                }
                detections.append(detection)
                logger.info(f"  Detected: {mapped_class_name} (confidence: {detection['confidence']:.2f})")

            # Save annotated image if requested
            if save_annotated:
                if output_dir is None:
                    output_dir = os.path.dirname(image_path)

                os.makedirs(output_dir, exist_ok=True)
                filename = os.path.basename(image_path)
                name, ext = os.path.splitext(filename)
                annotated_path = os.path.join(output_dir, f"{name}_annotated{ext}")

                # Draw annotations
                img = cv2.imread(image_path)
                for detection in detections:
                    bbox = detection['bbox']
                    x_min, y_min, x_max, y_max = int(bbox['x_min']), int(bbox['y_min']), int(bbox['x_max']), int(bbox['y_max'])

                    # Draw bounding box
                    cv2.rectangle(img, (x_min, y_min), (x_max, y_max), (0, 255, 0), 2)

                    # Draw label
                    label = f"{detection['class_name_mapped']} ({detection['confidence']:.2f})"
                    cv2.putText(img, label, (x_min, y_min - 10),
                               cv2.FONT_HERSHEY_SIMPLEX, 0.5, (0, 255, 0), 2)

                cv2.imwrite(annotated_path, img)
                logger.info(f"Saved annotated image: {annotated_path}")

            return {
                'image': image_path,
                'detections': detections,
                'num_detections': len(detections),
                'error': None
            }

        except Exception as e:
            logger.error(f"Error annotating image {image_path}: {e}")
            return {'image': image_path, 'detections': [], 'error': str(e)}

    def _save_yolo_format(self, output_dir: str, results: List[Dict], image_folder: str, save_classes_txt: bool = True, sorted_classes_override: List[str] = None):
        """
        Save annotations in YOLO format (Darknet format) for training.
        Creates .txt files with normalized coordinates suitable for YOLOv8 training.
        Only saves labels for bus and train detections.

        Args:
            output_dir: Directory to save YOLO format annotations
            results: List of annotation results
            image_folder: Path to the original image folder
            save_classes_txt: Whether to save classes.txt
            sorted_classes_override: Optional list of class names to use for consistent indexing
        """
        yolo_labels_dir = os.path.join(output_dir, 'labels')
        os.makedirs(yolo_labels_dir, exist_ok=True)

        # Use override if provided, otherwise determine from current mapping
        sorted_classes = sorted_classes_override if sorted_classes_override else sorted(set(self.class_mapping.values()))
        
        if save_classes_txt:
            classes_txt_path = os.path.join(output_dir, 'classes.txt')
            with open(classes_txt_path, 'w', encoding='utf-8') as f:
                for class_name in sorted_classes:
                    f.write(f"{class_name}\n")
            logger.info(f"Saved class names to: {classes_txt_path}")

        # Save YOLO format labels for each image
        yolo_results_summary = []
        for result in results:
            if result.get('error') or not result.get('detections'):
                continue

            # Check if there are any valid detections (after filtering)
            valid_detections = [d for d in result['detections'] if d['class_name_mapped'] is not None]
            if not valid_detections:
                logger.debug(f"Skipped {result['image']} - no target class detections")
                continue

            image_path = result['image']
            image_name = os.path.splitext(os.path.basename(image_path))[0]

            # Get image dimensions to normalize coordinates
            img = cv2.imread(image_path)
            if img is None:
                logger.warning(f"Could not read image dimensions for {image_path}")
                continue

            img_height, img_width = img.shape[:2]

            # Create YOLO format label file
            label_path = os.path.join(yolo_labels_dir, f"{image_name}.txt")
            with open(label_path, 'w', encoding='utf-8') as f:
                for detection in valid_detections:
                    mapped_class = detection['class_name_mapped']
                    # Find class index from mapping
                    class_idx = sorted_classes.index(mapped_class)

                    bbox = detection['bbox']
                    x_min, y_min, x_max, y_max = bbox['x_min'], bbox['y_min'], bbox['x_max'], bbox['y_max']

                    # Normalize coordinates to 0-1 range
                    x_center = ((x_min + x_max) / 2) / img_width
                    y_center = ((y_min + y_max) / 2) / img_height
                    width = (x_max - x_min) / img_width
                    height = (y_max - y_min) / img_height

                    # Ensure values are in valid range
                    x_center = max(0, min(1, x_center))
                    y_center = max(0, min(1, y_center))
                    width = max(0, min(1, width))
                    height = max(0, min(1, height))

                    # Write in YOLO format: class_id x_center y_center width height
                    f.write(f"{class_idx} {x_center:.6f} {y_center:.6f} {width:.6f} {height:.6f}\n")

            yolo_results_summary.append({
                'image': image_path,
                'label_file': label_path,
                'detections': len(valid_detections)
            })
            logger.info(f"Saved YOLO format label: {label_path}")

        logger.info(f"Saved {len(yolo_results_summary)} label files in YOLO format (only bus/train)")
        return yolo_results_summary

    def _create_dataset_structure(self, output_dir: str, results: List[Dict], image_folder: str, save_classes_txt: bool = True, sorted_classes_override: List[str] = None):
        """
        Create a complete dataset structure for YOLO training.
        Copies images and creates the necessary folder structure.
        Only includes images with bus or train detections.

        Args:
            output_dir: Base directory for dataset
            results: List of annotation results
            image_folder: Path to original images
            save_classes_txt: Whether to save classes.txt
            sorted_classes_override: Optional list of class names to use for consistent indexing
        """
        # Create directory structure
        images_dir = os.path.join(output_dir, 'images')
        labels_dir = os.path.join(output_dir, 'labels')
        os.makedirs(images_dir, exist_ok=True)
        os.makedirs(labels_dir, exist_ok=True)

        logger.info(f"Created dataset structure in: {output_dir}")

        # Use override if provided, otherwise determine from current mapping
        sorted_classes = sorted_classes_override if sorted_classes_override else sorted(set(self.class_mapping.values()))
        
        if save_classes_txt:
            classes_txt_path = os.path.join(output_dir, 'classes.txt')
            with open(classes_txt_path, 'w', encoding='utf-8') as f:
                for class_name in sorted_classes:
                    f.write(f"{class_name}\n")
            logger.info(f"Saved class names to: {classes_txt_path}")

        # Copy images and create labels
        yolo_results_summary = []
        for result in results:
            if result.get('error') or not result.get('detections'):
                continue

            # Check if there are any valid detections (after filtering)
            valid_detections = [d for d in result['detections'] if d['class_name_mapped'] is not None]
            if not valid_detections:
                logger.debug(f"Skipped {result['image']} - no target class detections")
                continue

            image_path = result['image']
            image_name = os.path.basename(image_path)
            image_name_no_ext = os.path.splitext(image_name)[0]

            # Copy image
            dest_image = os.path.join(images_dir, image_name)
            import shutil
            shutil.copy2(image_path, dest_image)

            # Get image dimensions
            img = cv2.imread(image_path)
            if img is None:
                continue

            img_height, img_width = img.shape[:2]

            # Create label file
            label_path = os.path.join(labels_dir, f"{image_name_no_ext}.txt")
            with open(label_path, 'w', encoding='utf-8') as f:
                for detection in valid_detections:
                    mapped_class = detection['class_name_mapped']
                    class_idx = sorted_classes.index(mapped_class)

                    bbox = detection['bbox']
                    x_min, y_min, x_max, y_max = bbox['x_min'], bbox['y_min'], bbox['x_max'], bbox['y_max']

                    x_center = ((x_min + x_max) / 2) / img_width
                    y_center = ((y_min + y_max) / 2) / img_height
                    width = (x_max - x_min) / img_width
                    height = (y_max - y_min) / img_height

                    x_center = max(0, min(1, x_center))
                    y_center = max(0, min(1, y_center))
                    width = max(0, min(1, width))
                    height = max(0, min(1, height))

                    f.write(f"{class_idx} {x_center:.6f} {y_center:.6f} {width:.6f} {height:.6f}\n")

            yolo_results_summary.append({
                'image': dest_image,
                'label_file': label_path,
                'detections': len(valid_detections)
            })

        logger.info(f"Copied {len(yolo_results_summary)} images to dataset structure (only with bus/train)")
        return yolo_results_summary

    def annotate_folder(self, folder_path: str, save_annotated=True,
                       output_dir=None, save_json=True, save_yolo=True,
                       create_dataset=False, recursive=False,
                       skip_existing=False, class_mapping=None,
                       is_combined=False, sorted_classes_override: List[str] = None) -> List[Dict]:
        """
        Annotate all images in a folder.

        Args:
            folder_path: Path to the folder containing images
            save_annotated: If True, saves annotated images with bounding boxes
            output_dir: Directory to save annotated images (if None, uses same as input)
            save_json: If True, saves detection results as JSON
            save_yolo: If True, saves annotations in YOLO format for training
            create_dataset: If True, creates complete dataset structure (images + labels)
            recursive: If True, processes subfolders recursively
            skip_existing: If True, skips images that already have output files
            class_mapping: Optional dictionary mapping original class names to custom ones
            is_combined: If True, skips saving JSON and classes.txt (handled by caller)
            sorted_classes_override: Optional list of class names for consistent indexing

        Returns:
            List of results for each image
        """
        if class_mapping:
            self.class_mapping = class_mapping

        if not os.path.isdir(folder_path):
            logger.error(f"Folder not found: {folder_path}")
            return []

        logger.info(f"\nStarting annotation of folder: {folder_path}")
        logger.info(f"Save annotated images: {save_annotated}")
        logger.info(f"Save JSON results: {save_json}")
        logger.info(f"Save YOLO format: {save_yolo}")
        logger.info(f"Create dataset structure: {create_dataset}")
        logger.info(f"Recursive: {recursive}")
        logger.info(f"Skip existing: {skip_existing}")

        # Supported image formats
        image_extensions = {'.jpg', '.jpeg', '.png', '.bmp', '.gif', '.tiff', '.webp'}

        # Find all image files
        if recursive:
            image_files = []
            for ext in image_extensions:
                image_files.extend(Path(folder_path).rglob(f'*{ext}'))
                image_files.extend(Path(folder_path).rglob(f'*{ext.upper()}'))
        else:
            image_files = []
            for ext in image_extensions:
                image_files.extend(Path(folder_path).glob(f'*{ext}'))
                image_files.extend(Path(folder_path).glob(f'*{ext.upper()}'))

        image_files = list(set([str(f) for f in image_files]))  # Remove duplicates

        if not image_files:
            logger.warning(f"No image files found in {folder_path}")
            return []

        if output_dir is None:
            output_dir = folder_path

        # If skip_existing is True, filter out images that already have output files
        if skip_existing:
            original_count = len(image_files)
            filtered_image_files = []
            
            for image_file in image_files:
                filename = os.path.basename(image_file)
                name, ext = os.path.splitext(filename)
                
                # Check for YOLO label file
                if save_yolo:
                    if create_dataset:
                        label_path = os.path.join(output_dir, 'labels', f"{name}.txt")
                    else:
                        label_path = os.path.join(output_dir, 'labels', f"{name}.txt")
                    
                    if os.path.exists(label_path):
                        continue
                
                # Check for annotated image
                if save_annotated:
                    annotated_path = os.path.join(output_dir, f"{name}_annotated{ext}")
                    if os.path.exists(annotated_path):
                        continue
                
                filtered_image_files.append(image_file)
            
            image_files = filtered_image_files
            skipped_count = original_count - len(image_files)
            if skipped_count > 0:
                logger.info(f"Skipping {skipped_count} already annotated images")

        if not image_files:
            logger.info("No new images to process")
            return []

        logger.info(f"Found {len(image_files)} images to process")

        # Process all images
        results = []
        for i, image_file in enumerate(image_files, 1):
            logger.info(f"\n[{i}/{len(image_files)}] Processing: {os.path.basename(image_file)}")
            result = self.annotate_image(image_file, save_annotated=save_annotated,
                                        output_dir=output_dir)
            results.append(result)

        os.makedirs(output_dir, exist_ok=True)

        # Save results as JSON if requested (skip if handled externally)
        if save_json and not is_combined:
            json_path = os.path.join(output_dir, 'annotations.json')

            with open(json_path, 'w', encoding='utf-8') as f:
                json.dump(results, f, indent=2, ensure_ascii=False)

            logger.info(f"\nSaved annotations to: {json_path}")

        # Save YOLO format if requested
        if save_yolo:
            if create_dataset:
                self._create_dataset_structure(output_dir, results, folder_path, 
                                               save_classes_txt=not is_combined,
                                               sorted_classes_override=sorted_classes_override)
            else:
                self._save_yolo_format(output_dir, results, folder_path, 
                                       save_classes_txt=not is_combined,
                                       sorted_classes_override=sorted_classes_override)

        # Print summary
        total_detections = sum(r.get('num_detections', 0) for r in results)
        logger.info(f"\n{'='*60}")
        logger.info(f"ANNOTATION SUMMARY")
        logger.info(f"{'='*60}")
        logger.info(f"Total images processed: {len(results)}")
        logger.info(f"Total detections: {total_detections}")

        # Count detections by class
        class_counts = {}
        for result in results:
            for detection in result.get('detections', []):
                mapped_class = detection['class_name_mapped']
                class_counts[mapped_class] = class_counts.get(mapped_class, 0) + 1

        if class_counts:
            logger.info(f"Detections by class:")
            for class_name, count in sorted(class_counts.items()):
                logger.info(f"  - {class_name}: {count}")
        else:
            logger.warning("No detections found in any images")

        logger.info(f"{'='*60}\n")

        return results


def main():
    """Main function to run the image annotator."""
    import sys

    print("\n" + "="*60)
    print("YOLOv8 MULTI-FOLDER IMAGE ANNOTATOR")
    print("="*60)

    # Folders to process
    folders_to_process = [
        ('bus_photos_pid', CLASS_MAPPINGS['pid']),
        ('bus_photos_other', CLASS_MAPPINGS['other'])
    ]

    # Verify folders exist
    existing_folders = []
    for folder, mapping in folders_to_process:
        if os.path.isdir(folder):
            existing_folders.append((folder, mapping))
        else:
            logger.warning(f"Folder not found: {folder}")

    if not existing_folders:
        logger.error("No valid folders found to process. Please check folder names.")
        # Fallback to manual input if no default folders found
        manual_folder = input("\nEnter the path to a folder with images manually: ").strip()
        if manual_folder and os.path.isdir(manual_folder):
            mapping_choice = input("Use 'pid' or 'other' mapping? (p/o): ").strip().lower()
            mapping = CLASS_MAPPINGS['pid'] if mapping_choice == 'p' else CLASS_MAPPINGS['other']
            existing_folders = [(manual_folder, mapping)]
        else:
            return

    # Get output directory (optional)
    output_choice = input("\nSave all annotated images and results in a single separate folder? (y/n, default: y): ").strip().lower() != 'n'
    if output_choice:
        output_dir = input("Enter output folder path (or press Enter for 'annotations_combined'): ").strip()
        if not output_dir:
            output_dir = 'annotations_combined'
    else:
        output_dir = None

    # Get recursive option
    recursive = input("\nProcess subfolders recursively? (y/n): ").strip().lower() == 'y'

    # Get skip existing option
    skip_existing = input("\nSkip already annotated images? (y/n, default: y): ").strip().lower() != 'n'

    # Get model choice
    models = ['yolov8n.pt', 'yolov8s.pt', 'yolov8m.pt', 'yolov8l.pt', 'yolov8x.pt']
    print("\nAvailable models:")
    for i, model in enumerate(models, 1):
        print(f"  {i}. {model}")

    model_choice = input("Select model (1-5) or enter custom model name (default: 1 - yolov8n.pt): ").strip()
    if model_choice.isdigit() and 1 <= int(model_choice) <= 5:
        model_name = models[int(model_choice) - 1]
    elif model_choice:
        model_name = model_choice
    else:
        model_name = 'yolov8n.pt'

    # Get confidence threshold
    confidence_input = input("Enter confidence threshold (0-1, default: 0.5): ").strip()
    try:
        confidence_threshold = float(confidence_input) if confidence_input else 0.5
        confidence_threshold = max(0, min(1, confidence_threshold))  # Clamp to 0-1
    except ValueError:
        confidence_threshold = 0.5
        logger.warning(f"Invalid confidence value, using default: {confidence_threshold}")

    # Ask about YOLO format output
    print("\n" + "="*60)
    print("OUTPUT FORMAT OPTIONS")
    print("="*60)
    
    # Check if user wants metadata only (e.g. if it was lost or incorrect)
    metadata_only = input("Generate ONLY Metadata (JSON and YOLO labels; skip saving/copying images)? (y/n, default: n): ").strip().lower() == 'y'
    
    if metadata_only:
        save_json = True
        save_yolo = True
        save_annotated = False
        create_dataset = False
    else:
        save_json = input("Save JSON annotations? (y/n, default: y): ").strip().lower() != 'n'
        save_yolo = input("Save YOLO format annotations for training? (y/n, default: y): ").strip().lower() != 'n'
        save_annotated = True

        create_dataset = False
        if save_yolo:
            create_dataset = input("Create full dataset structure (images + labels folders)? (y/n, default: y): ").strip().lower() != 'n'

    # Create annotator
    try:
        annotator = ImageAnnotator(model_name=model_name, confidence_threshold=confidence_threshold)
        
        all_results = []
        all_mapped_class_names = set()
        for folder_path, mapping in existing_folders:
            all_mapped_class_names.update(mapping.values())
            
        # Determine global sorted classes for consistent indexing across all folders
        global_sorted_classes = sorted(list(all_mapped_class_names))
        
        for folder_path, mapping in existing_folders:
            # Determine specific output dir for this folder if not combining
            current_output_dir = output_dir if output_dir else folder_path
            
            logger.info(f"\nProcessing folder: {folder_path} with mapping: {mapping}")
            results = annotator.annotate_folder(
                folder_path,
                save_annotated=save_annotated,
                output_dir=current_output_dir,
                save_json=save_json,
                save_yolo=save_yolo,
                create_dataset=create_dataset,
                recursive=recursive,
                skip_existing=skip_existing,
                class_mapping=mapping,
                is_combined=bool(output_dir),
                sorted_classes_override=global_sorted_classes if output_dir else None
            )
            all_results.extend(results)

        logger.info(f"\nAnnotation complete! Total processed {len(all_results)} images across all folders.")

        # If combined output, save global annotations.json and classes.txt
        if output_dir:
            os.makedirs(output_dir, exist_ok=True)
            
            if save_json:
                json_path = os.path.join(output_dir, 'annotations.json')
                with open(json_path, 'w', encoding='utf-8') as f:
                    json.dump(all_results, f, indent=2, ensure_ascii=False)
                logger.info(f"Saved combined annotations to: {json_path}")
            
        if save_yolo:
                classes_txt_path = os.path.join(output_dir, 'classes.txt')
                with open(classes_txt_path, 'w', encoding='utf-8') as f:
                    for class_name in global_sorted_classes:
                        f.write(f"{class_name}\n")
                logger.info(f"Saved combined class names to: {classes_txt_path}")

        if save_yolo and output_dir:
            logger.info("\n" + "="*60)
            logger.info("YOLO FORMAT TRAINING DATA")
            logger.info("="*60)
            logger.info("Your data is ready for YOLOv8 training!")
            if create_dataset:
                logger.info("Dataset structure created:")
                logger.info(f"  {output_dir}/images/ - Training images")
                logger.info(f"  {output_dir}/labels/ - YOLO format labels")
                logger.info(f"  {output_dir}/classes.txt - Class definitions")
            else:
                logger.info("YOLO format files created:")
                logger.info(f"  {output_dir}/labels/ - YOLO format labels")
                logger.info(f"  {output_dir}/classes.txt - Class definitions")
            logger.info("\nTo train a new YOLOv8 model:")
            logger.info("  from ultralytics import YOLO")
            logger.info("  model = YOLO('yolov8n.pt')")
            logger.info(f"  results = model.train(data='path/to/dataset.yaml', epochs=100)")
            logger.info("="*60 + "\n")

    except Exception as e:
        logger.error(f"Fatal error: {e}")
        import traceback
        logger.error(traceback.format_exc())
        return


if __name__ == "__main__":
    main()

