using System;
using Batmind.Editor;
using Batmind.Tree.Nodes.Composites;
using UnityEditor.Experimental.GraphView;

namespace Batmind.Batmind.Editor.Nodes
{
    public class RootNodeView : NodeView<Root>
    {
        private readonly Action<Tree.Nodes.Node> _onSelectionChanged;
        protected override Port.Capacity OutputPortCapacity => Port.Capacity.Multi;

        public RootNodeView(Root root, Action<Tree.Nodes.Node> onSelectionChanged) 
            : base(root, onSelectionChanged)
        {
            
        }

        protected override void AddInputPort()
        {
            _inputPort = null;
        }

        protected override void SetStyle()
        {
            this.capabilities &= Capabilities.Deletable;
            this.capabilities &= Capabilities.Collapsible;
            this.capabilities &= Capabilities.Resizable;
            this.capabilities &= Capabilities.Copiable;
        }
    }
}