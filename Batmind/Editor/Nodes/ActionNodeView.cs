using System;
using Batmind.Tree.Nodes.Actions;
using Batmind.Utils;
using UnityEngine;

namespace Batmind.Editor.Nodes
{
    public class ActionNodeView : NodeView<ActionNode>
    {
        private readonly Action<Tree.Nodes.Node> _onSelectionChanged;
        public Type ExplicitNodeType { get; set; }
        protected override Color BackgroundColor => ColorExtensions.FromInt(182, 226, 211);

        public ActionNodeView(ActionNode actionNode, Action<NodeView> onSelectionChanged) 
            : base(actionNode, onSelectionChanged)
        {
            
        }
        
        protected override void AddOutputPort()
        {
            // Emptied since there is no need for output nodes for action nodes.
        }
    }
}