import fetch from 'node-fetch';
import fs from 'fs';
import path from 'path';
import { fileURLToPath } from 'url';

const __dirname = path.dirname(fileURLToPath(import.meta.url));
const DOWNLOAD_DIR = path.join(__dirname, '..', 'ai-project (IMPORTANT)\\tram_photos_other');
const BASE_URL = 'https://foto.prazsketramvaje.cz';
const START_URL = `${BASE_URL}/ostrava.html`;

// ===== CONFIGURATION =====
// Example: [2024, 2023] or [2024] or null for all
const TARGET_YEARS = [2012, 2011];

// Maximum number of galleries to download (null for all)
const MAX_GALLERIES = 20;

// Maximum images per gallery
const MAX_IMAGES_PER_GALLERY = 100;
// ===== END CONFIGURATION =====

// Create download directory
if (!fs.existsSync(DOWNLOAD_DIR)) {
    fs.mkdirSync(DOWNLOAD_DIR, { recursive: true });
}

function delay(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

async function downloadFile(url, filename, retries = 0) {
    try {
        const response = await fetch(url);
        if (!response.ok) throw new Error(`HTTP ${response.status}`);

        const buffer = await response.buffer();
        const filepath = path.join(DOWNLOAD_DIR, filename);
        fs.writeFileSync(filepath, buffer);
        console.log(`✓ Downloaded: ${filename}`);
        return true;
    } catch (error) {
        if (retries < 2) {
            await delay(1000);
            return downloadFile(url, filename, retries + 1);
        }
        console.error(`✗ Failed: ${filename}`);
        return false;
    }
}

function extractDateFromUrl(url) {
    // Extract YYYY and MM from /jine/kosice/YYYY/MM/index.php
    const dateMatch = url.match(/\/jine\/ostrava\/(\d{4})\/(\d{2})/);
    if (dateMatch) {
        return {
            year: parseInt(dateMatch[1]),
            month: parseInt(dateMatch[2]),
            dateString: `${dateMatch[1]}-${dateMatch[2]}`
        };
    }
    return null;
}

async function getGalleryLinks() {
    try {
        console.log(`Fetching main gallery page: ${START_URL}`);
        const response = await fetch(START_URL);
        const html = await response.text();

        const allGalleries = [];

        // Extract gallery links from href="/jine/kosice/XXXX/XX/index.php"
        const galleryRegex = /href=["']([^"']*\/jine\/ostrava\/[^"']+\/index\.php)["']/gi;
        let match;

        while ((match = galleryRegex.exec(html)) !== null) {
            let url = match[1];
            if (!url.startsWith('http')) {
                url = new URL(url, START_URL).href;
            }
            if (!allGalleries.includes(url)) {
                allGalleries.push(url);
            }
        }

        console.log(`Found ${allGalleries.length} total gallery pages`);

        // Reverse to process newest galleries first (they appear at the end of the page)
        allGalleries.reverse();

        // Filter by year if TARGET_YEARS is specified
        let filteredGalleries = allGalleries;
        if (TARGET_YEARS !== null) {
            filteredGalleries = allGalleries.filter(url => {
                const dateInfo = extractDateFromUrl(url);
                return dateInfo && TARGET_YEARS.includes(dateInfo.year);
            });
            console.log(`Filtered to ${filteredGalleries.length} galleries from year(s): ${TARGET_YEARS.join(', ')}`);
        }

        console.log();
        return filteredGalleries;
    } catch (error) {
        console.error('Error fetching galleries:', error.message);
        return [];
    }
}

async function getImagesFromGallery(galleryUrl) {
    try {
        const response = await fetch(galleryUrl);
        const html = await response.text();

        const images = [];

        // Extract image filenames from src='tn_*.jpg' tags and convert to full-size image names
        // The gallery PHP generates IMG SRC='tn_FILENAME.jpg' but the actual images are FILENAME.jpg
        const thumbnailRegex = /SRC=["']tn_([^"']+\.(?:jpg|jpeg|png|gif|webp))["']/gi;
        let match;

        while ((match = thumbnailRegex.exec(html)) !== null) {
            const imageFilename = match[1]; // e.g., "100-1.jpg"
            // Construct the full URL to the actual (non-thumbnail) image
            let imageUrl = new URL(imageFilename, galleryUrl).href;

            if (!images.includes(imageUrl)) {
                images.push(imageUrl);
            }
        }

        return images;
    } catch (error) {
        console.error(`Error fetching gallery ${galleryUrl}:`, error.message);
        return [];
    }
}

async function main() {
    console.log('Tram Image Scraper for foto.prazsketramvaje.cz\n');

    const galleries = await getGalleryLinks();

    if (galleries.length === 0) {
        console.log('ERROR: No galleries found!');
        return;
    }

    const galleriesToProcess = Math.min(galleries.length, MAX_GALLERIES || galleries.length);
    console.log(`Processing ${galleriesToProcess} galleries...\n`);

    let totalDownloaded = 0;
    let totalSkipped = 0;

    for (let i = 0; i < galleriesToProcess; i++) {
        const dateInfo = extractDateFromUrl(galleries[i]);
        const dateLabel = dateInfo ? `${dateInfo.dateString}` : 'unknown';
        console.log(`[${i + 1}/${galleriesToProcess}] Gallery: ${dateLabel}`);

        const images = await getImagesFromGallery(galleries[i]);
        console.log(`  Found ${images.length} images`);

        if (images.length === 0) {
            totalSkipped++;
            await delay(100);
            continue;
        }

        // Download images
        for (let j = 0; j < Math.min(images.length, MAX_IMAGES_PER_GALLERY); j++) {
            const imageUrl = images[j];
            const filename = `kosice_${dateLabel}_image_${j}${path.extname(new URL(imageUrl).pathname)}`;

            const success = await downloadFile(imageUrl, filename);
            if (success) totalDownloaded++;

            await delay(20);
        }
        await delay(1000);
    }

    console.log(`\n${'='.repeat(50)}`);
    console.log(`Download complete!`);
    console.log(`  Downloaded: ${totalDownloaded} images`);
    console.log(`  Location: ${DOWNLOAD_DIR}`);
    console.log(`${'='.repeat(50)}`);
}

main().catch(console.error);
