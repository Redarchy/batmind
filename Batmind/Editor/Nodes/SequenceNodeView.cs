using Batmind.Tree.Nodes.Composites;
using UnityEditor.Experimental.GraphView;

namespace Batmind.Editor.Nodes
{
    public class SequenceNodeView : NodeView<Sequence>
    {
        protected override Port.Capacity OutputPortCapacity => Port.Capacity.Multi;

        
        public SequenceNodeView(Sequence sequence) : base(sequence)
        {
            
        }

        protected override void SetStyle()
        {
            
        }
    }
}