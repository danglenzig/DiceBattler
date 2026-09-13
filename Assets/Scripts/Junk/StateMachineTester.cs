using UnityEngine.UI;
using Dice;
using SimpleStateMachine;
using EventChannels;
using UnityEngine;

public class StateMachineTester : MonoBehaviour
{
    [SerializeField] private GameObject _encounterManagerObject; 

    [SerializeField] private SO_SimpleState _encounterSetupState;
    [SerializeField] private SO_SimpleState _turnDrawupState;
    [SerializeField] private SO_SimpleState _turnSelectState;
    [SerializeField] private SO_SimpleState _turnRollState;
    [SerializeField] private SO_SimpleState _turnResolutionState;
    [SerializeField] private SO_SimpleState _turnAftermathState;
    [SerializeField] private SO_SimpleState _encounterWinState;
    [SerializeField] private SO_SimpleState _encounterDieState;

    [SerializeField] private Button _setupButton;
    [SerializeField] private Button _drawupButton;
    [SerializeField] private Button _selectButton;
    [SerializeField] private Button _rollButton;
    [SerializeField] private Button _resolutionButton;
    [SerializeField] private Button _aftermathButton;
    [SerializeField] private Button _winButton;
    [SerializeField] private Button _dieButton;

    [SerializeField] private SO_EventStringPayload _stateEnteredEvent;
    [SerializeField] private SO_EventStringPayload _stateExitedEvent;

    private RuntimeSSM sm;

    private void Awake()
    {
        sm = _encounterManagerObject.GetComponent<RuntimeSSM>();
    }

    private void OnEnable()
    {
        if (sm == null) return;
        _setupButton.onClick.AddListener(OnSetupPressed);
        _drawupButton.onClick.AddListener(OnDrawupPressed);
        _selectButton.onClick.AddListener(OnSelectPressed);
        _rollButton.onClick.AddListener(OnRollPressed);
        _resolutionButton.onClick.AddListener(OnResolutionPressed);
        _aftermathButton.onClick.AddListener(OnAftermathPressed);
        _winButton.onClick.AddListener(OnWinPressed);
        _dieButton.onClick.AddListener(OnDiePressed);

        _stateEnteredEvent.OnEventTriggered += HandleOnStateEntered;
        _stateExitedEvent.OnEventTriggered += HandleOnStateExited;

        
    }
    private void OnDisable()
    {
        if (sm == null) return;
        _setupButton.onClick.RemoveAllListeners();
        _drawupButton.onClick.RemoveAllListeners();
        _selectButton.onClick.RemoveAllListeners();
        _rollButton.onClick.RemoveAllListeners();
        _resolutionButton.onClick.RemoveAllListeners();
        _aftermathButton.onClick.RemoveAllListeners();
        _winButton.onClick.RemoveAllListeners();
        _dieButton.onClick.RemoveAllListeners();

        _stateEnteredEvent.OnEventTriggered -= HandleOnStateEntered;
        _stateExitedEvent.OnEventTriggered -= HandleOnStateExited;
    }

    private void OnSetupPressed()
    {
        if (sm.TryTakeTransition(_encounterSetupState.StateString))
        {
            return;
        }
        else
        {
            DebugFailedTransition(_encounterSetupState.StateString);
        }
    }
    private void OnDrawupPressed()
    {
        if (sm.TryTakeTransition(_turnDrawupState.StateString))
        {
            return;
        }
        else
        {
            DebugFailedTransition(_turnDrawupState.StateString);
        }
    }
    private void OnSelectPressed()
    {
        if (sm.TryTakeTransition(_turnSelectState.StateString))
        {
            return;
        }
        else
        {
            DebugFailedTransition(_turnSelectState.StateString);
        }
    }
    private void OnRollPressed()
    {
        if (sm.TryTakeTransition(_turnRollState.StateString))
        {
            return;
        }
        else
        {
            DebugFailedTransition(_turnRollState.StateString);
        }
    }
    private void OnResolutionPressed()
    {
        if (sm.TryTakeTransition(_turnResolutionState.StateString))
        {
            return;
        }
        else
        {
            DebugFailedTransition(_turnResolutionState.StateString);
        }
    }
    private void OnAftermathPressed()
    {
        if (sm.TryTakeTransition(_turnAftermathState.StateString))
        {
            return;
        }
        else
        {
            DebugFailedTransition(_turnAftermathState.StateString);
        }
    }
    private void OnWinPressed()
    {
        if (sm.TryTakeTransition(_encounterWinState.StateString))
        {
            return;
        }
        else
        {
            DebugFailedTransition(_encounterWinState.StateString);
        }
    }
    private void OnDiePressed()
    {
        if (sm.TryTakeTransition(_encounterDieState.StateString))
        {
            return;
        }
        else
        {
            DebugFailedTransition(_encounterDieState.StateString);
        }
    }

    private void HandleOnStateEntered(string enteredStateString)
    {
        string current = sm.CurrentState.StateString;
        Debug.Log($"### {name}: state entered: {current}");
    }
    private void HandleOnStateExited(string exitedStateString)
    {
        //string mostRecentHistorical = sm.StateHistoryStrings[sm.StateHistoryStrings.Count - 1];
        // ^^Not working. Why?

        Debug.Log($"### {name}: state exited: {exitedStateString}");
    }

    private void DebugFailedTransition(string failedTransName)
    {
        if (sm == null) return;
        string current = sm.CurrentState.StateString;
        Debug.LogWarning($"### {name}: {current} cannot transition to {failedTransName}");
    }
}
