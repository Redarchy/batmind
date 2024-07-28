namespace Batmind.Tree.Nodes.Decorators
{
    [System.Serializable]
    public class Inverter : DecoratorNode
    {
        public override Status Process()
        {
            switch (Child.Process())
            {
                case Status.Running:
                    return Status.Running;
                case Status.Failure:
                    return Status.Success;
                case Status.Success:
                    return Status.Failure;
                default:
                    return Status.Failure;
            }
        }
    }
}