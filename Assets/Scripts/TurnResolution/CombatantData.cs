using System.Collections.Generic;
using UnityEngine;
namespace Dice
{
    public sealed class CombatantData
    {
        public int HP { get; private set; }
        // more stats as needed...
        public IReadOnlyList<StatusEffect> ActiveStatusEffects { get; private set; } = new List<StatusEffect>();

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
    }
}

