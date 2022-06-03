namespace TicTacToe.UnitTests
{
    [TestClass]
    public class GameTest
    {
        [TestMethod("it should mark a slot with X if X's trun")]
        public void TestMethod1()
        {
            var state = State.New();
            var game = new Game(state);

            game.MarkSlot(1, 1);

            var result = game.State.slots.Val(1, 1);
            Assert.AreEqual(Mark.X, result);
        }

        [TestMethod("it should mark a slot with O if O's trun")]
        public void TestMethod2()
        {
            var state = State.New();
            state = state.Update(new PartialState() { currentUserMark = Mark.O });
            var game = new Game(state);

            game.MarkSlot(1, 1);

            var result = game.State.slots.Val(1, 1);
            Assert.AreEqual(Mark.O, result);
        }

        [TestMethod("it should switch trun")]
        public void TestMethod3()
        {
            var state = State.New();
            var game = new Game(state);

            game.MarkSlot(1, 1);

            Assert.AreEqual(Mark.O, game.State.currentUserMark);
        }
    }
}