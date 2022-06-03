namespace TicTacToe.UnitTests;

[TestClass]
public class SlotsTests
{
    [TestMethod("test Slots init")]
    public void TestInit()
    {
        var slots = new Slots<Mark>();
        slots.ForEach((s, _, _) => Assert.AreEqual(Mark.NONE, s));
    }

    [TestMethod("test Map: returns a new instance of Slots")]
    public void TestMap()
    {
        var slots = new Slots<Mark>();
        var result = slots.Map((m, row, col) => $"{row}:{col}");
        Assert.AreNotSame(slots, result);
        result.ForEach((val, row, col) => Assert.AreEqual($"{row}:{col}", val));
    }
}