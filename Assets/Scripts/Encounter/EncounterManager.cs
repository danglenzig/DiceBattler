using UnityEngine;
using SimpleStateMachine;
using EventChannels;
using System.Collections.Generic;

namespace Dice
{
    [RequireComponent(typeof(RuntimeSSM))]
    [RequireComponent(typeof(TurnTableau))]
    public sealed class EncounterManager : MonoBehaviour
    {
        private const string SETUP_STATE = "ENCOUNTER_SETUP";
        private const string DRAWUP_STATE = "TURN_DRAW_UP";
        private const string SELECT_STATE = "TURN_SELECT_STATE";
        private const string ROLLING_STATE = "TURN_ROLLING_STATE";
        private const string RESOLUTION_STATE = "TURN_RESOLUTION_STATE";
        private const string AFTERMATH_STATE = "TURN_AFTERMATH_STATE";
        private const string PLAYER_DEAD_STATE = "ENCOUNTER_PLAYER_DEATH";
        private const string PLAYER_WIN_STATE = "ENCOUNTER_PLAYER_WIN";


        /*
         NOTE: Look at ResolverTester.cs in another tab while you're making this one
         */

        private TurnTableau _tableau;

        private RuntimeSSM _encounterStateMachine;

        private IResolverRules _rules;

        private void OnValidate()
        {
            // TODO...
        }

        private void Awake()
        {
            // state transition events
            _encounterStateMachine = GetComponent<RuntimeSSM>();
            _tableau = GetComponent<TurnTableau>();
            _encounterStateMachine.StateEntered += HandleOnStateEntered;
            _encounterStateMachine.StateExited += HandleOnStateExited;

            // tableau result announcement
            _tableau.OnTableauResultAnnounced += HandleTableauResult;



            // for now...
            _rules = new TestRules();
        }

        private void OnDestroy()
        {
            // disconnect the state machine
            _encounterStateMachine.StateEntered -= HandleOnStateEntered;
            _encounterStateMachine.StateExited -= HandleOnStateExited;

            // disconnect the tableau
            _tableau.OnTableauResultAnnounced -= HandleTableauResult;
        }

        void Start()
        {

        }

        /*
        void Update()
        {

        }
        */

        private void SetupTableau()
        {
            // set up the tableau data...
            //_tableau.SetCombatants(...)

            // now tell the tableau to do its encounter start animations, etc
            // include a reference to the state machine, and a the DRAWUP_STATE
            // string. When it's done doing its thing, it will call the transition
            // method on the provided SM with the provided toState string.
            // We'll know it's fininished when we get the StateEntered event
            // with payload DRAWUP_STATE

        }

        private void HandleOnStateEntered(string enteredStateString)
        {
            switch (enteredStateString)
            {
                case SETUP_STATE:
                    return;
                case DRAWUP_STATE:
                    return;
                case SELECT_STATE:
                    return;
                case ROLLING_STATE:
                    return;
                case RESOLUTION_STATE:
                    return;
                case AFTERMATH_STATE:
                    return;
                case PLAYER_DEAD_STATE:
                    return;
                case PLAYER_WIN_STATE:
                    return;
                default:
                    return;
            }
        }
        private void HandleOnStateExited(string exitedStateString)
        {
            switch (exitedStateString)
            {
                case SETUP_STATE:
                    return;
                case DRAWUP_STATE:
                    return;
                case SELECT_STATE:
                    return;
                case ROLLING_STATE:
                    return;
                case RESOLUTION_STATE:
                    return;
                case AFTERMATH_STATE:
                    return;
                case PLAYER_DEAD_STATE:
                    return;
                case PLAYER_WIN_STATE:
                    return;
                default:
                    return;
            }
        }

        private void HandleTableauResult(TableauResult tableauResult)
        {

        }

    }
}


