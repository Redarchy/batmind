using System.Collections.Generic;

namespace Batmind.Tree.Nodes.Composites
{
    [System.Serializable]
    public class Selector : Composite
    {
        public override Status Process()
        {
            if (_currentChild < Children.Count)
            {
                switch (Children[_currentChild].Process())
                {
                    case Status.Running:
                        return Status.Running;
                    case Status.Success:
                        Reset();
                        return Status.Success;
                    default:
                        _currentChild++;
                        return Status.Running;
                }
            }
            
            Reset();
            
            return Status.Failure;
        }
    }
}