namespace MIETests;

using System.Drawing;
using MassImageEditor.Core.Processors;
using NUnit.Framework;

/// <summary>
/// Tests for contrast adjustment functionality.
/// </summary>
[TestFixture]
public class ContrastTest
{
    private Bitmap _testImage = null!;

    [SetUp]
    public void SetUp()
    {
        _testImage = new Bitmap(3, 1);

        // Create three pixels with different brightness levels so we can
        // observe how they move relative to the midpoint when contrast changes.
        _testImage.SetPixel(0, 0, Color.FromArgb(64, 64, 64));   // dark
        _testImage.SetPixel(1, 0, Color.FromArgb(128, 128, 128)); // mid
        _testImage.SetPixel(2, 0, Color.FromArgb(192, 192, 192)); // bright
    }

    [TearDown]
    public void TearDown()
    {
        _testImage.Dispose();
    }

    [Test]
    public void ShouldProcess_IsTrue_ForValueWithinRange()
    {
        var processor = new ContrastProcessor(50);
        Assert.That(processor.ShouldProcess, Is.True);
    }

    [Test]
    public void ShouldProcess_IsTrue_ForBoundaryValues()
    {
        var processorHigh = new ContrastProcessor(100);
        var processorLow = new ContrastProcessor(-100);

        Assert.That(processorHigh.ShouldProcess, Is.True);
        Assert.That(processorLow.ShouldProcess, Is.True);
    }

    [Test]
    public void ShouldProcess_IsFalse_ForValueTooHighOrTooLow()
    {
        var processorHigh = new ContrastProcessor(200);
        var processorLow = new ContrastProcessor(-200);

        Assert.That(processorHigh.ShouldProcess, Is.False);
        Assert.That(processorLow.ShouldProcess, Is.False);
    }

    [Test]
    public void Process_DoesNotChangePixels_WhenContrastIsZero()
    {
        var processor = new ContrastProcessor(0);

        var result = processor.Process(_testImage);

        // For contrast 0 the color matrix becomes identity, so all pixels
        // should remain exactly the same.
        for (var x = 0; x < _testImage.Width; x++)
        {
            var original = _testImage.GetPixel(x, 0);
            var processed = result.GetPixel(x, 0);
            Assert.That(processed, Is.EqualTo(original),
                $"Pixel at {x},0 changed unexpectedly for zero contrast.");
        }
    }

    [Test]
    public void Process_IncreasesDifferenceFromMidpoint_WhenContrastPositive()
    {
        var processor = new ContrastProcessor(50);

        var result = processor.Process(_testImage);

        var darkBefore = _testImage.GetPixel(0, 0);
        var midBefore = _testImage.GetPixel(1, 0);
        var brightBefore = _testImage.GetPixel(2, 0);

        var darkAfter = result.GetPixel(0, 0);
        var midAfter = result.GetPixel(1, 0);
        var brightAfter = result.GetPixel(2, 0);

        // Darks should get darker and brights should get brighter.
        Assert.That(darkAfter.R, Is.LessThan(darkBefore.R));
        Assert.That(brightAfter.R, Is.GreaterThan(brightBefore.R));

        // Midpoint should stay close to its original value (around 128).
        Assert.That(midAfter.R, Is.InRange(120, 136));

        // Sanity: maintain ordering dark <= mid <= bright after processing.
        Assert.That(darkAfter.R, Is.LessThanOrEqualTo(midAfter.R));
        Assert.That(midAfter.R, Is.LessThanOrEqualTo(brightAfter.R));
    }

    [Test]
    public void Process_ReducesDifferenceFromMidpoint_WhenContrastNegative()
    {
        var processor = new ContrastProcessor(-50);

        var result = processor.Process(_testImage);

        var darkBefore = _testImage.GetPixel(0, 0);
        var midBefore = _testImage.GetPixel(1, 0);
        var brightBefore = _testImage.GetPixel(2, 0);

        var darkAfter = result.GetPixel(0, 0);
        var midAfter = result.GetPixel(1, 0);
        var brightAfter = result.GetPixel(2, 0);

        // With negative contrast, values move towards the midpoint.
        Assert.That(darkAfter.R, Is.GreaterThan(darkBefore.R));
        Assert.That(brightAfter.R, Is.LessThan(brightBefore.R));

        // Ordering should still be preserved.
        Assert.That(darkAfter.R, Is.LessThanOrEqualTo(midAfter.R));
        Assert.That(midAfter.R, Is.LessThanOrEqualTo(brightAfter.R));

        // Midpoint should remain close to the original value.
        Assert.That(midAfter.R, Is.InRange(120, 136));
    }
}