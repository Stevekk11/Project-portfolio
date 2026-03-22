import unittest
import os
import shutil
import tempfile
import yaml
from src.create_training_config import create_simple_yaml, create_dataset_yaml

class TestCreateTrainingConfig(unittest.TestCase):
    def setUp(self):
        # Create a temporary directory for tests
        self.test_dir = tempfile.mkdtemp()
        self.addCleanup(shutil.rmtree, self.test_dir)

    def test_create_simple_yaml(self):
        output_path = os.path.join(self.test_dir, "dataset.yaml")
        classes = {0: "test_class"}
        success = create_simple_yaml(output_path=output_path, classes=classes)
        
        self.assertTrue(success)
        self.assertTrue(os.path.exists(output_path))
        
        # Verify YAML content
        with open(output_path, 'r', encoding='utf-8') as f:
            data = yaml.safe_load(f)
            self.assertEqual(data['nc'], 1)
            self.assertEqual(data['names'], classes)

    def test_create_dataset_yaml(self):
        # Create necessary structure
        dataset_path = os.path.join(self.test_dir, "my_dataset")
        images_dir = os.path.join(dataset_path, 'images')
        labels_dir = os.path.join(dataset_path, 'labels')
        os.makedirs(images_dir)
        os.makedirs(labels_dir)
        
        classes_file = os.path.join(dataset_path, 'classes.txt')
        with open(classes_file, 'w', encoding='utf-8') as f:
            f.write("class1\nclass2\n")

        success = create_dataset_yaml(dataset_path, output_name='my_test.yaml')
        
        self.assertTrue(success)
        yaml_path = os.path.join(dataset_path, 'my_test.yaml')
        self.assertTrue(os.path.exists(yaml_path))
        
        with open(yaml_path, 'r', encoding='utf-8') as f:
            data = yaml.safe_load(f)
            self.assertEqual(data['nc'], 2)
            self.assertEqual(data['names'], {0: 'class1', 1: 'class2'})
            # path is absolute in the implementation
            self.assertEqual(data['path'], os.path.abspath(dataset_path))

    def test_create_dataset_yaml_missing_dir(self):
        # Empty dir without images/labels
        dataset_path = os.path.join(self.test_dir, "empty")
        os.makedirs(dataset_path)
        
        success = create_dataset_yaml(dataset_path)
        self.assertFalse(success)

if __name__ == '__main__':
    unittest.main()
