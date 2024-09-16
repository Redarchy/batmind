using System;

namespace Batmind.Tree.Nodes.Composites
{
    [Serializable]
    public class Selector : Composite
    {
        public override Status Process()
        {
            var result = Status.Failure;

            for (var i = _currentChild; i < Children.Count; i++)
            {
                _currentChild = i;
                
                result = Children[_currentChild].Process();

                switch (result)
                {
                    case Status.Success:
                        Reset();
                        return result;
                    
                    case Status.Failure:
                        break;
                    
                    case Status.Running:
                        return result;
                }

                _currentChild++;
            }

            Reset();
            
            return result;
        }
    }
}