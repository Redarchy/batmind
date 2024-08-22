using System.Collections.Generic;
using System.Linq;
using UnityEngine.Pool;

namespace Batmind.Tree.Nodes.Composites
{
    [System.Serializable]
    public class PrioritySelector : Composite
    {
        private List<Node> _sortedChildren;
        private List<Node> SortedChildren => _sortedChildren ??= SortChildren();
        
        protected virtual List<Node> SortChildren()
        {
            _sortedChildren = ListPool<Node>.Get();
            _sortedChildren.Clear();
            _sortedChildren.AddRange(Children.OrderByDescending(child => child.Priority));

            return _sortedChildren;
        }
        
        public override void Reset()
        {
            base.Reset();
            
            if (_sortedChildren != null)
            {
                _sortedChildren.Clear();
                ListPool<Node>.Release(_sortedChildren);
                _sortedChildren = null;
            }
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