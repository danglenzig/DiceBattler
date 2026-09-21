using UnityEngine;
namespace Dice
{
    public sealed class TurnResolution
    {
        public CombatantEffects PlayerEffects { get; private set; } = new CombatantEffects();
        public CombatantEffects OpponentEffects { get; private set; } = new CombatantEffects();

        public void SetPlayerEffects(CombatantEffects playerEffects)
        {
            if (playerEffects != null)
            {
                PlayerEffects = playerEffects;
            }
        }
        public void SetOpponentEffects(CombatantEffects opponentEffects)
        {
            if (opponentEffects != null)
            {
                OpponentEffects = opponentEffects;
            }
        }
    }
}

