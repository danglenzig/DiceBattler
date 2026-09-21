using UnityEngine;

namespace Dice
{
    public sealed class StatusEffect
    {
        // NOTE: StatusEffects DO NOT stack.

        public string EffectTag { get; private set; } = "";
        // ^^Example STATUS_EFFECT.WEAKENED, STATUS_EFFECT.INSPIRED, etc.

        public int Duration { get; private set; } = -1;
        // ^^Number of turns. -1 means permanent or persistent until removed by another effect

        public void SetEffectTag(string effectTag)
        {
            EffectTag = effectTag;
        }
        public void SetDuration(int duration)
        {
            Duration = duration;
        }
    }
}
