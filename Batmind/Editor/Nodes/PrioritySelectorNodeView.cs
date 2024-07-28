using Batmind.Tree.Nodes.Composites;
using UnityEditor.Experimental.GraphView;

namespace Batmind.Editor.Nodes
{
    public class PrioritySelectorNodeView : NodeView<PrioritySelector>
    {
        protected override Port.Capacity OutputPortCapacity => Port.Capacity.Multi;

        public PrioritySelectorNodeView(PrioritySelector prioritySelector) : base(prioritySelector)
        {
            
        }

        protected override void SetStyle()
        {
            
        }
    }
}