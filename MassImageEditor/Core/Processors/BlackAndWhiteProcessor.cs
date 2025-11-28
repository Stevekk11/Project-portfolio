namespace MassImageEditor.Core.Processors;

public class BlackAndWhiteProcessor : IImageProcessor
{
    public BlackAndWhiteProcessor(bool shouldProcess)
    {
        ShouldProcess = shouldProcess;
    }
    public Bitmap Process(Bitmap image)
    {
        if (!ShouldProcess)
            return image;
        //make an empty bitmap the same size as original
        Bitmap newBitmap = new Bitmap(image.Width, image.Height);

        for (int i = 0; i < image.Width; i++)
        {
            for (int j = 0; j < image.Height; j++)
            {
                //get the pixel from the original image
                Color originalColor = image.GetPixel(i, j);

                //create the grayscale version of the pixel
                int grayScale = (int)((originalColor.R * .3) + (originalColor.G * .59)
                                                             + (originalColor.B * .11));

                //create the color object
                Color newColor =  Color.FromArgb(grayScale, grayScale, grayScale);

                //set the new image's pixel to the grayscale version
                newBitmap.SetPixel(i, j, newColor);
            }
        }

        return newBitmap;

    }

    public bool ShouldProcess { get; }
}