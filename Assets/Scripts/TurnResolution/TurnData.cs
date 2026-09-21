using UnityEngine;

namespace Dice
{
    public sealed class TurnData
    {
        public RollResult TheRollResult { get; private set; }
        public CombatantData PlayerData { get; private set; }
        public CombatantData OpponentData { get; private set; }

        public void SetTheRollResult(RollResult result)
        {
            TheRollResult = result;
        }
        public void SetCombatantData(CombatantData playerData, CombatantData opponentData)
        {
            PlayerData = playerData;
            OpponentData = opponentData;
        }
    }
}
