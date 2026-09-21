using System.Collections.Generic;
using UnityEngine;

namespace Dice
{
    public sealed class CombatantEffects
    {
        public int HPChange { get; private set; }
        // and so on...

        public IReadOnlyList<StatusEffect> AddedStatusEffects { get; private set; } = new List<StatusEffect>();
        public IReadOnlyList<StatusEffect> RemovedStatusEffects { get; private set; } = new List<StatusEffect>();

        public void SetHPChange(int hpChange)
        {
            HPChange = hpChange;
        }
        public void SetAddedStatusEffects(IReadOnlyList<StatusEffect> addedStatusEffects)
        {
            if (addedStatusEffects != null)
            {
                AddedStatusEffects = addedStatusEffects;
            }
        }
        public void SetRemovedStatusEffects(IReadOnlyList<StatusEffect> removedStatusEffects)
        {
            if (removedStatusEffects != null)
            {
                RemovedStatusEffects = removedStatusEffects;
            }
        }
    }
}
