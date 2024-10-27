using System;

namespace Batmind.Tree.Nodes.Decorators
{
    [Serializable]
    public class ForceSuccessDecorator : DecoratorNode
    {
        public override Status Process()
        {
            Child.Process();
            
            return Status.Success;
        }
    }
}