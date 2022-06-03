using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TicTacToe.UnitTests;

[TestClass]
public class SlotsTests
{
    [TestMethod("test Slots init")]
    public void TestInit()
    {
        var slots = new Slots<Mark>();
        slots.ForEach(s => Assert.AreEqual(Mark.NONE, s));
    }

    [TestMethod("test Map")]
    public void TestMap()
    {
        var slots = new Slots<Mark>();
        var result = slots.Map((m, row, col) => $"{row}:{col}");
        Assert.AreNotSame(slots, result);
    }
}