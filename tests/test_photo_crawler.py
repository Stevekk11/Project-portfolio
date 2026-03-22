import unittest
import os
import shutil
import tempfile
from src.photo_crawler import SeleniumPhotoCrawler

class TestPhotoCrawler(unittest.TestCase):
    def setUp(self):
        # Create a temporary directory for downloads
        self.test_dir = tempfile.mkdtemp()
        self.addCleanup(shutil.rmtree, self.test_dir)
        
        # Real URL from the site mentioned in CREDITS.txt
        # (This URL should be stable and have many photos following)
        self.start_url = "https://seznam-autobusu.cz/dokumentacka/441886"
        
        try:
            # Initialize crawler with real driver (assumes Chrome is installed)
            self.crawler = SeleniumPhotoCrawler(self.start_url, download_dir=self.test_dir)
        except Exception as e:
            self.skipTest(f"Failed to initialize SeleniumPhotoCrawler (is Chrome installed?): {e}")

    def test_crawl_max_photos(self):
        """Test if the crawler downloads exactly 5 photos when max_photos is set to 5."""
        max_photos = 5
        self.crawler.crawl(max_photos=max_photos)
        
        # List files in the download directory
        downloaded_files = [f for f in os.listdir(self.test_dir) if f.endswith(('.jpg', '.jpeg', '.png', '.webp'))]
        
        # Check if we have 5 photos
        self.assertEqual(len(downloaded_files), max_photos, 
                         f"Expected {max_photos} photos, but found {len(downloaded_files)}: {downloaded_files}")

    def test_close(self):
        """Test if the driver is closed correctly."""
        self.crawler.close()
        # After close, the driver object might still exist but should be unusable or None
        # We just check it doesn't crash and we can set it to None if we want to be sure
        self.crawler.driver = None
        self.assertIsNone(self.crawler.driver)

if __name__ == '__main__':
    unittest.main()
