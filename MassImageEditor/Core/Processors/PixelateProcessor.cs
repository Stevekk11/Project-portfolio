namespace MassImageEditor.Core.Processors;

public class PixelateProcessor : IImageProcessor
{
    private int _blockSize;

    public PixelateProcessor(int blockSize)
    {
        _blockSize = blockSize;
    }

    public Bitmap Process(Bitmap image, IProgressReporter? progressReporter)
    {
        if (!ShouldProcess)
            return image;

        int blockSize = _blockSize;
        Bitmap result = new Bitmap(image.Width, image.Height);
        //create blocks of size blockSize x blockSize
        for (int y = 0; y < image.Height; y += blockSize)
        {
            for (int x = 0; x < image.Width; x += blockSize)
            {
                // Compute average color of the block
                int avgR = 0, avgG = 0, avgB = 0;
                int pixelCount = 0;

                for (int dy = 0; dy < blockSize && (y + dy) < image.Height; dy++)
                {
                    for (int dx = 0; dx < blockSize && (x + dx) < image.Width; dx++)
                    {
                        Color pixel = image.GetPixel(x + dx, y + dy);
                        avgR += pixel.R;
                        avgG += pixel.G;
                        avgB += pixel.B;
                        pixelCount++;
                    }
                }

                avgR /= pixelCount;
                avgG /= pixelCount;
                avgB /= pixelCount;
                Color avgColor = Color.FromArgb(avgR, avgG, avgB);

                // Paint the block with average color
                for (int dy = 0; dy < blockSize && (y + dy) < image.Height; dy++)
                {
                    for (int dx = 0; dx < blockSize && (x + dx) < image.Width; dx++)
                    {
                        result.SetPixel(x + dx, y + dy, avgColor);
                    }
                }
            }
            // Report progress after each row of blocks
            progressReporter?.ReportProgress((int)(((double)(y + blockSize) * 100 / image.Height)), $"Processed row starting at {y} of {image.Height}");
        }
        return result;
    }

    public bool ShouldProcess => _blockSize > 0;
}