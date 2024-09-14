using UnityEngine;

namespace Batmind.Tree.Nodes.Decorators
{
    [System.Serializable]
    public class RunningDecorator : DecoratorNode
    {
        [SerializeField] private Status _DecoratedValue = Status.Running;

        public override Status Process()
        {
            switch (Child.Process())
            {
                case Status.Running:
                    return _DecoratedValue;
                case Status.Failure:
                    return Status.Failure;
                case Status.Success:
                    return Status.Success;
                default:
                    return Status.Success;
            }
        }
    }
}