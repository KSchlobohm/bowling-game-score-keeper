using System.Collections.Generic;

namespace BowlingGame
{
    public class BowlingGame
    {
        private const int MaxFrames = 10;
        private Dictionary<string, List<Frame>> _playerFrames = new Dictionary<string, List<Frame>>();

        public void AddPlayer(string playerName)
        {
            if (!_playerFrames.ContainsKey(playerName))
            {
                _playerFrames[playerName] = new List<Frame>();
                for (int i = 0; i < MaxFrames; i++)
                {
                    _playerFrames[playerName].Add(new Frame());
                }
            }
        }

        public void RecordRoll(string playerName, int frame, int pins)
        {
            var frames = _playerFrames[playerName];
            var idx = frame - 1;
            var current = frames[idx];

            // Frames 1–9: a strike ends the frame; otherwise max 2 rolls
            if (idx < MaxFrames - 1)
            {
                if (current.IsStrike()) return;                   // ignore extra rolls after strike
                if (current.RollCount() >= 2) return;             // frame already has 2 rolls
                current.Rolls.Add(pins);
                return;
            }

            // 10th frame rules
            if (current.RollCount() == 0)
            {
                current.Rolls.Add(pins);
                return;
            }
            if (current.RollCount() == 1)
            {
                current.Rolls.Add(pins);
                return;
            }
            // Allow a 3rd roll only if strike or spare in first two rolls
            if (current.RollCount() == 2 &&
                (current.Rolls[0] == 10 || current.Rolls[0] + current.Rolls[1] == 10))
            {
                current.Rolls.Add(pins);
            }
        }

        public int GetTotalScore(string playerName)
        {
            var frames = _playerFrames[playerName];
            int score = 0;
            for (int i = 0; i < MaxFrames; i++)
            {
                var frame = frames[i];
                if (i == MaxFrames - 1)
                {
                    // Sum all rolls in 10th frame (could be 2 or 3)
                    score += frame.RollSum();
                }
                else if (frame.IsStrike())
                {
                    score += 10 + StrikeBonus(frames, i);
                }
                else if (frame.IsSpare())
                {
                    score += 10 + SpareBonus(frames, i);
                }
                else
                {
                    score += frame.RollSum();
                }
            }
            return score;
        }

        private int StrikeBonus(List<Frame> frames, int i) => SumNextRolls(frames, i, 2);
        private int SpareBonus(List<Frame> frames, int i) => SumNextRolls(frames, i, 1);

        // Collect the next N rolls across subsequent frames.
        private int SumNextRolls(List<Frame> frames, int frameIndex, int needed)
        {
            int sum = 0;
            for (int f = frameIndex + 1; f < MaxFrames && needed > 0; f++)
            {
                var fr = frames[f];
                for (int r = 0; r < fr.Rolls.Count && needed > 0; r++)
                {
                    // Ignore any second-roll placeholders in strike frames 1–9.
                    if (f < MaxFrames - 1 && fr.IsStrike() && r > 0) continue;
                    sum += fr.Rolls[r];
                    needed--;
                }
            }
            return sum;
        }

        private class Frame
        {
            public List<int> Rolls { get; set; } = new List<int>();
            public int RollCount() => Rolls.Count;

            // Sum all recorded rolls (10th frame may have 3).
            public int RollSum()
            {
                int total = 0;
                for (int i = 0; i < Rolls.Count; i++) total += Rolls[i];
                return total;
            }

            public bool IsStrike() => Rolls.Count > 0 && Rolls[0] == 10;
            public bool IsSpare() => Rolls.Count > 1 && Rolls[0] != 10 && Rolls[0] + Rolls[1] == 10;
        }
    }
}