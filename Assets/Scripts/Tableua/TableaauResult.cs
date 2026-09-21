using UnityEngine;

namespace Dice
{
    public sealed class TableauResult
    {
        private RollResult _rollResult;
        private CombatantData _playerData;
        private CombatantData _opponentData;

        public RollResult RollResult { get { return _rollResult; } }
        public CombatantData PlayerData { get { return _playerData; } }
        public CombatantData OpponentData {  get { return _opponentData; } }

        public TableauResult(RollResult rollResult, CombatantData playerData, CombatantData opponentData)
        {
            _rollResult = rollResult;
            _playerData = playerData;
            _opponentData = opponentData;
        }



    }
}

