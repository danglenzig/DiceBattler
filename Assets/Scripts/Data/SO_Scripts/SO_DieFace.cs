using System.Collections.Generic;
using UnityEngine;

namespace Dice
{
    [CreateAssetMenu(fileName = "SO_DieFace", menuName = "Dice/Die Face")]
    public class SO_DieFace : ScriptableObject
    {
        [SerializeField, Min(0)] private int _integerValue;
        [SerializeField] private List<string> _tagStrings;
        [SerializeField] private Sprite _imageSprite;

        [HideInInspector] public int IntegerValue { get { return _integerValue; } }
        [HideInInspector] public List<string> TagStrings { get { return _tagStrings; } }
        [HideInInspector] public Sprite ImageSprite { get { return _imageSprite; } }
        public Texture2D GetSpriteTexture()
        {
            return _imageSprite.texture;
        }
        public RuntimeDieFace GetRuntimeDieFace()
        {
            return new RuntimeDieFace(_integerValue, _tagStrings, _imageSprite);
        }

    }
}

