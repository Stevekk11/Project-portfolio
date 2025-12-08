namespace MIETests;

using System.Drawing;
using MassImageEditor.Core.Processors;
using NUnit.Framework;

/// <summary>
/// Represents a test class for handling pixelation-related functionalities.
/// </summary>
[TestFixture]
public class PixelateTest
{
    private Bitmap _testImage = null!;

    [SetUp]
    public void SetUp()
    {
        _testImage = new Bitmap(100, 100);
        using var g = Graphics.FromImage(_testImage);
        g.Clear(Color.Red);
    }

    [TearDown]
    public void TearDown()
    {
        _testImage.Dispose();
    }

    [Test]
    public void ShouldProcess_IsTrue_WhenBlockSizeIsValid()
    {
        var processor = new PixelateProcessor(10);
        Assert.That(processor.ShouldProcess, Is.True);
    }

    [Test]
    public void Process_PixelatesImage_WhenProcessing()
    {
        var processor = new PixelateProcessor(10);
        var result = processor.Process(_testImage);
        var pixel = result.GetPixel(5, 5);
        Assert.That(pixel, Is.EqualTo(result.GetPixel(9, 9)));
    }
}

