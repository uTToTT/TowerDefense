using NUnit.Framework;

public class MapBoundsTests
{
    [Test]
    public void IsInside_ReturnsFalse_WhenOutOfBounds()
    {
        var bounds = new MapBounds();
        bounds.SetSize(10, 10);
        Assert.IsFalse(bounds.IsInside(-1, 0));
        Assert.IsFalse(bounds.IsInside(10, 0));
        Assert.IsFalse(bounds.IsInside(0, 10));
    }

    [Test]
    public void IsInside_ReturnsTrue_WhenInside()
    {
        var bounds = new MapBounds();
        bounds.SetSize(10, 10);
        Assert.IsTrue(bounds.IsInside(0, 0));
        Assert.IsTrue(bounds.IsInside(9, 9));
        Assert.IsTrue(bounds.IsInside(5, 5));
    }
}
