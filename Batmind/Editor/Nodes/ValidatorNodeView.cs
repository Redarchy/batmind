using System;
using Batmind.Tree.Nodes.Composites;
using UnityEditor.Experimental.GraphView;

namespace Batmind.Editor.Nodes
{
    public class ValidatorNodeView : NodeView<Validator>
    {
        protected override Port.Capacity OutputPortCapacity => Port.Capacity.Multi;

        public ValidatorNodeView(Validator validator, Action<Tree.Nodes.Node> onSelectionChanged) 
            : base(validator, onSelectionChanged)
        {
            
        }

        protected override void SetStyle()
        {
            
        }
    }
}