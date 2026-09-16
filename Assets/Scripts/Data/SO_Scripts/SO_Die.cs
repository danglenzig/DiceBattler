using System.Collections.Generic;
using UnityEngine;

namespace Dice
{

    public enum DieType
    {
        D2,D4,D6,D8,D10,D12,D20
    }

    [CreateAssetMenu(fileName = "SO_Die", menuName = "Dice/Die")]
    public class SO_Die : ScriptableObject
    {
        [SerializeField] private string _dieName;
        [SerializeField] private DieType _deeType = DieType.D6; 
        [SerializeField] private List<SO_DieFace> _faces;
        [SerializeField] private List<string> _tagStrings;

        [HideInInspector] public string DieName { get { return _dieName; } }
        [HideInInspector] public DieType DeeType { get { return _deeType; } }
        [HideInInspector] public List<SO_DieFace> FaceSOs { get { return _faces; } }
        [HideInInspector] public List<string> TagStrings { get { return _tagStrings; } }

        private void OnValidate()
        {
            foreach (SO_DieFace face in _faces)
            {
                if (face == null)
                {
                    Debug.LogError($"### {name}: All faces must be non-null");
                    return;
                }
            }

            switch (_deeType)
            {
                case DieType.D6:
                    if (_faces.Count != 6)
                    {
                        Debug.LogError($"### {name}: DeeSix dice must have six non-null DieFaces");
                        return;
                    }
                    break;

                // and so on

                default:
                    break;
            }

        }

        public RuntimeDie GetRuntimeDie()
        {
            List<RuntimeDieFace> runtimeFaces = new();
            foreach (SO_DieFace face in _faces)
            {
                runtimeFaces.Add(face.GetRuntimeDieFace());
            }

            return new RuntimeDie(_dieName, _deeType, runtimeFaces, _tagStrings);
        }

    }
}


