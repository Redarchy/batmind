using Batmind.Tree.Nodes.Composites;
using UnityEditor.Experimental.GraphView;

namespace Batmind.Editor.Nodes
{
    public class RandomOrderSelectorNodeView : NodeView<RandomOrderSelector>
    {
        protected override Port.Capacity OutputPortCapacity => Port.Capacity.Multi;

        public RandomOrderSelectorNodeView(RandomOrderSelector randomOrderSelector) : base(randomOrderSelector)
        {
            
        }

        protected override void SetStyle()
        {
            
        }
    }
}