using System;
using Batmind.Tree.Nodes.Actions;

namespace Batmind.Editor.Nodes
{
    public class ActionNodeView : NodeView<ActionNode>
    {
        private readonly Action<Tree.Nodes.Node> _onSelectionChanged;
        public Type ExplicitNodeType { get; set; }

        public ActionNodeView(ActionNode actionNode, Action<Tree.Nodes.Node> onSelectionChanged) : base(actionNode)
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