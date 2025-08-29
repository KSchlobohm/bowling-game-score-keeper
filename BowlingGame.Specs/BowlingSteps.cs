using System.Collections.Generic;
using TechTalk.SpecFlow;
using Xunit;

namespace BowlingGame.Specs
{
    [Binding]
    public class BowlingSteps
    {
        private BowlingGame _game;

        [Given(@"a new bowling game")]
        public void GivenANewBowlingGame()
        {
            _game = new BowlingGame();
        }

        [Given(@"a player named ""(.*)""")]
        public void GivenAPlayerNamed(string playerName)
        {
            _game.AddPlayer(playerName);
        }

        [When(@"(.*) records a score of (\d+) for frame (\d+)")]
        public void WhenPlayerRecordsScore(string playerName, int score, int frame)
        {
            _game.RecordRoll(playerName, frame, score);
        }

        [When(@"(.*) records the following scores:")]
        public void WhenPlayerRecordsMultipleScores(string playerName, Table table)
        {
            foreach (var row in table.Rows)
            {
                int frame = int.Parse(row["Frame"]);
                int roll1 = int.Parse(row["Roll1"]);
                int roll2 = int.Parse(row["Roll2"]);
                _game.RecordRoll(playerName, frame, roll1);
                if (roll2 > 0)
                {
                    _game.RecordRoll(playerName, frame, roll2);
                }
            }
        }

        [Then(@"(.*)'s total score should be (\d+)")]
        public void ThenPlayersTotalScoreShouldBe(string playerName, int expectedScore)
        {
            Assert.Equal(expectedScore, _game.GetTotalScore(playerName));
        }
    }
}