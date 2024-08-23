using System;
using Batmind.Tree.Nodes.Conditions;
using UnityEngine;

namespace Batmind.Editor.Nodes
{
    public class ConditionNodeView : NodeView<ConditionNode>
    {
        public Type ExplicitNodeType { get; set; }
        protected override Color BackgroundColor => new (71 / 255f, 217 / 255f, 144 / 255f, 1); 

        public ConditionNodeView(ConditionNode conditionNode, Action<Tree.Nodes.Node> onSelectionChanged) 
            : base(conditionNode, onSelectionChanged)
        {
        }
        
        protected override void AddOutputPort()
        {
            // Emptied since there is no need for output nodes for action nodes.
        }
    }
}