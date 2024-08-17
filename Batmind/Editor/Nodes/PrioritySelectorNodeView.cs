using System;
using Batmind.Tree.Nodes.Composites;
using UnityEditor.Experimental.GraphView;

namespace Batmind.Editor.Nodes
{
    public class PrioritySelectorNodeView : NodeView<PrioritySelector>
    {
        protected override Port.Capacity OutputPortCapacity => Port.Capacity.Multi;

        public PrioritySelectorNodeView(PrioritySelector prioritySelector, Action<Tree.Nodes.Node> onSelectionChanged) 
            : base(prioritySelector, onSelectionChanged)
        {
            
        }

        protected override void SetStyle()
        {
            
        }
    }
}