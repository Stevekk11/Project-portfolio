using NUnit.Framework.Legacy;

namespace MIETests;

using System.Drawing;
using MassImageEditor.Core.Processors;
using NUnit.Framework;

public class FormatTest
{
    private Bitmap _testBitmap = null!;

    [SetUp]
    public void SetUp()
    {
        _testBitmap = new Bitmap(10, 10);
        using var g = Graphics.FromImage(_testBitmap);
        g.Clear(Color.Red);
    }

    [TearDown]
    public void TearDown()
    {
        _testBitmap.Dispose();
    }

    [Test]
    public void ShouldConvert_IsFalse_WhenFormatIsEmpty()
    {
        var saver = new FormatSaver(string.Empty);

        Assert.That(saver.ShouldConvert, Is.False);
    }

    [Test]
    public void ShouldConvert_IsTrue_WhenFormatIsProvided()
    {
        var saver = new FormatSaver("png");

        Assert.That(saver.ShouldConvert, Is.True);
    }

    [Test]
    public void Save_WithoutConversion_SavesFile()
    {
        var saver = new FormatSaver(string.Empty);
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".png");

        try
        {
            saver.Save(_testBitmap, path);

            Assert.That(File.Exists(path), Is.True);
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    [TestCase("jpg")]
    [TestCase("jpeg")]
    [TestCase("png")]
    [TestCase("bmp")]
    [TestCase("webp")]
    public void Save_WithConversion_SavesFile(string format)
    {
        var saver = new FormatSaver(format);
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + "." + format.ToLowerInvariant());

        try
        {
            saver.Save(_testBitmap, path);

            Assert.That(File.Exists(path), Is.True);
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    [Test]
    public void GetOutputPath_ChangesExtension_WhenFormatIsSet()
    {
        var saver = new FormatSaver("png");
        var original = @"C:\images\photo.jpg";

        var result = saver.GetOutputPath(original);

        StringAssert.EndsWith(".png", result);
        StringAssert.StartsWith(@"C:\images\photo", result);
    }

    [Test]
    public void GetOutputPath_ReturnsOriginal_WhenFormatIsEmpty()
    {
        var saver = new FormatSaver(string.Empty);
        var original = @"C:\images\photo.jpg";

        var result = saver.GetOutputPath(original);

        Assert.That(result, Is.EqualTo(original));
    }
}