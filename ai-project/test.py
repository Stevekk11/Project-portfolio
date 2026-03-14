import os
from ultralytics import YOLO

def test_model():
    # Load the trained model
    model_path = 'trained-model.pt'
    if not os.path.exists(model_path):
        print(f"Error: {model_path} not found.")
        return

    model = YOLO(model_path)

    # Path to test images and output
    test_images_dir = 'test_images'
    output_dir = 'test_results'
    
    if not os.path.exists(test_images_dir):
        print(f"Error: {test_images_dir} folder not found.")
        return

    # Create output directory if it doesn't exist
    if not os.path.exists(output_dir):
        os.makedirs(output_dir)
        print(f"Created output directory: {output_dir}")

    # Get list of images in test_images
    image_files = [f for f in os.listdir(test_images_dir) if f.lower().endswith(('.jpg', '.jpeg', '.png'))]
    
    if not image_files:
        print(f"No images found in {test_images_dir}.")
        return

    print(f"Testing on {len(image_files)} images from {test_images_dir}...")

    # Run inference
    for img_name in image_files:
        img_path = os.path.join(test_images_dir, img_name)
        print(f"Processing {img_name}...")
        results = model(img_path)
        
        # Save results locally in the output folder
        for result in results:
            output_path = os.path.join(output_dir, f"result_{img_name}")
            result.save(filename=output_path)
            print(f"Result saved to {output_path}")

if __name__ == "__main__":
    test_model()
