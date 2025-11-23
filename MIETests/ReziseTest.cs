namespace MIETests;

using System.Drawing;
using MassImageEditor.Core.Processors;
using NUnit.Framework;

public class ResizeTest
{
    private Bitmap _landscape = null!;
    private Bitmap _portrait = null!;
    private Bitmap _square = null!;

    [SetUp]
    public void SetUp()
    {
        _landscape = new Bitmap(400, 200); // ratio 2.0
        _portrait = new Bitmap(200, 400);  // ratio 0.5
        _square = new Bitmap(300, 300);    // ratio 1.0

        using (var g = Graphics.FromImage(_landscape))
        {
            g.Clear(Color.Blue);
        }

        using (var g = Graphics.FromImage(_portrait))
        {
            g.Clear(Color.Green);
        }

        using (var g = Graphics.FromImage(_square))
        {
            g.Clear(Color.Red);
        }
    }

    [TearDown]
    public void TearDown()
    {
        _landscape.Dispose();
        _portrait.Dispose();
        _square.Dispose();
    }

    [Test]
    public void ShouldProcess_IsFalse_WhenTargetDimensionsZero()
    {
        var processor = new ResizeProcessor(0, 0);

        Assert.That(processor.ShouldProcess, Is.False);
    }

    [Test]
    public void ShouldProcess_IsTrue_WhenTargetDimensionsSet()
    {
        var processor = new ResizeProcessor(1920, 1080);

        Assert.That(processor.ShouldProcess, Is.True);
    }

    [Test]
    public void Process_ReturnsSameInstance_WhenShouldProcessFalse()
    {
        var processor = new ResizeProcessor(0, 0);

        var result = processor.Process(_square);

        Assert.That(ReferenceEquals(result, _square), Is.True);
    }

    [Test]
    public void Process_ResizesLandscape_AndMaintainsAspectWithLetterbox()
    {
        var processor = new ResizeProcessor(400, 400);

        var result = processor.Process(_landscape);

        Assert.That(result.Width, Is.EqualTo(400));
        Assert.That(result.Height, Is.EqualTo(400));

        // Middle row should be non-black, top and bottom might be letterbox (black)
        var middlePixel = result.GetPixel(200, 200);
        Assert.That(middlePixel.ToArgb(), Is.Not.EqualTo(Color.Black.ToArgb()));
    }

    [Test]
    public void Process_ResizesPortrait_AndMaintainsAspectWithLetterbox()
    {
        var processor = new ResizeProcessor(400, 400);

        var result = processor.Process(_portrait);

        Assert.That(result.Width, Is.EqualTo(400));
        Assert.That(result.Height, Is.EqualTo(400));

        var middlePixel = result.GetPixel(200, 200);
        Assert.That(middlePixel.ToArgb(), Is.Not.EqualTo(Color.Black.ToArgb()));
    }

    [Test]
    public void Process_ResizesSquare_ExactlyToTarget()
    {
        var processor = new ResizeProcessor(500, 500);

        var result = processor.Process(_square);

        Assert.That(result.Width, Is.EqualTo(500));
        Assert.That(result.Height, Is.EqualTo(500));

        // Corners should not be black since aspect ratio is same
        Assert.That(result.GetPixel(0, 0).ToArgb(), Is.Not.EqualTo(Color.Black.ToArgb()));
        Assert.That(result.GetPixel(499, 499).ToArgb(), Is.Not.EqualTo(Color.Black.ToArgb()));
    }
}