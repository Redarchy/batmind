namespace Batmind.Tree.Nodes.Composites
{
    [System.Serializable]
    public class Validator : Composite 
    {
        public override Status Process()
        {
            for (_currentChild = 0; _currentChild < Children.Count; _currentChild++)
            {
                switch (Children[_currentChild].Process())
                {
                    case Status.Failure:
                        Reset();
                        return Status.Failure;
                    default:
                        continue;
                }
            }
            
            Reset();
            
            return Status.Success;
        }

        public void Clear()
        {
            Children.Clear();
        }
    }
}