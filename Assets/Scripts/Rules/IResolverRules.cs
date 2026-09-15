using UnityEngine;

namespace Dice
{
    public interface IResolverRules
    {
        public TurnResolution GetOutcome(
            RollResult rollResult,
            CombatantData player,
            CombatantData opponent);

        public string SayHello();
    }
}
