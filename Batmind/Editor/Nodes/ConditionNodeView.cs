using System;
using Batmind.Tree.Nodes.Conditions;
using Batmind.Utils;
using UnityEngine;

namespace Batmind.Editor.Nodes
{
    public class ConditionNodeView : NodeView<ConditionNode>
    {
        public Type ExplicitNodeType { get; set; }
        protected override Color BackgroundColor => ColorExtensions.FromInt(71, 217, 144); 

        public ConditionNodeView(ConditionNode conditionNode, Action<NodeView> onSelectionChanged) 
            : base(conditionNode, onSelectionChanged)
        {
        }
        
        protected override void AddOutputPort()
        {
            // Emptied since there is no need for output nodes for action nodes.
        }
    }
}