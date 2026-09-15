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
        [SerializeField] private List<CanvasDie> _playerDice;
        [SerializeField] private List<CanvasDie> _opponentDice;
        [SerializeField] private TMP_Text _playerResultText;
        [SerializeField] private TMP_Text _opponentResultText;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _playerResultText.text   = string.Empty;
            _opponentResultText.text = string.Empty;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}


