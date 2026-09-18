using System.Collections.Generic;
using MiscTools;
using UnityEngine;


namespace Dice
{
    public class TestRules : IResolverRules
    {

        private const string ATTACK_TAG = "ACTION.ATTACK";
        private const string EVASION_TAG = "ACTION.EVASION";
        private const string DEAD_TAG = "STATUS_EFFECT.DEAD";

        private const int ATTACK_DAMAGE = -1;


        // IResolverRules implementation...

        public string SayHello()
        {
            return "Test rules says hi!";
        }

        public TurnResolution GetOutcome(
            RollResult rollResult,
            CombatantData player,
            CombatantData opponent)
        {
            IReadOnlyList<DieFaceResult> playerRoll = rollResult.PlayerRoll;
            IReadOnlyList<DieFaceResult> opponentRoll = rollResult.OpponentRoll;

            int playerHP = player.HP;
            int opponentHP = opponent.HP;

            List<StatusEffect> playerStatusEffectsAddedThisTurn = new();
            List<StatusEffect> playerStatusEffectsRemovedThisTurn = new();
            List<StatusEffect> opponentStatusEffectsAddedThisTurn = new();
            List<StatusEffect> opponentStatusEffectsRemovedThisTurn = new();

            int playerAttackTotal    = 0;
            int playerEvasionTotal   = 0;
            int opponentAttackTotal  = 0;
            int opponentEvasionTotal = 0;

            int playerDamageThisTurn = 0;
            int opponentDamageThisTurn = 0;

            //========================================
            // Keep this here for pattern reference...
            // Remove it when implemented elsewhere
            //========================================

            // decrement the duration for any temporary status effects
            // "reverse for loop" pattern is a safe way to remove items
            // from a list while iteration through it

            // Actually, let's maybe just not do this here...
            /*
            for (int i = playerStatusEffects.Count - 1; i >= 0; i--)
            {
                StatusEffect effect = playerStatusEffects[i];

                if (effect.Duration > 1)
                {
                    effect.SetDuration(effect.Duration - 1);
                }
                else
                {
                    playerStatusEffects.RemoveAt(i);
                }
            }
            */



            // add up the player attack and evasion values
            foreach (DieFaceResult result in playerRoll)
            {
                foreach(string tagStr in result.TagStrings)
                {
                    if (MiscTools.TagStringTools.IsAMatch(tagStr, ATTACK_TAG))
                    {
                        //Debug.Log($"### TestRules: Adding {result.IntegerValue} to player attack");
                        playerAttackTotal += result.IntegerValue;
                        break;
                    }
                    if (MiscTools.TagStringTools.IsAMatch(tagStr, EVASION_TAG))
                    {
                        //Debug.Log($"### TestRules: Adding {result.IntegerValue} to player evasion");
                        playerEvasionTotal += result.IntegerValue;
                        break;
                    }
                }
            }

            // add up the opponent attack and evasion values
            foreach (DieFaceResult result in opponentRoll)
            {
                foreach (string tagStr in result.TagStrings)
                {
                    if (MiscTools.TagStringTools.IsAMatch(tagStr, ATTACK_TAG))
                    {
                        //Debug.Log($"### TestRules: Adding {result.IntegerValue} to opponent attack");
                        opponentAttackTotal += result.IntegerValue;
                        break;
                    }
                    if (MiscTools.TagStringTools.IsAMatch(tagStr, EVASION_TAG))
                    {
                        //Debug.Log($"### TestRules: Adding {result.IntegerValue} to opponent evasion");
                        opponentEvasionTotal += result.IntegerValue;
                        break;
                    }
                }
            }

            // - adjust the opponent damage taken during this durn
            // - add the dead tag if the damage amount makes them dead
            if (playerAttackTotal >= opponentEvasionTotal) // will otherwise remain 0
            {
                opponentDamageThisTurn = ATTACK_DAMAGE;
                if (opponent.HP + ATTACK_DAMAGE <= 0)
                {
                    StatusEffect opponentDeadStatus = new StatusEffect();
                    opponentDeadStatus.SetDuration(-1);
                    opponentDeadStatus.SetEffectTag(DEAD_TAG);
                    opponentStatusEffectsAddedThisTurn.Add(opponentDeadStatus);
                }
            }
            // - adjust the player damage taken during this durn
            // - add the dead tag if the damage amount makes them dead
            if (opponentAttackTotal >= playerEvasionTotal) // otherwise will remain 0
            {
                playerDamageThisTurn = ATTACK_DAMAGE;
                if (player.HP + ATTACK_DAMAGE <= 0)
                {
                    StatusEffect playerDeadStatus = new StatusEffect();
                    playerDeadStatus.SetDuration(-1);
                    playerDeadStatus.SetEffectTag(DEAD_TAG);
                    playerStatusEffectsAddedThisTurn.Add(playerDeadStatus);
                }
            }

            // TODO: figure out where is the best place to handle decrementing the effects durations.
            // for now, just fuck it...

            // package up the HP change and added/removes status effects for the player
            CombatantEffects playerTurnEffects = new CombatantEffects();
            playerTurnEffects.SetHPChange(playerDamageThisTurn);
            playerTurnEffects.SetAddedStatusEffects(playerStatusEffectsAddedThisTurn);

            // package up the HP change and added/removes status effects for the opponent
            CombatantEffects opponentTurnEffects = new CombatantEffects();
            opponentTurnEffects.SetHPChange(opponentDamageThisTurn);
            opponentTurnEffects.SetAddedStatusEffects(playerStatusEffectsAddedThisTurn);

            // package and return the turn outcome...
            TurnResolution res = new TurnResolution();
            res.SetPlayerEffects(playerTurnEffects);
            res.SetOpponentEffects(opponentTurnEffects);
            return res;

        }
    }
}

