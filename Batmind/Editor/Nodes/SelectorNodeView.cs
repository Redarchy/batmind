using System;
using Batmind.Tree.Nodes.Composites;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Batmind.Editor.Nodes
{
    public class SelectorNodeView : NodeView<Selector>
    {
        protected override Port.Capacity OutputPortCapacity => Port.Capacity.Multi;
        protected override Color BackgroundColor => new(239 / 255f, 124 / 255f, 142 / 255f, 1);
        protected override string DefaultDescription => "Until One Succeeds, L2R";

        public SelectorNodeView(Selector selector, Action<Tree.Nodes.Node> onSelectionChanged) 
            : base(selector, onSelectionChanged)
        {
        }

        protected override void SetStyle()
        {
            
        }
    }
}