using MiscTools;
using EventChannels;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine;

namespace Dice
{
    public class TurnTableau : MonoBehaviour
    {
        private const string ATTACK_TAG = "ACTION.ATTACK";
        private const string EVASION_TAG = "ACTION.EVASION";

        [SerializeField] private List<CanvasDie> _playerCanvasDice;
        [SerializeField] private List<CanvasDie> _opponentCanvasDice;
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
            foreach (CanvasDie d in _playerCanvasDice)
            {
                d.OnResultDecided += HandleOnDieStoppedRolling;
            }
            foreach (CanvasDie d in _opponentCanvasDice)
            {
                d.OnResultDecided += HandleOnDieStoppedRolling;
            }
        }

        private void OnDisable()
        {
            foreach (CanvasDie d in _playerCanvasDice)
            {
                d.OnResultDecided -= HandleOnDieStoppedRolling;
            }
            foreach (CanvasDie d in _opponentCanvasDice)
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
            foreach (CanvasDie d in _playerCanvasDice)
            {
                if (d.IsRolling) return;
            }
            foreach (CanvasDie d in _opponentCanvasDice)
            {
                if (d.IsRolling) return;
            }

            List<DieFaceResult> playerDiceResults = new List<DieFaceResult>();
            List<DieFaceResult> opponentDiceResults = new List<DieFaceResult>();

            int playerAttackTotal = 0;
            int playerEvasionTotal = 0;
            int opponentAttackTotal = 0;
            int opponentEvasionTotal = 0;

            foreach (CanvasDie d in _playerCanvasDice)
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

            foreach (CanvasDie d in _opponentCanvasDice)
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
            foreach (CanvasDie die in _playerCanvasDice)
            {
                if (die.IsRolling)
                {
                    return true;
                }
            }
            foreach (CanvasDie die in _opponentCanvasDice)
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
            if (_defaultPlayerDiceSOs.Count != _playerCanvasDice.Count) return;
            if (_defaultOpponentDiceSOs.Count != _opponentCanvasDice.Count) return;

            for (int i = 0; i < _defaultPlayerDiceSOs.Count; i++)
            {
                _playerCanvasDice[i].SetRuntimeDieData(_defaultPlayerDiceSOs[i].GetRuntimeDie());
            }
            for (int i = 0; i < _defaultOpponentDiceSOs.Count; i++)
            {
                _opponentCanvasDice[i].SetRuntimeDieData(_defaultOpponentDiceSOs[i].GetRuntimeDie());
            }
        }
        

        //=====
        // API
        //=====

        public void SetPlayerDice(List<RuntimeDie> diceDatas)
        {
            // assert that diceDatas.Count == _playerCanvasDice.Count
            if(diceDatas.Count != _playerCanvasDice.Count)
            {
                Debug.LogError($"###{name}: Incoming player dice data != the number of canvas dice");
            }

            for (int i = 0; i < diceDatas.Count; i++)
            {
                _playerCanvasDice[i].SetRuntimeDieData(diceDatas[i]);
            }
        }
        public void SetOpponentDice(List<RuntimeDie> diceDatas)
        {
            // assert that diceDatas.Count == _opponentCanvasDice.Count
            if (diceDatas.Count != _opponentCanvasDice.Count)
            {
                Debug.LogError($"###{name}: Incoming opponent dice data != the number of canvas dice");
            }

            for (int i = 0; i < diceDatas.Count; i++)
            {
                _opponentCanvasDice[i].SetRuntimeDieData(diceDatas[i]);
            }
        }

        public bool ReadyToRoll()
        {
            foreach (CanvasDie d in _playerCanvasDice)
            {
                if (d.CurrentDieData == null) return false;
            }
            foreach(CanvasDie d in _opponentCanvasDice)
            {
                if (d.CurrentDieData == null) return false;
            }

            return true;
        }

        public void SetCombatants(CombatantData playerData, CombatantData opponentData)
        {
            _playerData = playerData;
            _opponentData = opponentData;
        }

        public void Roll()
        {
            if (GetIsRolling()) return;

            foreach (CanvasDie d in _playerCanvasDice)
            {
                if (d.CurrentDieData != null)
                {
                    d.Roll();
                }                
            }
            foreach (CanvasDie d in _opponentCanvasDice)
            {
                if (d.CurrentDieData != null)
                {
                    d.Roll();
                }
            }
        }
    }
}


