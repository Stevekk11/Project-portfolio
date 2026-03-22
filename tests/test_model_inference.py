import unittest
import os
import shutil
import tempfile
import cv2
import numpy as np
from pathlib import Path
from lib.inference_service import YoloInferenceService

class TestModelInference(unittest.TestCase):
    def setUp(self):
        # Create a temporary directory for tests
        self.test_dir = tempfile.mkdtemp()
        self.addCleanup(shutil.rmtree, self.test_dir)
        
        # Path to the trained model
        self.model_path = os.path.join("models", "trained-model.pt")
        
        # Skip tests if model is not found
        if not os.path.exists(self.model_path):
            self.skipTest(f"Model {self.model_path} not found. Skipping inference test.")

    def create_dummy_image(self, filename, width=640, height=640):
        path = os.path.join(self.test_dir, filename)
        # Create a gray image
        img = np.full((height, width, 3), 128, np.uint8)
        # Add some random shapes to have something for the model to look at 
        # (even if it doesn't detect anything, we test the pipeline)
        cv2.rectangle(img, (100, 100), (300, 300), (0, 255, 0), -1)
        cv2.circle(img, (400, 400), 50, (0, 0, 255), -1)
        cv2.imwrite(path, img)
        return path

    def test_model_loading(self):
        """Test if the model can be loaded successfully."""
        service = YoloInferenceService(self.model_path)
        self.assertIsNotNone(service.model)
        self.assertTrue(len(service.class_names) > 0)

    def test_image_inference_pipeline(self):
        """Test the full image inference pipeline with the trained model."""
        service = YoloInferenceService(self.model_path)
        image_path = self.create_dummy_image("test_input.jpg")
        
        output_dir = os.path.join(self.test_dir, "output")
        result = service.predict_image(image_path, output_dir)
        
        # Check if output files were created
        self.assertTrue(os.path.exists(result.annotated_path))
        self.assertTrue(os.path.exists(result.metadata_path))
        
        # Check result structure
        self.assertEqual(result.source_path, os.path.abspath(image_path))
        self.assertIsInstance(result.detections, list)
        self.assertIsInstance(result.label_counts, dict)
        
        # Verify annotated image can be read
        annotated_img = cv2.imread(result.annotated_path)
        self.assertIsNotNone(annotated_img)
        self.assertEqual(annotated_img.shape[1], result.image_size[0])
        self.assertEqual(annotated_img.shape[0], result.image_size[1])

if __name__ == '__main__':
    unittest.main()
