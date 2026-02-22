from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
from selenium.common.exceptions import TimeoutException, NoSuchElementException
from selenium.webdriver.chrome.service import Service
from webdriver_manager.chrome import ChromeDriverManager
import requests
import os
import time
import logging

# Set up logging
logging.basicConfig(
    level=logging.INFO,
    format='%(asctime)s - %(levelname)s - %(message)s'
)
logger = logging.getLogger(__name__)


class SeleniumPhotoCrawler:
    def __init__(self, start_url, download_dir='downloaded_photos', delay=2):
        """
        Initialize the photo crawler with Selenium.

        Args:
            start_url: The starting URL of the photo gallery
            download_dir: Directory to save downloaded images
            delay: Delay between requests in seconds
        """
        self.start_url = start_url
        self.download_dir = download_dir
        self.delay = delay
        self.visited_urls = set()

        # Create download directory if it doesn't exist
        os.makedirs(self.download_dir, exist_ok=True)

        # Set up Selenium WebDriver
        self.driver = None
        self._setup_driver()

    def _setup_driver(self):
        """Set up the Selenium WebDriver with Chrome."""
        options = webdriver.ChromeOptions()
        options.add_argument('--disable-blink-features=AutomationControlled')
        options.add_argument(
            '--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36')
        options.add_experimental_option("excludeSwitches", ["enable-automation"])
        options.add_experimental_option('useAutomationExtension', False)
        options.add_argument('--headless')

        try:
            # Automatically install and use ChromeDriver
            service = Service(ChromeDriverManager().install())
            self.driver = webdriver.Chrome(service=service, options=options)

            self.driver.execute_script("Object.defineProperty(navigator, 'webdriver', {get: () => undefined})")
            logger.info("Chrome WebDriver initialized successfully")
        except Exception as e:
            logger.error(f"Failed to initialize Chrome WebDriver: {e}")
            logger.info("Make sure you have Chrome browser installed")
            raise

    def download_image(self, image_url, photo_id):
        """
        Download an image from the given URL.

        Args:
            image_url: URL of the image to download
            photo_id: Identifier for the photo (used in filename)

        Returns:
            True if successful, False otherwise
        """
        try:
            # Get the image file extension
            ext = os.path.splitext(image_url.split('?')[0])[1] or '.jpg'
            filename = f"{photo_id}{ext}"
            filepath = os.path.join(self.download_dir, filename)

            # Skip if already downloaded
            if os.path.exists(filepath):
                logger.info(f"Image {filename} already exists, skipping...")
                return True

            # Download the image using requests with browser cookies
            logger.info(f"Downloading {image_url}...")

            # Get cookies from Selenium session
            cookies = self.driver.get_cookies()
            session = requests.Session()
            for cookie in cookies:
                session.cookies.set(cookie['name'], cookie['value'])

            # Add headers
            session.headers.update({
                'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36',
                'Referer': self.driver.current_url
            })

            response = session.get(image_url, timeout=30)
            response.raise_for_status()

            # Save the image
            with open(filepath, 'wb') as f:
                f.write(response.content)

            logger.info(f"Saved: {filename} ({len(response.content)} bytes)")
            return True

        except Exception as e:
            logger.error(f"Failed to download {image_url}: {e}")
            return False

    def extract_photo_data(self):
        """
        Extract photo data from the current page.

        Returns:
            Dictionary with image_url, next_url, prev_url, photo_id, has_next, has_prev
        """
        data = {
            'image_url': None,
            'next_url': None,
            'prev_url': None,
            'photo_id': None,
            'has_next': False,
            'has_prev': False
        }

        try:
            # Wait for the photo to load
            WebDriverWait(self.driver, 10).until(
                EC.presence_of_element_located((By.CSS_SELECTOR, '.photo-container img, #photo img'))
            )

            # Extract the main image URL
            try:
                img_element = self.driver.find_element(By.CSS_SELECTOR, '.photo-container img, #photo img')
                data['image_url'] = img_element.get_attribute('src')
            except NoSuchElementException:
                logger.warning("Could not find image element")

            # Extract photo ID from current URL
            current_url = self.driver.current_url
            if '/dokumentacka/' in current_url:
                data['photo_id'] = current_url.split('/dokumentacka/')[-1].split('?')[0].split('#')[0]

            # Find the "next photo" link (right-link)
            try:
                next_link = self.driver.find_element(By.CSS_SELECTOR, 'a#right-link, a.side-link[href*="dokumentacka"]')

                # Check if the link exists and is not a placeholder
                if next_link and next_link.get_attribute('href'):
                    data['next_url'] = next_link.get_attribute('href')
                    data['has_next'] = True
                    logger.info(f"Found next photo link")
            except NoSuchElementException:
                logger.info("No next photo link found (might be the last photo)")
                data['has_next'] = False

            # Find the "previous photo" link (left-link)
            try:
                prev_link = self.driver.find_element(By.CSS_SELECTOR, 'a#left-link, a.side-link[href*="dokumentacka"]')
                if prev_link and prev_link.get_attribute('href'):
                    data['prev_url'] = prev_link.get_attribute('href')
                    data['has_prev'] = True
                    logger.info(f"Found previous photo link")
            except NoSuchElementException:
                logger.info("No previous photo link found (might be the first photo)")
                data['has_prev'] = False

        except TimeoutException:
            logger.error("Timeout waiting for page elements to load")
        except Exception as e:
            logger.error(f"Error extracting photo data: {e}")

        return data

    def crawl(self, max_photos=None, go_backwards=False):
        """
        Start crawling from the start_url.

        Args:
            max_photos: Maximum number of photos to download (None for unlimited)
            go_backwards: If True, crawl in reverse order using the 'left-link'.
        """
        photo_count = 0

        logger.info(f"Starting crawl from: {self.start_url}")
        logger.info(f"Download directory: {os.path.abspath(self.download_dir)}")
        logger.info(f"Crawling direction: {'Backwards' if go_backwards else 'Forwards'}")

        try:
            # Navigate to the starting URL
            logger.info(f"\nNavigating to: {self.start_url}")
            self.driver.get(self.start_url)
            time.sleep(3)  # Wait for page to fully load

            while True:
                current_url = self.driver.current_url

                # Check if we've already visited this URL
                if current_url in self.visited_urls:
                    logger.info("Detected loop or end of gallery (visited URL again)")
                    break

                # Check if we've reached the max photos limit
                if max_photos and photo_count >= max_photos:
                    logger.info(f"Reached maximum photo limit ({max_photos})")
                    break

                self.visited_urls.add(current_url)

                # Extract photo data from current page
                data = self.extract_photo_data()

                # Download the image
                if data['image_url'] and data['photo_id']:
                    success = self.download_image(data['image_url'], data['photo_id'])
                    if success:
                        photo_count += 1
                        logger.info(f"Progress: {photo_count} photos downloaded")
                else:
                    logger.warning("No image found on this page")

                # Determine next navigation based on go_backwards flag
                if go_backwards:
                    if not data['has_prev'] or not data['prev_url']:
                        logger.info("\nNo more photos to crawl (reached the beginning)")
                        break
                    nav_url = data['prev_url']
                    nav_link_selector = 'a#left-link'
                else:
                    if not data['has_next'] or not data['next_url']:
                        logger.info("\nNo more photos to crawl (reached the end)")
                        break
                    nav_url = data['next_url']
                    nav_link_selector = 'a#right-link'

                # Navigate to next/previous photo
                logger.info(f"\nNavigating {'previous' if go_backwards else 'next'} photo...")
                try:
                    # Click the next/previous link
                    nav_link = self.driver.find_element(By.CSS_SELECTOR, nav_link_selector)
                    nav_link.click()

                    # Wait for new page to load
                    time.sleep(self.delay)

                    # Wait for URL to change
                    WebDriverWait(self.driver, 10).until(
                        lambda driver: driver.current_url != current_url
                    )

                except Exception as e:
                    logger.error(f"Failed to navigate {'previous' if go_backwards else 'next'} photo: {e}")
                    break

        except Exception as e:
            logger.error(f"Crawling error: {e}")
        finally:
            logger.info(f"\nCrawling complete! Downloaded {photo_count} photos.")
            logger.info(f"Photos saved to: {os.path.abspath(self.download_dir)}")
            self.close()

    def close(self):
        """Close the browser."""
        if self.driver:
            self.driver.quit()
            logger.info("Browser closed")


def main(forward=True, url="", max_photos=None):
    start_url = url

    if forward:
        logger.info("\n--- Starting FORWARDS crawling ---")
        crawler_forward = SeleniumPhotoCrawler(
            start_url=start_url,
            download_dir='bus_photos_forward',
            delay=0.5
        )
        try:
            crawler_forward.crawl(max_photos=max_photos)
        except Exception as e:
            logger.error(f"Forward crawling error: {e}")
        finally:
            crawler_forward.close()

    else:
        logger.info("\n--- Starting BACKWARDS crawling ---")
        crawler_backward = SeleniumPhotoCrawler(
            start_url=start_url,
            download_dir='bus_photos_pid',
            delay=0.5
        )
        try:
            crawler_backward.crawl(max_photos=max_photos, go_backwards=True)
        except Exception as e:
            logger.error(f"Backward crawling error: {e}")
        finally:
            crawler_backward.close()


if __name__ == "__main__":
    main(False,"https://seznam-autobusu.cz/dokumentacka/346388?searchParameters%5BcarOperatorId%5D=106&searchPresenter=Busy%3APhotoList&listUrl=https%3A%2F%2Fseznam-autobusu.cz%2Ffotky%3Fiddopravce%3D106%26strana%3D400#photo", max_photos=300)
