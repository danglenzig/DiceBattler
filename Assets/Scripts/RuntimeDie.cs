using System.Collections.Generic;
using UnityEngine;

namespace Dice
{
    public sealed class RuntimeDie
    {
        private string _dieName;
        private DieType _deeType;
        private List<RuntimeDieFace> _faces;
        private List<string> _tagStrings;

        public string DieName { get { return _dieName; } }
        public DieType DeeType { get { return _deeType; } }
        
        public List<RuntimeDieFace> Faces {get { return _faces; } }
        public List<string> TagStrings { get { return _tagStrings; } }

        public RuntimeDie(
            string dieName,
            DieType deeType,
            List<RuntimeDieFace> faces,
            List<string> tagStrings
            )
        {
            _dieName = dieName;
            _deeType = deeType;
            _faces = faces;
            _tagStrings = tagStrings;
        }

    }
}