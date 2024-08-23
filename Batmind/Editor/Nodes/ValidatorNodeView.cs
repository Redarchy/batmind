using System;
using Batmind.Tree.Nodes.Composites;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Batmind.Editor.Nodes
{
    public class ValidatorNodeView : NodeView<Validator>
    {
        protected override Port.Capacity OutputPortCapacity => Port.Capacity.Multi;
        protected override Color BackgroundColor => new (25 / 255f, 125 /255f, 75 / 255f, 1);
        protected override string DefaultDescription => "Reset If Any Fails";

        public ValidatorNodeView(Validator validator, Action<Tree.Nodes.Node> onSelectionChanged) 
            : base(validator, onSelectionChanged)
        {
            
        }

        protected override void AddInputPort()
        {
            _inputPort = null;
        }

        protected override void SetStyle()
        {
            
        }
    }
}