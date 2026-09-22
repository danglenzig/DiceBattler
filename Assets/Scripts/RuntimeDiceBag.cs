using System.Collections.Generic;
using UnityEngine;

namespace Dice
{
    public class RuntimeDiceBag
    {
        private string _bagName;
        private List<RuntimeDie> _dieList;
        private List<string> _tagStrings;

        public string BagName { get { return _bagName; } }
        public List<RuntimeDie> DieList { get { return _dieList; } }
        public List<string> TagStrings { get { return _tagStrings; } }

        public RuntimeDiceBag()
        {
            _bagName = string.Empty;
            _dieList = new List<RuntimeDie>();
            _tagStrings = new List<string>();
        }

        public RuntimeDiceBag(
            string bagName,
            List<RuntimeDie> dieList,
            List<string> tagStrings
            )
        {
            _bagName = bagName;
            _dieList = dieList;
            _tagStrings = tagStrings;
        }
    }
    
}

