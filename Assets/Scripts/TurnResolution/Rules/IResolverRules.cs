using UnityEngine;

namespace Dice
{
    public interface IResolverRules
    {
        /*
         NOTE: The rules should only remove status effects based on roll results
               That is, removing expired effects (Duration reaches zero) is the job
               off the encounter manager (e.g. ResolverTester, etc.)
         */

        public TurnResolution GetTableauResolution(TableauResult tableauResult);

        public string SayHello();
    }
}
