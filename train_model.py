"""
Script to train YOLOv8 model on custom annotated data.
"""

import os
from ultralytics import YOLO


def train_yolo_model(dataset_yaml, base_model='yolov8m.pt', epochs=100, imgsz=640, batch=16, export_format=None):
    """
    Train a YOLOv8 model using the provided dataset.

    Args:
        dataset_yaml: Path to the dataset.yaml file
        base_model: Pre-trained model weights to start from
        epochs: Number of training epochs
        imgsz: Image size for training
        batch: Batch size
        export_format: Optional format to export the model (e.g., 'onnx', 'torchscript', 'tflite')
    """
    print(f"Starting training on {dataset_yaml} using {base_model}...")
    
    # Load the model
    model = YOLO(base_model)

    # Start training
    results = model.train(
        data=dataset_yaml,
        epochs=epochs,
        imgsz=imgsz,
        batch=batch,
        name='yolov8_custom_pid',
        exist_ok=True
    )

    print("Training finished!")

    # Export the model if requested
    if export_format:
        print(f"Exporting model to {export_format} format...")
        model.export(format=export_format)
        print(f"Export to {export_format} finished!")

    return results


if __name__ == "__main__":
    # Correct path to the dataset.yaml created by create_training_config.py
    current_dir = os.path.dirname(os.path.abspath(__file__))
    dataset_path = os.path.join(current_dir, 'annotations_output_pid', 'dataset.yaml')
    
    if not os.path.exists(dataset_path):
        print(f"Error: {dataset_path} not found. Please run create_training_config.py first.")
    else:
        train_yolo_model(dataset_path, base_model='yolov8m.pt', epochs=50, imgsz=640, batch=16, export_format='onnx')
