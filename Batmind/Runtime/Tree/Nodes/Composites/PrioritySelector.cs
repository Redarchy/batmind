using System.Collections.Generic;
using System.Linq;

namespace Batmind.Tree.Nodes.Composites
{
    [System.Serializable]
    public class PrioritySelector : Composite
    {
        private List<Node> _sortedChildren;
        private List<Node> SortedChildren => _sortedChildren ??= SortChildren();
        
        protected virtual List<Node> SortChildren() => Children.OrderByDescending(child => child.Priority).ToList();
        
        public override void Reset()
        {
            base.Reset();
            _sortedChildren = null;
        }
        
        public override Status Process()
        {
            foreach (var child in SortedChildren)
            {
                switch (child.Process())
                {
                    case Status.Running:
                        return Status.Running;
                    case Status.Success:
                        Reset();
                        return Status.Success;
                    default:
                        continue;
                }
            }

            Reset();
            return Status.Failure;
        }
    }
}