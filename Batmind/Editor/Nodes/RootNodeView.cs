using System;
using System.Linq;
using Batmind.Editor;
using Batmind.Tree.Nodes.Composites;
using Batmind.Utils;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Batmind.Batmind.Editor.Nodes
{
    public class RootNodeView : NodeView<Root>
    {
        private readonly Action<Tree.Nodes.Node> _onSelectionChanged;
        protected override Port.Capacity OutputPortCapacity => Port.Capacity.Single;
        protected override Color BackgroundColor => ColorExtensions.FromInt(160, 32, 240);
        protected override string DefaultDescription => "of all evil...";
        public override string Title => "The Root";


        public RootNodeView(Root root, Action<NodeView> onSelectionChanged) 
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

        public override void OrderChildren()
        {
            if (OutputPort.connections == null || !OutputPort.connections.Any())
            {
                return;
            }
            
            var childView = OutputPort.connections.Select(c => c.input.node as NodeView).FirstOrDefault();

            if (childView == null || childView.ImplicitTreeNode is not Composite)
            {
                return;
            }
            
            var ownRect = GetPosition();
            var center = ownRect.center;
            var position = ownRect.position;
            var ownBottomY = position.y + ownRect.height;

            var rect = childView.GetPosition();
            rect.x = center.x - rect.width / 2f;
            rect.y = ownBottomY + 40f;
            childView.SetPosition(rect);

            childView.OrderChildren();
        }

    }
}