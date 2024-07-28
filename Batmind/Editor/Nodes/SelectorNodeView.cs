using Batmind.Tree.Nodes.Composites;
using UnityEditor.Experimental.GraphView;

namespace Batmind.Editor.Nodes
{
    public class SelectorNodeView : NodeView<Selector>
    {
        protected override Port.Capacity OutputPortCapacity => Port.Capacity.Multi;

        public SelectorNodeView(Selector selector) : base(selector)
        {
            
        }

        protected override void SetStyle()
        {
            
        }
    }
}