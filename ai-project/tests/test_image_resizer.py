import unittest
import os
import shutil
import tempfile
import cv2
import numpy as np
from pathlib import Path
from src.image_resizer import get_image_files, get_smallest_image_dimensions

class TestImageResizer(unittest.TestCase):
    def setUp(self):
        # Create a temporary directory for test images
        self.test_dir = tempfile.mkdtemp()
        self.addCleanup(shutil.rmtree, self.test_dir)

    def create_dummy_image(self, filename, width, height):
        path = os.path.join(self.test_dir, filename)
        img = np.zeros((height, width, 3), np.uint8)
        cv2.imwrite(path, img)
        return path

    def test_get_image_files(self):
        # Create some dummy files
        self.create_dummy_image("test1.jpg", 100, 100)
        self.create_dummy_image("test2.png", 200, 200)
        with open(os.path.join(self.test_dir, "test.txt"), "w") as f:
            f.write("not an image")

        files = get_image_files(self.test_dir)
        # Convert to names for easier comparison
        file_names = [f.name for f in files]
        
        self.assertIn("test1.jpg", file_names)
        self.assertIn("test2.png", file_names)
        self.assertNotIn("test.txt", file_names)
        self.assertEqual(len(files), 2)

    def test_get_smallest_image_dimensions(self):
        # Create images of different sizes
        img1 = self.create_dummy_image("large.jpg", 1000, 800)
        img2 = self.create_dummy_image("small.jpg", 100, 50)
        img3 = self.create_dummy_image("medium.jpg", 500, 500)

        image_files = [Path(img1), Path(img2), Path(img3)]
        min_dims, smallest_image = get_smallest_image_dimensions(image_files)

        self.assertEqual(min_dims, (100, 50))
        self.assertEqual(smallest_image.name, "small.jpg")

    def test_get_smallest_image_dimensions_empty(self):
        min_dims, smallest_image = get_smallest_image_dimensions([])
        self.assertEqual(min_dims, (0, 0))
        self.assertIsNone(smallest_image)

if __name__ == '__main__':
    unittest.main()
