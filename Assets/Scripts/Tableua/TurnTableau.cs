using MiscTools;
using EventChannels;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Dice
{

    public class TurnTableau : MonoBehaviour
    {

        private const string ATTACK_TAG = "ACTION.ATTACK";
        private const string EVASION_TAG = "ACTION.EVASION";

        [SerializeField] private List<CanvasDie> _playerDice;
        [SerializeField] private List<CanvasDie> _opponentDice;
        [SerializeField] private TMP_Text _playerResultText;
        [SerializeField] private TMP_Text _opponentResultText;

        [SerializeField] private List<SO_Die> _defaultPlayerDiceSOs;
        [SerializeField] private List<SO_Die> _defaultOpponentDiceSOs;

        [SerializeField] private bool _useDefaultDiceSOs = true;
        private CombatantData _playerData;
        private CombatantData _opponentData;
        public event System.Action<TableauResult> OnTableauResultAnnounced;

        public bool IsRolling
        {
            get
            {
                return GetIsRolling();
            }
        }

        private void OnEnable()
        {
            foreach (CanvasDie d in _playerDice)
            {
                d.OnResultDecided += HandleOnDieStoppedRolling;
            }
            foreach (CanvasDie d in _opponentDice)
            {
                d.OnResultDecided += HandleOnDieStoppedRolling;
            }
        }

        private void OnDisable()
        {
            foreach (CanvasDie d in _playerDice)
            {
                d.OnResultDecided -= HandleOnDieStoppedRolling;
            }
            foreach (CanvasDie d in _opponentDice)
            {
                d.OnResultDecided -= HandleOnDieStoppedRolling;
            }
        }
        void Start()
        {
            if (_useDefaultDiceSOs)
            {
                SetupDefaultDice();
            }
            _playerResultText.text   = string.Empty;
            _opponentResultText.text = string.Empty;

        }

        private void HandleOnDieStoppedRolling()
        {
            if (_playerData == null || _opponentData == null) return;

            // return early if any dice are still rolling
            foreach (CanvasDie d in _playerDice)
            {
                if (d.IsRolling) return;
            }
            foreach (CanvasDie d in _opponentDice)
            {
                if (d.IsRolling) return;
            }

            List<DieFaceResult> playerDiceResults = new List<DieFaceResult>();
            List<DieFaceResult> opponentDiceResults = new List<DieFaceResult>();

            int playerAttackTotal = 0;
            int playerEvasionTotal = 0;
            int opponentAttackTotal = 0;
            int opponentEvasionTotal = 0;

            foreach (CanvasDie d in _playerDice)
            {
                RuntimeDieFace thisFace = d.GetCurrentRuntimeDieFace();
                int intValue = thisFace.IntegerValue;
                List<string> tags = thisFace.TagStrings;
                DieFaceResult thisResult = new DieFaceResult();
                thisResult.SetIntegerValue(intValue);
                thisResult.SetTagStrings(tags);
                playerDiceResults.Add(thisResult);

                //DebugDieFaceResult("Player", thisResult);

                if (TagStringTools.IsAMatch(tags[0], ATTACK_TAG))
                {
                    playerAttackTotal += intValue;
                }
                if (TagStringTools.IsAMatch(tags[0], EVASION_TAG))
                {
                    playerEvasionTotal += intValue;
                }

            }

            foreach (CanvasDie d in _opponentDice)
            {
                RuntimeDieFace thisFace = d.GetCurrentRuntimeDieFace();
                int intValue = thisFace.IntegerValue;
                List<string> tags = thisFace.TagStrings;
                DieFaceResult thisResult = new DieFaceResult();
                thisResult.SetIntegerValue(intValue);
                thisResult.SetTagStrings(tags);

                //DebugDieFaceResult("Opponent", thisResult);

                if (TagStringTools.IsAMatch(tags[0], ATTACK_TAG))
                {
                    opponentAttackTotal += intValue;
                }
                if (TagStringTools.IsAMatch(tags[0], EVASION_TAG))
                {
                    opponentEvasionTotal += intValue;
                }

                opponentDiceResults.Add(thisResult);
            }

            RollResult rollResult = new RollResult();
            rollResult.SetPlayerRoll(playerDiceResults);
            rollResult.SetOpponentRoll(opponentDiceResults);

            TableauResult tableauResult = new TableauResult(rollResult, _playerData, _opponentData);
            OnTableauResultAnnounced?.Invoke(tableauResult);
        }

        private bool GetIsRolling()
        {
            foreach (CanvasDie die in _playerDice)
            {
                if (die.IsRolling)
                {
                    return true;
                }
            }
            foreach (CanvasDie die in _opponentDice)
            {
                if (die.IsRolling)
                {
                    return true;
                }
            }
            return false;
        }

        private void SetupDefaultDice()
        {
            if (_defaultPlayerDiceSOs.Count != _playerDice.Count) return;
            if (_defaultOpponentDiceSOs.Count != _opponentDice.Count) return;

            for (int i = 0; i < _defaultPlayerDiceSOs.Count; i++)
            {
                _playerDice[i].SetRuntimeDieData(_defaultPlayerDiceSOs[i].GetRuntimeDie());
            }
            for (int i = 0; i < _defaultOpponentDiceSOs.Count; i++)
            {
                _opponentDice[i].SetRuntimeDieData(_defaultOpponentDiceSOs[i].GetRuntimeDie());
            }
        }
        

        //=====
        // API
        //=====

        public void SetCombatants(CombatantData playerData, CombatantData opponentData)
        {
            _playerData = playerData;
            _opponentData = opponentData;
        }

        public void Roll()
        {
            if (GetIsRolling()) return;

            foreach (CanvasDie d in _playerDice)
            {
                if (d.CurrentDieData != null)
                {
                    d.Roll();
                }                
            }
            foreach (CanvasDie d in _opponentDice)
            {
                if (d.CurrentDieData != null)
                {
                    d.Roll();
                }
            }
        }
    }
}


