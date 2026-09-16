using System.Collections.Generic;
using UnityEngine;

namespace SimpleStateMachine
{
    [CreateAssetMenu(fileName = "SO_SimpleState", menuName = "Simple State Machine/Simple State")]
    public class SO_SimpleState : ScriptableObject
    {
        [SerializeField] private string _stateString;
        [SerializeField, Tooltip("What happens during this state")][Multiline] private string _stateDescription = "What happens during this state";
        
        [SerializeField] private List<SO_SimpleState> _toStates;

        [HideInInspector] public string StateString { get { return _stateString;  } }
        [HideInInspector] public string StateDescription { get { return _stateDescription; } }
        [HideInInspector] public List<SO_SimpleState> ToStates { get { return _toStates; } }
    }
}


