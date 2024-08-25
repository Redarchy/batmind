using System;
using Batmind.Tree.Nodes.Composites;
using Batmind.Utils;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Batmind.Editor.Nodes
{
    public class ValidatorNodeView : NodeView<Validator>
    {
        protected override Port.Capacity OutputPortCapacity => Port.Capacity.Multi;
        protected override Color BackgroundColor => ColorExtensions.FromInt(25, 125, 75);
        protected override string DefaultDescription => "Reset If Any Fails";

        public ValidatorNodeView(Validator validator, Action<NodeView> onSelectionChanged) 
            : base(validator, onSelectionChanged)
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
            this.capabilities |= Capabilities.Movable;
            this.capabilities |= Capabilities.Selectable;

            var rect = GetPosition();
            var position = rect.position;
            
            if (position.y == 0)
            {
                position.y -= 200;
            }

            rect.position = position;
            SetPosition(rect);
        }
    }
}