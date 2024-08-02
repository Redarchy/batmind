using Batmind.Board;
using UnityEngine;

namespace Batmind.Tree.Nodes.Conditions
{
    [System.Serializable]
    public class IsEntryAccessorSatisfiedCondition : ConditionNode
    {
        [SerializeField] public EntryAccessor<bool> _Condition;

        protected override bool IsConditionSatisfied()
        {
            if (_Condition.RuntimeEntry == null)
            {
                Debug.Log("Entry Accessor is not assigned!");
                return false;
            }

            return _Condition.RuntimeEntry.Value.BoolValue;
        }
    }
}