using MiscTools;
using EventChannels;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Dice
{

    public class Combatant
    {
        private CombatantData _data = new CombatantData();

        public CombatantData Data { get { return _data; } }

        

        public void SetData(CombatantData inData)
        {
            _data = inData;
        }
    }

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


        private Combatant _player;
        private Combatant _opponent;

        //private IResolverRules _rules;

        private CombatResolver _resolver;

        public event System.Action<TurnResolution> OnRollingFinished;

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

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (_useDefaultDiceSOs)
            {
                SetupDefaultDice();
            }
            _playerResultText.text   = string.Empty;
            _opponentResultText.text = string.Empty;

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void HandleOnDieStoppedRolling()
        {
            if (_player == null || _opponent == null) return;

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

            PresentRollResult(_playerResultText, playerAttackTotal, playerEvasionTotal);
            PresentRollResult(_opponentResultText, opponentAttackTotal, opponentEvasionTotal);

            RollResult turnResult = new RollResult();
            turnResult.SetPlayerRoll(playerDiceResults);
            turnResult.SetOpponentRoll(opponentDiceResults);

            TurnResolution res = _resolver.Resolve(turnResult, _player.Data, _opponent.Data);
            OnRollingFinished?.Invoke(res);

            //Debug.Log($"### {name}: Dice stopped rolling");
        }

        private void PresentRollResult(TMP_Text resultText, int attackTotal, int evasionTotal)
        {
            if (resultText == null) return;
            string resultStr = $"Attack Power: {attackTotal.ToString()}\nTotal Evasion: {evasionTotal}";
            resultText.text = resultStr;
        }

        private void DebugDieFaceResult(string combatantName, DieFaceResult res)
        {
            string tagsStr = string.Join(",", res.TagStrings);
            string debStr = $"{combatantName}:\n  {res.TagStrings[0]}\n  {res.IntegerValue.ToString()}";
            Debug.Log($"### {name}: {debStr}");
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

        
        private TurnData GetTurnData()
        {
            TurnData turnData = new TurnData();
            RollResult rollResult = new RollResult();

            CombatantData playerData = _player.Data;
            CombatantData opponentData = _opponent.Data;

            List<DieFaceResult> playerDiceResults = new List<DieFaceResult>();
            List<DieFaceResult> opponentDiceResults = new List<DieFaceResult>();

            // iterate through the player dice and create a DieFaceResult for each one
            // add each one to playerDiceResults

            // iterate through the opponent dice and create a DieFaceResult for each one
            // add each one to opponentDiceResults

            rollResult.SetPlayerRoll(playerDiceResults);
            rollResult.SetOpponentRoll(opponentDiceResults);

            turnData.SetTheRollResult(rollResult);
            turnData.SetCombatantData(playerData, opponentData);

            return turnData;
        }
        

        //=====
        // API
        //=====

        public void SetCombatants(Combatant player, Combatant opponent)
        {
            _player = player;
            _opponent = opponent;
        }

        public void SetResolver(IResolverRules rules)
        {
            _resolver = new CombatResolver(rules);
        }

        public void Roll()
        {
            if (GetIsRolling()) return;

            //Debug.Log($"### {name}: Tryna' roll...");

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


