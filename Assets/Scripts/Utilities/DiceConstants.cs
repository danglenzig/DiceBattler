using UnityEngine;
namespace Dice
{
    public static class EncounterStateStrings
    {
        public const string SETUP_STATE = "ENCOUNTER_SETUP";
        public const string DRAWUP_STATE = "TURN_DRAW_UP";
        public const string SELECT_STATE = "TURN_SELECT_STATE";
        public const string ROLLING_STATE = "TURN_ROLLING_STATE";
        public const string RESOLUTION_STATE = "TURN_RESOLUTION_STATE";
        public const string AFTERMATH_STATE = "TURN_AFTERMATH_STATE";
        public const string PLAYER_DEAD_STATE = "ENCOUNTER_PLAYER_DEATH";
        public const string PLAYER_WIN_STATE = "ENCOUNTER_PLAYER_WIN";
    }

    public static class ActionTagStrings
    {
        public const string ATTACK = "ACTION.ATTACK";
        public const string EVASION = "ACTION.EVASION";
    }
}

