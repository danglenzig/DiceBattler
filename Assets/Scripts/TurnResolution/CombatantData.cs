using System.Collections.Generic;
using UnityEngine;
namespace Dice
{
    public sealed class CombatantData
    {
        public int HP { get; private set; }
        // more stats as needed...
        public IReadOnlyList<StatusEffect> ActiveStatusEffects { get; private set; } = new List<StatusEffect>();
        public RuntimeDiceBag DrawBag { get; private set; }
        public RuntimeDiceBag DiscardBag { get; private set; }
        public RuntimeDiceBag InHand {  get; private set; }
        public string CombatantID { get; private set; }
        public string CombatantName { get; private set; } = "Namey Nameson";

        public void SetHP(int hp)
        {
            HP = Mathf.Max(0, hp);
        }
        public void SetActiveStatusEffects(IReadOnlyList<StatusEffect> statusEffectList)
        {
            if (statusEffectList != null)
            {
                ActiveStatusEffects = statusEffectList;
            }
        }

        public void SetCombatantName(string combatantName)
        {
            CombatantName = combatantName;
        }

        public void SetCombatantID(string combatantID)
        {
            CombatantID = combatantID;
        }

        public void SetDrawBag(RuntimeDiceBag bag)
        {
            DrawBag = bag;
        }

        public void SetInHand(RuntimeDiceBag bag)
        {
            InHand = bag;
        }

        public void SetDiscardBag(RuntimeDiceBag bag)
        {
            DiscardBag = bag;
        }
    }
}

