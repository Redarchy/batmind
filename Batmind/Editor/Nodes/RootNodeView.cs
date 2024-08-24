using System;
using Batmind.Editor;
using Batmind.Tree.Nodes.Composites;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Batmind.Batmind.Editor.Nodes
{
    public class RootNodeView : NodeView<Root>
    {
        private readonly Action<Tree.Nodes.Node> _onSelectionChanged;
        protected override Port.Capacity OutputPortCapacity => Port.Capacity.Single;
        protected override Color BackgroundColor => new(160 / 255f, 32 / 255f, 240 / 255f, 1);
        protected override string DefaultDescription => "of all evil...";
        public override string Title => "The Root";


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