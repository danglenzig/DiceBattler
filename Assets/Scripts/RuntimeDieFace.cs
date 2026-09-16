using System.Collections.Generic;
using UnityEngine;

namespace Dice
{
    public sealed class RuntimeDieFace
    {
        private int _integerValue;
        private List<string> _tagStrings;
        private Sprite _imageSprite;

        public int IntegerValue { get { return _integerValue; } }
        public List<string> TagStrings { get { return _tagStrings; } }
        public Sprite ImageSprite { get { return _imageSprite; } }

        // public constructor
        public RuntimeDieFace(
            int integerValue,
            List<string> tagstrings,
            Sprite imageSprite
            )
        {
            _integerValue = integerValue;
            _tagStrings = tagstrings;
            _imageSprite = imageSprite;
        }
    }
}

