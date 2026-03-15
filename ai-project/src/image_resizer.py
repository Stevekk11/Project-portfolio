import cv2
import os
import argparse
from pathlib import Path
import logging

# Set up logging
logging.basicConfig(
    level=logging.INFO,
    format='%(asctime)s - %(levelname)s - %(message)s'
)
logger = logging.getLogger(__name__)

def get_image_files(directory):
    """Get all image files from a directory."""
    valid_extensions = ('.jpg', '.jpeg', '.png', '.bmp', '.tiff', '.webp')
    return [f for f in Path(directory).iterdir() if f.suffix.lower() in valid_extensions]

def get_smallest_image_dimensions(image_files):
    """Find the dimensions (width, height) of the image with the smallest area."""
    min_area = float('inf')
    min_dims = (0, 0)
    smallest_image = None

    for img_path in image_files:
        try:
            # We use imread with IMREAD_UNCHANGED to avoid potential color conversions during initial check
            # but for just getting size, we can also use cv2.imread(..., 0) or similar
            img = cv2.imread(str(img_path))
            if img is not None:
                height, width = img.shape[:2]
                area = width * height
                if area < min_area:
                    min_area = area
                    min_dims = (width, height)
                    smallest_image = img_path
        except Exception as e:
            logger.warning(f"Could not read image {img_path}: {e}")

    return min_dims, smallest_image

def resize_images(image_files, target_dims, output_dir):
    """Resize images to the target dimensions while keeping aspect ratio."""
    target_w, target_h = target_dims
    
    if not os.path.exists(output_dir):
        os.makedirs(output_dir)
        logger.info(f"Created output directory: {output_dir}")

    for img_path in image_files:
        try:
            img = cv2.imread(str(img_path))
            if img is None:
                continue

            h, w = img.shape[:2]
            
            # Calculate scaling factor to fit within target_w, target_h while keeping aspect ratio
            # Use the more restrictive scale factor
            scale = min(target_w / w, target_h / h)
            
            new_w = int(w * scale)
            new_h = int(h * scale)
            
            resized_img = cv2.resize(img, (new_w, new_h), interpolation=cv2.INTER_AREA)
            
            # Save the resized image
            output_path = Path(output_dir) / img_path.name
            cv2.imwrite(str(output_path), resized_img)
            # logger.info(f"Resized {img_path.name} to {new_w}x{new_h}")
            
        except Exception as e:
            logger.error(f"Error processing {img_path}: {e}")

def main():
    parser = argparse.ArgumentParser(description='Resize all images in a folder to the size of the smallest one, keeping aspect ratio.')
    parser.add_argument('input_dir', nargs='?', help='Directory containing images to resize', default=None)
    parser.add_argument('--output_dir', help='Directory to save resized images (default: input_dir_resized)', default=None)
    
    args = parser.parse_args()
    
    input_dir = args.input_dir
    if not input_dir:
        input_dir = input("Enter the input directory containing images: ").strip()
    
    if not input_dir:
        logger.error("No input directory provided.")
        return

    output_dir = args.output_dir
    if not output_dir:
        default_output = f"{input_dir.rstrip('/\\')}_resized"
        output_dir = input(f"Enter the output directory (default: {default_output}): ").strip()
        if not output_dir:
            output_dir = default_output
    
    image_files = get_image_files(input_dir)
    
    if not image_files:
        logger.error(f"No image files found in {input_dir}")
        return

    logger.info(f"Found {len(image_files)} image files.")
    
    min_dims, smallest_image = get_smallest_image_dimensions(image_files)
    
    if min_dims == (0, 0):
        logger.error("Could not determine smallest image dimensions.")
        return
        
    logger.info(f"Smallest image: {smallest_image} with dimensions {min_dims[0]}x{min_dims[1]}")
    
    resize_images(image_files, min_dims, output_dir)
    logger.info(f"Resizing complete. Saved to: {output_dir}")

if __name__ == "__main__":
    main()
