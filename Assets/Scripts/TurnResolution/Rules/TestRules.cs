using System.Collections.Generic;
using MiscTools;
using UnityEngine;


namespace Dice
{
    public sealed class TestRules : IResolverRules
    {

        private const string ATTACK_TAG = "ACTION.ATTACK";
        private const string EVASION_TAG = "ACTION.EVASION";
        private const string DEAD_TAG = "STATUS_EFFECT.DEAD";

        private const int ATTACK_DAMAGE = -6;


        // IResolverRules implementation...

        public string SayHello()
        {
            return "Test rules says hi!";
        }

        public TurnResolution GetTableauResolution(TableauResult tableauResult)
        {
            RollResult rollResult = tableauResult.RollResult;
            CombatantData playerData = tableauResult.PlayerData;
            CombatantData opponentData = tableauResult.OpponentData;

            IReadOnlyList<DieFaceResult> playerRoll = rollResult.PlayerRoll;
            IReadOnlyList<DieFaceResult> opponentRoll = rollResult.OpponentRoll;

            int playerHP = playerData.HP;
            int opponentHP = opponentData.HP;

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
                if (opponentData.HP + ATTACK_DAMAGE <= 0)
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
                if (playerData.HP + ATTACK_DAMAGE <= 0)
                {
                    StatusEffect playerDeadStatus = new StatusEffect();
                    playerDeadStatus.SetDuration(-1);
                    playerDeadStatus.SetEffectTag(DEAD_TAG);
                    playerStatusEffectsAddedThisTurn.Add(playerDeadStatus);
                }
            }

            // NOTE, in this ruleset, we're not removing any status effects,
            // so ...StatusEffectsRemovedThisNurn is just an empty list

            // package up the HP change and added/removes status effects for the player
            CombatantEffects playerTurnEffects = new CombatantEffects();
            playerTurnEffects.SetHPChange(playerDamageThisTurn);
            playerTurnEffects.SetAddedStatusEffects(playerStatusEffectsAddedThisTurn);
            playerTurnEffects.SetRemovedStatusEffects(playerStatusEffectsRemovedThisTurn);

            // package up the HP change and added/removes status effects for the opponent
            CombatantEffects opponentTurnEffects = new CombatantEffects();
            opponentTurnEffects.SetHPChange(opponentDamageThisTurn);
            opponentTurnEffects.SetAddedStatusEffects(opponentStatusEffectsAddedThisTurn);
            opponentTurnEffects.SetRemovedStatusEffects(opponentStatusEffectsRemovedThisTurn);

            // package and return the turn outcome...
            TurnResolution res = new TurnResolution();
            res.SetPlayerEffects(playerTurnEffects);
            res.SetOpponentEffects(opponentTurnEffects);
            return res;
        }
    }
}

