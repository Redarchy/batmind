using System;

namespace Batmind.Tree.Nodes.Composites
{
    [Serializable]
    public class Sequence : Composite
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
                        break;
                    
                    case Status.Failure:
                        Reset();
                        return result;
                    
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