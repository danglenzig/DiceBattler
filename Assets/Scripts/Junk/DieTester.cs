using UnityEngine;
using Dice;
using MiscTools;
using EventChannels;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Runtime.CompilerServices;

public class DieTester : MonoBehaviour
{
    [SerializeField] private SO_Die _dieDataA;
    [SerializeField] private SO_Die _dieDataB;
    [SerializeField] private SO_Die _dieDataC;

    [SerializeField] private CanvasDie _canvasDieA;
    [SerializeField] private CanvasDie _canvasDieB;
    [SerializeField] private CanvasDie _canvasDieC;

    [SerializeField] private Button _testButton;

    [SerializeField] private TMP_Text _resultText;

    private void Awake()
    {
        //_canvasDieA.SetDiceData(_dieDataA);
        //_canvasDieB.SetDiceData(_dieDataB);
        //_canvasDieC.SetDiceData(_dieDataC);
        _canvasDieA.SetRuntimeDieData(_dieDataA.GetRuntimeDie());
        _canvasDieB.SetRuntimeDieData(_dieDataB.GetRuntimeDie());
        _canvasDieC.SetRuntimeDieData(_dieDataC.GetRuntimeDie());

    }

    private void OnEnable()
    {
        _testButton.onClick.AddListener(Roll);
        _canvasDieA.OnResultDecided += HandleOnRollDecided;
        _canvasDieB.OnResultDecided += HandleOnRollDecided;
        _canvasDieC.OnResultDecided += HandleOnRollDecided;
    }
    private void OnDisable()
    {
        _testButton.onClick.RemoveAllListeners();
        _canvasDieA.OnResultDecided -= HandleOnRollDecided;
        _canvasDieB.OnResultDecided -= HandleOnRollDecided;
        _canvasDieC.OnResultDecided -= HandleOnRollDecided;
    }




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _resultText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Roll()
    {
        _resultText.text = "";
        _testButton.interactable = false;
        _canvasDieA.Roll();
        _canvasDieB.Roll();
        _canvasDieC.Roll();
    }

    private void HandleOnRollDecided()
    {
        if (_canvasDieA.IsRolling || _canvasDieB.IsRolling || _canvasDieC.IsRolling) return;

        // If they're all done rolling

        int attackTotal = 0;
        int evasionTotal = 0;

        //SO_DieFace aFaceData = _canvasDieA.GetCurrentFaceData();
        //SO_DieFace bFaceData = _canvasDieB.GetCurrentFaceData();
        //SO_DieFace cFaceData = _canvasDieC.GetCurrentFaceData();

        RuntimeDieFace aFaceData = _canvasDieA.GetCurrentRuntimeDieFace();
        RuntimeDieFace bFaceData = _canvasDieB.GetCurrentRuntimeDieFace();
        RuntimeDieFace cFaceData = _canvasDieC.GetCurrentRuntimeDieFace();

        //List<SO_DieFace> datas = new List<SO_DieFace>() { aFaceData, bFaceData, cFaceData };
        List<RuntimeDieFace> datas = new List<RuntimeDieFace>() { aFaceData, bFaceData, cFaceData};

        foreach (RuntimeDieFace data in datas)
        {
            if (TagStringTools.IsAMatch("ACTION.ATTACK", data.TagStrings[0]))
            {
                attackTotal += data.IntegerValue;
            }
            if (TagStringTools.IsAMatch("ACTION.EVASION", data.TagStrings[0]))
            {
                evasionTotal += data.IntegerValue;
            }
        }

        string resultString = $"Attack total result: {attackTotal}, Evasion total result: {evasionTotal}";

        _testButton.interactable = true;

        _resultText.text = resultString;

        Debug.Log($"### {name}: {resultString}");
    }

}
