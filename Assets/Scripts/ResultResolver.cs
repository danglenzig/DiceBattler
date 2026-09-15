using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Dice
{
    public sealed class StatusEffect
    {
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

    public sealed class CombatantData
    {
        public int HP { get; private set; }
        // more stats as needed...
        public IReadOnlyList<StatusEffect> ActiveStatusEffects { get; private set; } = new List<StatusEffect>();

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
    }

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
            if(opponentEffects != null)
            {
                OpponentEffects = opponentEffects;
            }
        }
    }

    public sealed class DieFaceResult
    {
        public int IntegerValue { get; private set; }
        public IReadOnlyList<string> TagStrings { get; private set; } = new List<string>();

        public void SetIntegerValue(int integerValue)
        {
            IntegerValue = integerValue;
        }
        public void SetTagStrings(IReadOnlyList<string> tagStrings)
        {
            if(tagStrings != null)
            {
                TagStrings = tagStrings;
            }
        }
    }

    public sealed class RollResult
    {
        public IReadOnlyList<DieFaceResult> PlayerRoll { get; private set; } = new List<DieFaceResult>();
        public IReadOnlyList<DieFaceResult> OpponentRoll { get; private set; } = new List<DieFaceResult>();

        public void SetPlayerRoll(IReadOnlyList<DieFaceResult> playerRoll)
        {
            if (playerRoll != null)
            {
                PlayerRoll = playerRoll;
            }
        }
        public void SetOpponentRoll(IReadOnlyList<DieFaceResult> opponentRoll)
        {
            if(opponentRoll != null)
            {
                OpponentRoll = opponentRoll;
            }
        }
    }

    /*
    public interface IResolverRules
    {
        TurnResolution GetOutcome(
            RollResult rollResult,
            CombatantData player,
            CombatantData opponent);
    }
    */

    // Various implementations of IResolverRules defined in their own scripts...

    public sealed class CombatResolver
    {
        private readonly IResolverRules _rules;

        public CombatResolver(IResolverRules rules)
        {
            _rules = rules;
        }

        public TurnResolution Resolve(
            RollResult rollResult,
            CombatantData player,
            CombatantData opponent)
        {
            return _rules.GetOutcome(
                rollResult,
                player,
                opponent);
        }

        public string SayHello()
        {
            return $"Combat resolver says hi!\n{_rules.SayHello()}";
        }
    }
}

