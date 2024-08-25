using System;
using Batmind.Tree.Nodes.Composites;
using UnityEditor.Experimental.GraphView;

namespace Batmind.Editor.Nodes
{
    public class SequenceNodeView : NodeView<Sequence>
    {
        protected override Port.Capacity OutputPortCapacity => Port.Capacity.Multi;
        protected override string DefaultDescription => "Until One Fails";

        
        public SequenceNodeView(Sequence sequence, Action<NodeView> onSelectedNodeChanged) 
            : base(sequence, onSelectedNodeChanged)
        {
            (ImplicitTreeNode as Composite)!._EditorOrderMode = Composite.CompositeOrderMode.TopToBottom;
        }

        protected override void SetStyle()
        {
            AddOrderModeLabel();
        }

    }
}