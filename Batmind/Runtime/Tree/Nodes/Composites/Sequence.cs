namespace Batmind.Tree.Nodes.Composites
{
    [System.Serializable]
    public class Sequence : Composite
    {
        public override Status Process()
        {
            if (_currentChild < Children.Count)
            {
                switch (Children[_currentChild].Process())
                {
                    case Status.Running:
                        return Status.Running;
                    case Status.Failure:
                        _currentChild = 0;
                        return Status.Failure;
                    default:
                        _currentChild++;
                        return _currentChild == Children.Count ? Status.Success : Status.Running;
                }
            }

            Reset();
            return Status.Success;
        }
    }
}