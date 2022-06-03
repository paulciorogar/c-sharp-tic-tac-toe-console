namespace TicTacToe.UnitTests
{
    [TestClass]
    public class GameTest
    {
        [TestMethod("it should mark a slot with X if X's turn")]
        public void TestMethod1()
        {
            var state = State.New();
            var game = new Game(state);

            game.MarkSlot(1, 1);

            var result = game.State.slots.Val(1, 1).OrSome(Mark.NONE);
            Assert.AreEqual(Mark.X, result);
        }

        [TestMethod("it should mark a slot with O if O's turn")]
        public void TestMethod2()
        {
            var state = State.New();
            state = state.Update(new PartialState() { currentUserMark = Mark.O });
            var game = new Game(state);

            game.MarkSlot(1, 1);

            var result = game.State.slots.Val(1, 1).OrSome(Mark.NONE);
            Assert.AreEqual(Mark.O, result);
        }

        [TestMethod("it should switch turn")]
        public void TestMethod3()
        {
            var state = State.New();
            var game = new Game(state);

            game.MarkSlot(1, 1);

            Assert.AreEqual(Mark.O, game.State.currentUserMark);
        }

        [TestMethod("it should not mark a slot that is marked already")]
        public void TestMethod4()
        {
            var state = State.New();
            var game = new Game(state);

            Assert.AreEqual(String.Empty, game.State.message);

            game.MarkSlot(1, 1);
            game.MarkSlot(1, 1);

            var result = game.State.slots.Val(1, 1).OrSome(Mark.NONE);
            Assert.AreEqual(Mark.X, result);
            Assert.AreEqual("Slot 1.1 already marked by: X", game.State.message);
        }

        [TestMethod("it should clear message once a valid mark is done")]
        public void TestMethod5()
        {
            var state = State.New();
            var game = new Game(state);

            Assert.AreEqual(String.Empty, game.State.message);

            game.MarkSlot(1, 1);
            game.MarkSlot(1, 1);

            Assert.AreNotEqual(string.Empty, game.State.message);

            game.MarkSlot(1, 2);

            Assert.AreEqual(string.Empty, game.State.message);
        }

        [TestMethod("it provides a message if slot does not exist")]
        public void TestMethod6()
        {
            var state = State.New();
            var game = new Game(state);

            Assert.AreEqual(String.Empty, game.State.message);

            game.MarkSlot(100, 100);

            Assert.AreEqual("Slot 100.100 is not in this game", game.State.message);
        }
    }
}