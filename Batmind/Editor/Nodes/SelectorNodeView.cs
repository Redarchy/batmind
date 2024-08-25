using System;
using Batmind.Tree.Nodes.Composites;
using Batmind.Utils;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Batmind.Editor.Nodes
{
    public class SelectorNodeView : NodeView<Selector>
    {
        protected override Port.Capacity OutputPortCapacity => Port.Capacity.Multi;
        protected override Color BackgroundColor => ColorExtensions.FromInt(239, 124, 142);
        protected override string DefaultDescription => "Until One Succeeds";

        public SelectorNodeView(Selector selector, Action<NodeView> onSelectionChanged) 
            : base(selector, onSelectionChanged)
        {
            (ImplicitTreeNode as Composite)!._EditorOrderMode = Composite.CompositeOrderMode.LeftToRight;
        }

        protected override void SetStyle()
        {
            AddOrderModeLabel();
        }
    }
}