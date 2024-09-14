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
                        _currentChild = 0;
                        return Status.Success;
                    default:
                        _currentChild++;
                        return Status.Running;
                }
            }
            
            _currentChild = 0;
            
            return Status.Failure;
        }
    }
}