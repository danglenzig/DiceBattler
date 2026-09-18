using Dice;
using System.Collections.Generic;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

namespace MiscTools
{

    public struct Transform2DTweaksStruct
    {
        public Vector3 PosTweak;
        public Vector3 AngleTweak;

        public Transform2DTweaksStruct(Vector3 _posTweak, Vector3 _angleTweak)
        {
            this.PosTweak = _posTweak;
            this.AngleTweak = _angleTweak;
        }
    }

    public static class MiscTransformTools
    {
        public static Transform2DTweaksStruct GetTransformTweak2D(float maxTweakPos, float maxTweakAngle)
        {
            Transform2DTweaksStruct tweaks = new Transform2DTweaksStruct(Vector3.zero, Vector3.zero);

            // ...

            float posXRando = UnityRandom.Range(-maxTweakPos, maxTweakPos);
            float posYRando = UnityRandom.Range(-maxTweakPos, maxTweakPos);
            Vector3 posAdjust = new Vector3(posXRando, posYRando, 0f);
            tweaks.PosTweak = posAdjust;

            float degreesRando = UnityRandom.Range(-maxTweakAngle, maxTweakAngle);
            Vector3 degreesAdjust = new Vector3(0, 0, degreesRando);
            tweaks.AngleTweak = degreesAdjust;

            return tweaks;
        }
    }

    public static class TagStringTools
    {
        public static bool IsAMatch(string tagStringToTest, string tagStringToMatch)
        {
            return tagStringToTest.StartsWith(tagStringToMatch);
        } 
    }

    public static class DiceTools
    {
        public static int NumFaces(DieType deeType)
        {
            switch (deeType)
            {
                case DieType.D2: return 2;
                case DieType.D4: return 4;
                case DieType.D6: return 6;
                case DieType.D8: return 8;
                case DieType.D10: return 10;
                case DieType.D12: return 12;
                case DieType.D20: return 20;
                default: return -1;
            }
        }

        public static IReadOnlyList<int> GetAdjacentFaceIndexes(int currentFaceIndex, DieType deeType)
        {
            IReadOnlyList<List<int>> deeSixAdjacentFaces = new List<List<int>>()
            {
                new List<int> { 1,2,3,4 }, // 0
                new List<int> { 0,2,3,5 }, // 1
                new List<int> { 0,1,4,5 }, // 2
                new List<int> { 0,1,4,5 }, // 3
                new List<int> { 0,2,3,5 }, // 4
                new List<int> { 1,2,3,4 }, // 5
            };

            // and so on...

            switch (deeType)
            {
                case DieType.D6: return deeSixAdjacentFaces[currentFaceIndex];

                // and so on...

                default: break;
            }
            return new List<int>() { currentFaceIndex };
        }
    }
}


