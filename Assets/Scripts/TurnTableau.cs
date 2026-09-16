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


            Debug.Log($"### {name}: Dice stopped rolling");

            //==========
            // BOOKMARK
            //==========

            // get a TurnResolution from _resolver.Resolve()
            //TurnResolution res = ...

            // pop off an event with information (?)
            //OnRollingFinished?.Invoke(turnRes);
            


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

            Debug.Log($"### {name}: Tryna' roll...");

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


