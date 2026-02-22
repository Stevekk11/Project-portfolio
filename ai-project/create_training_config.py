"""
Helper script to create YAML configuration for YOLOv8 training

This script creates a dataset.yaml file needed for training a new YOLOv8 model
on custom annotated data.
"""

import os
import json
import yaml
from pathlib import Path


def create_dataset_yaml(dataset_path: str, output_name: str = 'dataset.yaml'):
    """
    Create a dataset.yaml file for YOLOv8 training.

    Args:
        dataset_path: Path to the dataset root directory (contains 'images' and 'labels' folders)
        output_name: Name of the output YAML file (default: dataset.yaml)
    """
    dataset_path = os.path.abspath(dataset_path)

    # Check if required directories exist
    images_dir = os.path.join(dataset_path, 'images')
    labels_dir = os.path.join(dataset_path, 'labels')
    classes_file = os.path.join(dataset_path, 'classes.txt')

    if not os.path.isdir(images_dir):
        print(f"Error: {images_dir} not found")
        return False

    if not os.path.isdir(labels_dir):
        print(f"Error: {labels_dir} not found")
        return False

    if not os.path.exists(classes_file):
        print(f"Error: {classes_file} not found")
        return False

    # Read class names
    classes = {}
    with open(classes_file, 'r', encoding='utf-8') as f:
        for idx, line in enumerate(f):
            class_name = line.strip()
            if class_name:
                classes[idx] = class_name

    # Create YAML content
    yaml_content = {
        'path': dataset_path,
        'train': os.path.join(dataset_path, 'images'),
        'val': os.path.join(dataset_path, 'images'),  # You can split this later
        'test': os.path.join(dataset_path, 'images'),  # Optional
        'nc': len(classes),
        'names': classes
    }

    # Write YAML file
    yaml_path = os.path.join(dataset_path, output_name)
    with open(yaml_path, 'w', encoding='utf-8') as f:
        yaml.dump(yaml_content, f, default_flow_style=False, allow_unicode=True)

    print(f"Created {yaml_path}")
    print(f"\nDataset configuration:")
    print(f"  Path: {dataset_path}")
    print(f"  Classes: {len(classes)}")
    for idx, name in sorted(classes.items()):
        print(f"    {idx}: {name}")

    return True


def create_simple_yaml(output_path: str = 'dataset.yaml',
                      train_images: str = './images',
                      val_images: str = './images',
                      classes: dict = None):
    """
    Create a simple dataset.yaml file with custom paths.

    Args:
        output_path: Where to save the YAML file
        train_images: Path to training images
        val_images: Path to validation images
        classes: Dictionary of class_id: class_name
    """
    if classes is None:
        classes = {0: 'autobus_pid', 1: 'tramvaj_pid'}

    yaml_content = {
        'train': train_images,
        'val': val_images,
        'nc': len(classes),
        'names': classes
    }

    with open(output_path, 'w', encoding='utf-8') as f:
        yaml.dump(yaml_content, f, default_flow_style=False, allow_unicode=True)

    print(f"Created {output_path}")
    return True


def main():
    import sys

    print("="*60)
    print("YOLO Dataset YAML Creator")
    print("="*60)

    if len(sys.argv) > 1:
        dataset_path = sys.argv[1]
    else:
        dataset_path = input("\nEnter the path to dataset directory (with images/ and labels/): ").strip()

    if not dataset_path:
        print("No path provided")
        return

    create_dataset_yaml(dataset_path)


if __name__ == "__main__":
    main()

