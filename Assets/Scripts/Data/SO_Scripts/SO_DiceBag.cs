using UnityEngine;
using System.Collections.Generic;

namespace Dice
{
    [CreateAssetMenu(fileName = "SO_DiceBag", menuName = "Dice/Dice Bag")]
    public class SO_DiceBag : ScriptableObject
    {
        [SerializeField] private string _bagName;

        [SerializeField] private List<SO_Die> _dieList;

        [SerializeField] private List<string> _tagStrings;

        [HideInInspector] public string BagName { get { return _bagName; } }
        
        [HideInInspector] public List<SO_Die> DieList { get { return _dieList; } }

        [HideInInspector] public List<string> TagStrings { get { return _tagStrings; } }


        public RuntimeDiceBag GetRuntimeDiceBag()
        {
            List<RuntimeDie> runtimeDice = new();
            foreach (SO_Die die in _dieList)
            {
                runtimeDice.Add(die.GetRuntimeDie());
            }

            return new RuntimeDiceBag(_bagName, runtimeDice, _tagStrings);
        }

    }
}


