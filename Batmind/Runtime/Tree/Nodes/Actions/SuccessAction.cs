using System;

namespace Batmind.Tree.Nodes.Actions
{
    [Serializable]
    public class SuccessAction : ActionNode
    {
        public override Status Process()
        {
            return Status.Success;
        }
    }
}