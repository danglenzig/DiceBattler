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

            Debug.Log($"### {name}: All dice have stopped rolling");

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

        public void SetCombatants(Combatant player, Combatant opponent)
        {
            _player = player;
            _opponent = opponent;
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


