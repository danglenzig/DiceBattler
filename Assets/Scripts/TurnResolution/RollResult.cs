using System.Collections.Generic;
using UnityEngine;

namespace Dice
{
    public sealed class RollResult
    {
        public IReadOnlyList<DieFaceResult> PlayerRoll { get; private set; } = new List<DieFaceResult>();
        public IReadOnlyList<DieFaceResult> OpponentRoll { get; private set; } = new List<DieFaceResult>();

        public void SetPlayerRoll(IReadOnlyList<DieFaceResult> playerRoll)
        {
            if (playerRoll != null)
            {
                PlayerRoll = playerRoll;
            }
        }
        public void SetOpponentRoll(IReadOnlyList<DieFaceResult> opponentRoll)
        {
            if (opponentRoll != null)
            {
                OpponentRoll = opponentRoll;
            }
        }
    }
}
