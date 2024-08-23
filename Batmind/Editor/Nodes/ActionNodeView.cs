using System;
using Batmind.Tree.Nodes.Actions;
using UnityEngine;

namespace Batmind.Editor.Nodes
{
    public class ActionNodeView : NodeView<ActionNode>
    {
        private readonly Action<Tree.Nodes.Node> _onSelectionChanged;
        public Type ExplicitNodeType { get; set; }
        protected override Color BackgroundColor => new(182 / 255f, 226 / 255f, 211 / 255f, 1);

        public ActionNodeView(ActionNode actionNode, Action<Tree.Nodes.Node> onSelectionChanged) 
            : base(actionNode, onSelectionChanged)
        {
            
        }
        
        protected override void AddOutputPort()
        {
            // Emptied since there is no need for output nodes for action nodes.
        }
    }
}