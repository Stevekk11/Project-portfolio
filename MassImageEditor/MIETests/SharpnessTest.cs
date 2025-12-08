namespace MIETests;

using System.Drawing;
using MassImageEditor.Core.Processors;
using NUnit.Framework;

/// <summary>
/// Represents a test class for handling sharpness-related functionalities.
/// </summary>
[TestFixture]
public class SharpnessTest
{
    private Bitmap _testImage = null!;

    [SetUp]
    public void SetUp()
    {
        _testImage = new Bitmap(100, 100);
        using var g = Graphics.FromImage(_testImage);
        g.Clear(Color.Green);
    }

    [TearDown]
    public void TearDown()
    {
        _testImage.Dispose();
    }

    [Test]
    public void ShouldProcess_IsTrue_WhenSharpnessIsValid()
    {
        var processor = new SharpnessProcessor(50);
        Assert.That(processor.ShouldProcess, Is.True);
    }

    [Test]
    public void Process_SharpensImage_WhenProcessing()
    {
        var processor = new SharpnessProcessor(50);
        var result = processor.Process(_testImage);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Size, Is.EqualTo(_testImage.Size));
    }
}

