using System;
using Batmind.Tree.Nodes.Decorators;
using UnityEditor.Experimental.GraphView;

namespace Batmind.Editor.Nodes
{
    public class InverterNodeView : NodeView<Inverter>
    {
        protected override Port.Capacity OutputPortCapacity => Port.Capacity.Single;

        public InverterNodeView(Inverter inverter, Action<Tree.Nodes.Node> onSelectionChanged) 
            : base(inverter, onSelectionChanged)
        {
            
        }

        protected override void SetStyle()
        {
            
        }
    }
}