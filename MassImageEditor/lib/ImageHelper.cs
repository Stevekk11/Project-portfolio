using System.Drawing.Imaging;

namespace MassImageEditor.lib;

public class ImageHelper
{
    public static Bitmap Sharpen(Bitmap image, float sharpness)
    {
        // Clamp sharpness 1–100 → 0.0–1.0 intensity scale
        float s = Math.Clamp(sharpness / 100f, 0f, 1f);

        // Sharpen kernel (unsharp mask style)
        // Strength grows with s; base weights tuned for natural sharpness
        float w = 1f + 4f * s;
        float[,] kernel =
        {
            { 0, -s, 0 },
            { -s, w, -s },
            { 0, -s, 0 }
        };

        // Apply 3×3 convolution
        Bitmap output = new Bitmap(image.Width, image.Height);
        var rect = new Rectangle(0, 0, image.Width, image.Height);
        var src = image.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
        var dst = output.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

        unsafe
        {
            int stride = src.Stride;
            byte* pSrc = (byte*)src.Scan0;
            byte* pDst = (byte*)dst.Scan0;

            for (int y = 1; y < image.Height - 1; y++)
            {
                for (int x = 1; x < image.Width - 1; x++)
                {
                    float b = 0, g = 0, r = 0;

                    for (int ky = -1; ky <= 1; ky++)
                    for (int kx = -1; kx <= 1; kx++)
                    {
                        byte* px = pSrc + (y + ky) * stride + (x + kx) * 3;
                        float k = kernel[ky + 1, kx + 1];
                        b += px[0] * k;
                        g += px[1] * k;
                        r += px[2] * k;
                    }

                    byte* pd = pDst + y * stride + x * 3;
                    pd[0] = (byte)Math.Clamp((int)b, 0, 255);
                    pd[1] = (byte)Math.Clamp((int)g, 0, 255);
                    pd[2] = (byte)Math.Clamp((int)r, 0, 255);
                }
            }
        }

        image.UnlockBits(src);
        output.UnlockBits(dst);
        return output;
    }


}