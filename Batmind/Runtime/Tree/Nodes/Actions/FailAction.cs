using System;

namespace Batmind.Tree.Nodes.Actions
{
    [Serializable]
    public class FailAction : ActionNode
    {
        public override Status Process()
        {
            return Status.Failure;
        }
    }
}