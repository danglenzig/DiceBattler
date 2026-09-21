using System.Collections.Generic;
using UnityEngine;

namespace Dice
{
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
            if (tagStrings != null)
            {
                TagStrings = tagStrings;
            }
        }
    }
}