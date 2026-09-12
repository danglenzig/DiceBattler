using UnityEngine;
using Dice;
using MiscTools;
using UnityEngine.UI;


public class TestRig : MonoBehaviour
{
    [SerializeField] private SO_Die testDieData;
    [SerializeField] private CanvasDie testCanvasDie;

    [SerializeField] private Button _testButton;

    void Start()
    {
        testCanvasDie.SetDiceData(testDieData);
        testCanvasDie.SetDieVisible(true);
    }

    private void OnEnable()
    {
        _testButton.onClick.AddListener(HandleTestButtonPressed);
    }

    private void OnDisable()
    {
        _testButton.onClick.RemoveAllListeners();
    }

    private void HandleTestButtonPressed()
    {
        _testButton.interactable = false;
        testCanvasDie.Roll();
    }
}
