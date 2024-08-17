using System;
using Batmind.Tree.Nodes.Conditions;

namespace Batmind.Editor.Nodes
{
    public class ConditionNodeView : NodeView<ConditionNode>
    {
        public Type ExplicitNodeType { get; set; }

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