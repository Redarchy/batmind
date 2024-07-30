using System;
using Batmind.Tree.Nodes.Conditions;

namespace Batmind.Editor.Nodes
{
    public class ConditionNodeView : NodeView<ConditionNode>
    {
        private readonly Action<Tree.Nodes.Node> _onSelectionChanged;
        public Type ExplicitNodeType { get; set; }

        public ConditionNodeView(ConditionNode conditionNode, Action<Tree.Nodes.Node> onSelectionChanged) : base(conditionNode)
        {
            _onSelectionChanged = onSelectionChanged;
        }
        
        protected override void AddOutputPort()
        {
            // Emptied since there is no need for output nodes for action nodes.
        }

        public override void OnSelected()
        {
            base.OnSelected();
            
            _onSelectionChanged?.Invoke(TreeNode);
        }

    }
}