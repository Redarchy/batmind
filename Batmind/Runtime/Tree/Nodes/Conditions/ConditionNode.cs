namespace Batmind.Tree.Nodes.Conditions
{
    [System.Serializable]
    public abstract class ConditionNode : Node
    {
        public override Status Process()
        {
            var isSatisfied = IsConditionSatisfied();
            
            return isSatisfied ? Status.Success : Status.Failure;
        }

        protected virtual bool IsConditionSatisfied()
        {
            return true;
        }
    }
}