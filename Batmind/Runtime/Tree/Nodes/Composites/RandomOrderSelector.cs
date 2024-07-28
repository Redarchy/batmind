using System.Collections.Generic;
using Batmind.Utils;

namespace Batmind.Tree.Nodes.Composites
{
    [System.Serializable]
    public class RandomOrderSelector : Composite
    {
        private List<Node> _sortedChildren;
        private List<Node> SortedChildren => _sortedChildren ??= SortChildren();
        
        protected List<Node> SortChildren() => ShuffleClone();
        
        private List<Node> ShuffleClone()
        {
            var clone = new List<Node>(Children);
            clone.Shuffle();
            return clone;
        }

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