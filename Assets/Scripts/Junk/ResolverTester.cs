using UnityEngine;
using UnityEngine.UI;
using Dice;
using System.Collections.Generic;

namespace Junk
{
    public class ResolverTester : MonoBehaviour
    {
        [SerializeField] private TurnTableau _tableau;
        [SerializeField] private Button _rollButton;

        private TestRules _rules = new TestRules();
        CombatantData _playerData = new CombatantData();
        CombatantData _opponentData = new CombatantData();

        private void Awake()
        {
            TestRules rules = new TestRules();
        }

        private void OnEnable()
        {
            _rollButton.onClick.AddListener(HandleOnRollPressed);
            _tableau.OnTableauResultAnnounced += HandleTableuResult;
        }
        private void OnDisable()
        {
            _rollButton.onClick.RemoveAllListeners();
            _tableau.OnTableauResultAnnounced -= HandleTableuResult;
        }

        void Start()
        {
            string testEffectTag = "STATUS_EFFECT.TEMP";
            StatusEffect effect = new StatusEffect();
            effect.SetEffectTag(testEffectTag);
            effect.SetDuration(3);

            IReadOnlyList<StatusEffect> effects = new List<StatusEffect>() { effect };

            _playerData.SetCombatantName("Player");
            _playerData.SetHP(20);
            _playerData.SetActiveStatusEffects(effects);
            _opponentData.SetCombatantName("Opponent");
            _opponentData.SetHP(20);

            _tableau.SetCombatants(_playerData, _opponentData);
        }

        private void HandleOnRollPressed()
        {
            if (!_tableau.ReadyToRoll()) return;

            _rollButton.interactable = false;
            _tableau.Roll();
        }

        private void HandleTableuResult(TableauResult tableauResult)
        {
            TurnResolution tableauResolution = _rules.GetTableauResolution(tableauResult);

            CombatantEffects playerEffects = tableauResolution.PlayerEffects;
            CombatantEffects opponentEffects = tableauResolution.OpponentEffects;

            int playerHPChange = playerEffects.HPChange;
            IReadOnlyList<StatusEffect> playerAddedStatusEffects = playerEffects.AddedStatusEffects;
            IReadOnlyList<StatusEffect> playerRemovedStatusEffects = playerEffects.RemovedStatusEffects;

            int opponentHPChange = opponentEffects.HPChange;
            IReadOnlyList<StatusEffect> opponentAddedStatusEffects = opponentEffects.AddedStatusEffects;
            IReadOnlyList<StatusEffect> opponentRemovedStatusEffects = opponentEffects.RemovedStatusEffects;
            _playerData.SetHP(_playerData.HP + playerHPChange);
            _opponentData.SetHP(_opponentData.HP + opponentHPChange);

            // Handle the post-turn status effect changes

            UpdateStatusEffects(_playerData, playerAddedStatusEffects, playerRemovedStatusEffects);
            UpdateStatusEffects(_opponentData, opponentAddedStatusEffects, opponentRemovedStatusEffects);


            DebugCombatantData(_playerData);
            DebugCombatantData(_opponentData);
            Debug.Log("===========");

            _rollButton.interactable = true;
        }

        private void UpdateStatusEffects(
            CombatantData combatantData,
            IReadOnlyList<StatusEffect> addedList,
            IReadOnlyList<StatusEffect> removedList)
        {
            // ActiveStatusEffects is a read-only list, so we'll construct a new list and swap it out
            List<StatusEffect> newCombatantActiveStatusEffects = new List<StatusEffect>();

            // add the ones added this turn
            for (int i=0; i<addedList.Count; i++)
            {
                newCombatantActiveStatusEffects.Add(addedList[i]);
            }

            // add the still-valid status effects, drecrementing their duration by 1
            foreach (StatusEffect effect in combatantData.ActiveStatusEffects)
            {
                string tag = effect.EffectTag;

                bool inRemovedList = false;
                for (int i = 0; i < removedList.Count; i++)
                {
                    if (removedList[i].EffectTag == tag)
                    {
                        inRemovedList = true;
                        break;
                    }
                }

                bool alreadyAdded = false;
                for (int i = 0; i < newCombatantActiveStatusEffects.Count; i++)
                {
                    if (newCombatantActiveStatusEffects[i].EffectTag == tag)
                    {
                        alreadyAdded = true;
                        break;
                    }
                }

                // is the status effect in the removed list, or is its duration <= 1?
                bool removed = (effect.Duration <= 1 || inRemovedList);
                if (removed) //yes
                {
                    // - DO NOT add it to newPlayerActiveStatusEffects
                    // - DO pop off an event saying the status effect was removed.
                }
                else // no
                {
                    // - Add the new status effect to newPlayerActiveStatusEffects
                    //   but only if we haven't already added one with the same tag, above
                    if (!alreadyAdded)
                    {
                        // - Make a new status effect with the same tag
                        StatusEffect updatedStatusEffect = new StatusEffect();
                        updatedStatusEffect.SetEffectTag(effect.EffectTag);

                        // - Set its durarion to effect.Duration - 1
                        updatedStatusEffect.SetDuration(effect.Duration - 1);

                        // add it
                        newCombatantActiveStatusEffects.Add(updatedStatusEffect);

                        // pop off an event saying this tag was added.
                    }
                }
            }
            combatantData.SetActiveStatusEffects(newCombatantActiveStatusEffects);
        }

        private void DebugCombatantData(CombatantData data)
        {   
            int hp = data.HP;
            IReadOnlyList<StatusEffect> activeStatusEffects = data.ActiveStatusEffects;

            string cName = data.CombatantName;
            string hpStr = $"HP: {hp.ToString()}";
            string fXString = "";
            foreach (StatusEffect statusEffect in activeStatusEffects)
            {
                fXString += statusEffect.EffectTag;
            }

            Debug.Log($"### {name}: {cName}:\n  {hpStr}\n  {fXString}");
        }
    }
}