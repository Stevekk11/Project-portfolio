using System.Drawing.Imaging;
using MassImageEditor.lib;

namespace MassImageEditor.Core.Processors;

public class SharpnessProcessor : IImageProcessor
{
    private int _sharpness;

    public SharpnessProcessor(int sharpness)
    {
        _sharpness = sharpness;
        ShouldProcess = sharpness != 0 && sharpness < 101;
    }
    public Bitmap Process(Bitmap image)
    {
        return ImageHelper.Sharpen(image, _sharpness);
    }
    public bool ShouldProcess { get; }
}