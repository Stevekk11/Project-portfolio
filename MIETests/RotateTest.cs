namespace MIETests;

using System.Drawing;
using MassImageEditor.Core.Processors;
using NUnit.Framework;

/// <summary>
/// Contains unit tests for verifying the behavior of the RotateProcessor class.
/// </summary>
public class RotateTest
{
    private Bitmap _source = null!;

    [SetUp]
    public void SetUp()
    {
        _source = new Bitmap(100, 50); // width != height so rotation is visible
        using var g = Graphics.FromImage(_source);
        g.Clear(Color.White);

        // Draw a colored pixel in a known place so we can verify rotation
        _source.SetPixel(10, 20, Color.Red);
    }

    [TearDown]
    public void TearDown()
    {
        _source.Dispose();
    }

    [Test]
    public void ShouldProcess_IsFalse_WhenAngleIsZero()
    {
        var processor = new RotateProcessor(0);

        Assert.That(processor.ShouldProcess, Is.False);
    }

    [Test]
    public void ShouldProcess_IsTrue_WhenAngleIsNonZero()
    {
        var processor = new RotateProcessor(90);

        Assert.That(processor.ShouldProcess, Is.True);
    }

    [Test]
    public void Process_ReturnsSameInstance_WhenShouldProcessFalse()
    {
        var processor = new RotateProcessor(0);

        var result = processor.Process(_source);

        Assert.That(ReferenceEquals(result, _source), Is.True);
    }

    [TestCase(90)]
    [TestCase(180)]
    [TestCase(270)]
    public void Process_RotatesImage_ChangesDimensionsForRightAngles(int angle)
    {
        var processor = new RotateProcessor(angle);

        var result = processor.Process(_source);

        if (angle is 90 or 270)
        {
            Assert.That(result.Width, Is.EqualTo(_source.Height));
            Assert.That(result.Height, Is.EqualTo(_source.Width));
        }
        else
        {
            Assert.That(result.Width, Is.EqualTo(_source.Width));
            Assert.That(result.Height, Is.EqualTo(_source.Height));
        }
    }

    [Test]
    public void Process_RotatesPixel_90DegreesClockwise()
    {
        var processor = new RotateProcessor(90);

        var result = processor.Process(_source);

        // Original red pixel at (10,20) should move in a predictable way
        // For a 100x50 bitmap rotated 90° clockwise, the new coords are:
        // x' = height - 1 - y, y' = x
        int xPrime = _source.Height - 1 - 20;
        int yPrime = 10;

        var rotatedPixel = result.GetPixel(xPrime, yPrime);

        Assert.That(rotatedPixel.ToArgb(), Is.EqualTo(Color.Red.ToArgb()));
    }
}