using System;
using UnityEngine;

namespace Batmind.Tree.Nodes.Conditions
{
    [Serializable]
    public class IsDayOfMonthPassedCondition : ConditionNode
    {
        [SerializeField] private int _DayToPass;

        protected override bool IsConditionSatisfied()
        {
            var today = DateTime.Now.Day;

            return today > _DayToPass;
        }
    }
}