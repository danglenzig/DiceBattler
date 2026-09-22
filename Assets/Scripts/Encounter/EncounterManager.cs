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

        [SerializeField] private SO_DiceBag _defaultDiceBagData;

        private ITableau _tableau;

        private RuntimeSSM _encounterStateMachine;

        private IResolverRules _rules;

        CombatantData _playerData = new CombatantData();
        CombatantData _opponentData = new CombatantData();

        private void OnValidate()
        {
            // TODO...
        }

        private void Awake()
        {
            // state transition events
            _encounterStateMachine = GetComponent<RuntimeSSM>();
            _tableau = GetComponent<ITableau>();
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
            SetupTableau();
        }

        private void SetupTableau()
        {
            string startingEffectTag = "STATUS_EFFECT.UNHARMED";
            StatusEffect startingEffect = new StatusEffect();
            startingEffect.SetEffectTag(startingEffectTag);
            startingEffect.SetDuration(-1);

            // set up the tableau data...
            //_tableau.SetCombatants(...)
            // for now, we're just creating them here.
            // TODO: construct these from data...
            _playerData = new CombatantData();
            _playerData.SetCombatantName("Player Playerson");
            _playerData.SetCombatantID("ABC123");
            _playerData.SetHP(20);
            _playerData.SetActiveStatusEffects(new List<StatusEffect>() { startingEffect });
            _playerData.SetDrawBag(_defaultDiceBagData.GetRuntimeDiceBag());
            _playerData.SetInHand(new RuntimeDiceBag());
            _playerData.SetDiscardBag(new RuntimeDiceBag());

            _opponentData = new CombatantData();
            _opponentData.SetCombatantName("Bad guy");
            _opponentData.SetCombatantID("DEF456");
            _opponentData.SetHP(20);
            _opponentData.SetActiveStatusEffects(new List<StatusEffect>() { startingEffect });
            _opponentData.SetDrawBag(_defaultDiceBagData.GetRuntimeDiceBag());
            _opponentData.SetInHand(new RuntimeDiceBag());
            _opponentData.SetDiscardBag(new RuntimeDiceBag());

            _tableau.SetCombatants(_playerData, _opponentData);

            // now tell the tableau to do its encounter start animations, etc
            // include a reference to the state machine, and a the DRAWUP_STATE
            // string. When it's done doing its thing, it will call the transition
            // method on the provided SM with the provided toState string.
            // We'll know it's fininished when we get the StateEntered event
            // with payload DRAWUP_STATE
            _tableau.EncounterStart(_encounterStateMachine, EncounterStateStrings.DRAWUP_STATE);

        }

        private void HandleOnStateEntered(string enteredStateString)
        {
            switch (enteredStateString)
            {
                case EncounterStateStrings.SETUP_STATE:
                    return;
                case EncounterStateStrings.DRAWUP_STATE:

                    

                    return;
                case EncounterStateStrings.SELECT_STATE:
                    return;
                case EncounterStateStrings.ROLLING_STATE:
                    return;
                case EncounterStateStrings.RESOLUTION_STATE:
                    return;
                case EncounterStateStrings.AFTERMATH_STATE:
                    return;
                case EncounterStateStrings.PLAYER_DEAD_STATE:
                    return;
                case EncounterStateStrings.PLAYER_WIN_STATE:
                    return;
                default:
                    return;
            }
        }
        private void HandleOnStateExited(string exitedStateString)
        {
            switch (exitedStateString)
            {
                case EncounterStateStrings.SETUP_STATE:
                    return;
                case EncounterStateStrings.DRAWUP_STATE:
                    return;
                case EncounterStateStrings.SELECT_STATE:
                    return;
                case EncounterStateStrings.ROLLING_STATE:
                    return;
                case EncounterStateStrings.RESOLUTION_STATE:
                    return;
                case EncounterStateStrings.AFTERMATH_STATE:
                    return;
                case EncounterStateStrings.PLAYER_DEAD_STATE:
                    return;
                case EncounterStateStrings.PLAYER_WIN_STATE:
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


