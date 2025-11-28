namespace MIETests;

using System.Drawing;
using MassImageEditor.Core.Processors;
using NUnit.Framework;

/// <summary>
/// Represents a test class for handling brightness-related functionalities.
/// </summary>
public class BrightnessTest
{
    private Bitmap _testImage = null!;

    [SetUp]
    public void SetUp()
    {
        _testImage = new Bitmap(100, 100);
        using var g = Graphics.FromImage(_testImage);
        g.Clear(Color.Blue);
    }

    [TearDown]
    public void TearDown()
    {
        _testImage.Dispose();
    }

    [Test]
    public void ShouldProcess_IsTrue_WhenInstantiated()
    {
        var processor = new BrightnessProcessor(50);
        Assert.That(processor.ShouldProcess, Is.True);
    }

    [Test]
    public void Process_ChangesBrightness_WhenProcessing()
    {
        var processor = new BrightnessProcessor(50);
        var result = processor.Process(_testImage);
        var pixel = result.GetPixel(50, 50);
        Assert.That(pixel, Is.Not.EqualTo(_testImage.GetPixel(50, 50)));
    }
}