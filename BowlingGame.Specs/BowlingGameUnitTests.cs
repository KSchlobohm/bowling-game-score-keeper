using Xunit;
using BowlingGame;

namespace BowlingGame.Specs
{
    public class BowlingGameUnitTests
    {
        [Fact]
        public void Test_Strike_Then_Two_Rolls()
        {
            var game = new BowlingGame();
            game.AddPlayer("Eve");
            game.RecordRoll("Eve", 1, 10); // strike
            game.RecordRoll("Eve", 2, 3);
            game.RecordRoll("Eve", 2, 4);
            Assert.Equal(24, game.GetTotalScore("Eve"));
        }

        [Fact]
        public void Test_Spare_Then_Single_Roll()
        {
            var game = new BowlingGame();
            game.AddPlayer("Frank");
            game.RecordRoll("Frank", 1, 7);
            game.RecordRoll("Frank", 1, 3); // spare
            game.RecordRoll("Frank", 2, 4);
            Assert.Equal(18, game.GetTotalScore("Frank"));
        }

        [Fact]
        public void Test_MultiFrameGame()
        {
            var game = new BowlingGame();
            game.AddPlayer("Grace");
            int[,] scores = new int[,] {
                {10,0}, {7,3}, {9,0}, {10,0}, {0,8}, {8,2}, {0,6}, {10,0}, {10,0}, {8,1}
            };
            for (int i = 0; i < 10; i++)
            {
                game.RecordRoll("Grace", i+1, scores[i,0]);
                game.RecordRoll("Grace", i+1, scores[i,1]);
            }
            Assert.Equal(146, game.GetTotalScore("Grace"));
        }
    }
}