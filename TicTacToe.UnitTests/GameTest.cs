namespace TicTacToe.UnitTests
{
    [TestClass]
    public class GameTest
    {
        [TestMethod("should mark a slot with X if X's turn")]
        public void TestMethod1()
        {
            var state = State.New();
            var game = new Game(state);

            game.MarkSlot(1, 1);

            var result = game.State.Slots.Val(1, 1).OrSome(Mark.NONE);
            Assert.AreEqual(Mark.X, result);
        }

        [TestMethod("should mark a slot with O if O's turn")]
        public void TestMethod2()
        {
            var state = State.New();
            state = state.Update(data =>
            {
                data.CurrentUserMark = Mark.O;
                return data;
            });
            var game = new Game(state);

            game.MarkSlot(1, 1);

            var result = game.State.Slots.Val(1, 1).OrSome(Mark.NONE);
            Assert.AreEqual(Mark.O, result);
        }

        [TestMethod("should switch turn")]
        public void TestMethod3()
        {
            var state = State.New();
            var game = new Game(state);

            game.MarkSlot(1, 1);

            Assert.AreEqual(Mark.O, game.State.CurrentUserMark);
        }

        [TestMethod("should not mark a slot that is marked already")]
        public void TestMethod4()
        {
            var state = State.New();
            var game = new Game(state);

            Assert.IsInstanceOfType(game.State.Conclusion, typeof(NotConcluded));

            game.MarkSlot(1, 1);
            game.MarkSlot(1, 1);

            var result = game.State.Slots.Val(1, 1).OrSome(Mark.NONE);
            Assert.IsInstanceOfType(game.State.Conclusion, typeof(InvalidInput));
            Assert.AreEqual(Mark.X, result);
            Assert.AreEqual("Slot 1.1 already marked by: X", game.State.Conclusion.Message);
        }

        [TestMethod("should clear the error once a valid mark is done")]
        public void TestMethod5()
        {
            var state = State.New();
            var game = new Game(state);

            game.MarkSlot(1, 1);
            game.MarkSlot(1, 1);

            Assert.IsNotInstanceOfType(game.State.Conclusion, typeof(NotConcluded));

            game.MarkSlot(1, 2);

            Assert.IsInstanceOfType(game.State.Conclusion, typeof(NotConcluded));
        }

        [TestMethod("should handle the case when slot does not exist")]
        public void TestMethod6()
        {
            var state = State.New();
            var game = new Game(state);

            Assert.IsNotInstanceOfType(game.State.Conclusion, typeof(InvalidInput));

            game.MarkSlot(100, 100);

            Assert.IsInstanceOfType(game.State.Conclusion, typeof(InvalidInput));
            Assert.AreEqual("Slot 100.100 is not in this game", game.State.Conclusion.Message);
        }

        [TestMethod("should handle victory for row")]
        public void TestMethod7()
        {
            var state = State.New();
            state = state.Update(data =>
            {
                data.Slots = state.Slots.Map((val, row, col) =>
                {
                    if (row == 0 && col > 0) return Mark.X;
                    return val;
                });
                return data;
            });

            var game = new Game(state);

            Assert.IsInstanceOfType(game.State.Conclusion, typeof(NotConcluded));

            game.MarkSlot(0, 0);

            Assert.IsInstanceOfType(game.State.Conclusion, typeof(Victory));
            Assert.AreEqual("X won the game", game.State.Conclusion.Message);
        }

        [TestMethod("should handle victory for column")]
        public void TestMethod8()
        {
            var state = State.New();
            state = state.Update(data =>
            {
                data.Slots = state.Slots.Map((val, row, col) =>
                {
                    if (row > 0 && col == 0) return Mark.X;
                    return val;
                });
                return data;
            });

            var game = new Game(state);

            Assert.IsInstanceOfType(game.State.Conclusion, typeof(NotConcluded));

            game.MarkSlot(0, 0);

            Assert.IsInstanceOfType(game.State.Conclusion, typeof(Victory));
            Assert.AreEqual("X won the game", game.State.Conclusion.Message);
        }

        [TestMethod("should handle victory for diagonal: top left to bottom right")]
        public void TestMethod9()
        {
            var state = State.New();
            state = state.Update(data =>
            {
                data.Slots = state.Slots.Map((val, row, col) =>
                {
                    if (row == 1 && col == 1) return Mark.X;
                    if (row == 2 && col == 2) return Mark.X;
                    return val;
                });
                return data;
            });

            var game = new Game(state);

            Assert.IsInstanceOfType(game.State.Conclusion, typeof(NotConcluded));

            game.MarkSlot(0, 0);

            Assert.IsInstanceOfType(game.State.Conclusion, typeof(Victory));
            Assert.AreEqual("X won the game", game.State.Conclusion.Message);
        }

        [TestMethod("should handle victory for diagonal: bottom right to top left")]
        public void TestMethod10()
        {
            var state = State.New();
            state = state.Update(data =>
            {
                data.Slots = state.Slots.Map((val, row, col) =>
                {
                    if (row == 0 && col == 0) return Mark.X;
                    if (row == 1 && col == 1) return Mark.X;
                    return val;
                });
                return data;
            });

            var game = new Game(state);

            Assert.IsInstanceOfType(game.State.Conclusion, typeof(NotConcluded));

            game.MarkSlot(2, 2);

            Assert.IsInstanceOfType(game.State.Conclusion, typeof(Victory));
            Assert.AreEqual("X won the game", game.State.Conclusion.Message);
        }

        [TestMethod("should handle victory for diagonal: bottom left to top right")]
        public void TestMethod11()
        {
            var state = State.New();
            state = state.Update(data =>
            {
                data.Slots = state.Slots.Map((val, row, col) =>
                {
                    if (row == 0 && col == 2) return Mark.X;
                    if (row == 1 && col == 1) return Mark.X;
                    return val;
                });
                return data;
            });

            var game = new Game(state);

            Assert.IsInstanceOfType(game.State.Conclusion, typeof(NotConcluded));

            game.MarkSlot(2, 0);

            Assert.IsInstanceOfType(game.State.Conclusion, typeof(Victory));
            Assert.AreEqual("X won the game", game.State.Conclusion.Message);
        }

        [TestMethod("should handle victory for diagonal: top right to bottom left")]
        public void TestMethod12()
        {
            var state = State.New();
            state = state.Update(data =>
            {
                data.Slots = state.Slots.Map((val, row, col) =>
                {
                    if (row == 1 && col == 1) return Mark.X;
                    if (row == 2 && col == 0) return Mark.X;
                    return val;
                });
                return data;
            });

            var game = new Game(state);

            Assert.IsInstanceOfType(game.State.Conclusion, typeof(NotConcluded));

            game.MarkSlot(0, 2);

            Assert.IsInstanceOfType(game.State.Conclusion, typeof(Victory));
            Assert.AreEqual("X won the game", game.State.Conclusion.Message);
        }
    }
}