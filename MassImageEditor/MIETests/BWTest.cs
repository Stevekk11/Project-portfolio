namespace MIETests;

using System.Drawing;
using MassImageEditor.Core.Processors;
using NUnit.Framework;

/// <summary>
/// Contains unit tests for the BlackAndWhiteProcessor class to verify its functionality.
/// </summary>
public class BWTest
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
        var processor = new BlackAndWhiteProcessor(true);
        Assert.That(processor.ShouldProcess, Is.True);
    }

    [Test]
    public void Process_ConvertsToBW_WhenProcessing()
    {
        var processor = new BlackAndWhiteProcessor(true);
        var result = processor.Process(_testImage);
        var pixel = result.GetPixel(50, 50);
        Assert.That(pixel.R, Is.EqualTo(pixel.G));
        Assert.That(pixel.G, Is.EqualTo(pixel.B));
    }
}