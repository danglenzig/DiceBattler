using UnityEngine;
using System.Collections.Generic;

namespace Dice
{
    [CreateAssetMenu(fileName = "SO_DiceBag", menuName = "Dice/Dice Bag")]
    public class SO_DiceBag : ScriptableObject
    {
        [SerializeField] private string _bagName;
        [SerializeField] private List<SO_Die> _dieList;

        [HideInInspector] public string BagName { get { return _bagName; } }
        [HideInInspector] public List<SO_Die> DieList { get { return _dieList; } }
    }
}


