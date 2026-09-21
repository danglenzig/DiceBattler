using EventChannels;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

namespace SimpleStateMachine
{
    public class RuntimeSSM : MonoBehaviour
    {
        [SerializeField] private SO_SimpleStateMachine _stateMachineData;

        [SerializeField] private int _maxHistory = 50;

        private SO_SimpleState _currentState;
        private List<string> _stateHistory = new List<string>();

        [HideInInspector] public SO_SimpleState CurrentState { get { return _currentState; } }
        [HideInInspector] public IReadOnlyList<string> StateHistoryStrings { get { return _stateHistory; } }

        public event System.Action<string> StateEntered;
        public event System.Action<string> StateExited;


        private void Awake()
        {
            _currentState = _stateMachineData.States[0];

        }

        private void OnEnable()
        {
            
        }
        private void OnDisable()
        {
            
        }

        void Start()
        {
            
            Debug.Log($"### {name}: Initial state: {_currentState.StateString}");
        }

        /*
        void Update()
        {

        }
        */

        private void FixStateHistory(string lastExitedStateString)
        {
            _stateHistory.Add(lastExitedStateString);
            if (_stateHistory.Count > _maxHistory)
            {
                _stateHistory.RemoveAt(0);
            }
        }

        //=====
        // API
        //=====

        public bool TryTakeTransition(string toStateString)
        {
            List<string> allowedTransitions = new List<string>();
            foreach (SO_SimpleState state in _currentState.ToStates)
            {
                if (state.StateString == toStateString)
                {
                    FixStateHistory(_currentState.StateString);
                    StateExited?.Invoke(_currentState.StateString);

                    _currentState = state;
                    StateEntered?.Invoke(_currentState.StateString);

                    return true;
                }
            }
            return false;
        }




    }
}

