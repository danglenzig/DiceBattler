using UnityEngine;
using UnityEngine.UI;
using Dice;

namespace Junk
{
    public class ResolverTester : MonoBehaviour
    {
        [SerializeField] private TurnTableau _tableau;
        [SerializeField] private Button _rollButton;

        TestRules _rules = new TestRules();
 
        //private CombatResolver _resolver;

        private void Awake()
        {
            TestRules rules = new TestRules();
            //_resolver = new CombatResolver(rules);
        }

        private void OnEnable()
        {
            _rollButton.onClick.AddListener(HandleOnRollPressed);
        }
        private void OnDisable()
        {
            _rollButton.onClick.RemoveAllListeners();
        }

        void Start()
        {
            //print(_resolver.SayHello());
            // ^^so far, so good!
            Combatant player = new Combatant();
            Combatant opponent = new Combatant();
            _tableau.SetCombatants(player, opponent);
            _tableau.SetResolver(_rules);
        }

        private void HandleOnRollPressed()
        {
            _rollButton.interactable = false;
            _tableau.Roll();
        }

        /*
        void Update()
        {

        }
        */
    }
}