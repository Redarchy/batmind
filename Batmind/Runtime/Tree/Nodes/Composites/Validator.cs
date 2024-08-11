using System.Collections.Generic;

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
        
        public override void SetBehaviourContext(BehaviourContext context)
        {
            _context = context;
            SetBehaviourContext(context, Children);            
        }

        private void SetBehaviourContext(BehaviourContext context, List<Node> nodes)
        {
            foreach (var node in nodes)
            {
                if (node is Composite composite)
                {
                    SetBehaviourContext(context, composite.Children);
                    continue;
                }

                node.SetBehaviourContext(context);
            }
        }

    }
}