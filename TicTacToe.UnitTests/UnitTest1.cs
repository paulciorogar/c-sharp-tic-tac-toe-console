namespace TicTacToe.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var game = new Game();
            var result = game.ItWorks();

            Assert.IsTrue(result);
        }
    }
}