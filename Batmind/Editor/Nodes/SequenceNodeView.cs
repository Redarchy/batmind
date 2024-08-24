using System;
using Batmind.Tree.Nodes.Composites;
using UnityEditor.Experimental.GraphView;

namespace Batmind.Editor.Nodes
{
    public class SequenceNodeView : NodeView<Sequence>
    {
        protected override Port.Capacity OutputPortCapacity => Port.Capacity.Multi;
        protected override string DefaultDescription => "Until One Fails, T2B";

        
        public SequenceNodeView(Sequence sequence, Action<Tree.Nodes.Node> onSelectedNodeChanged) 
            : base(sequence, onSelectedNodeChanged)
        {
            
        }

        protected override void SetStyle()
        {
            
        }
    }
}