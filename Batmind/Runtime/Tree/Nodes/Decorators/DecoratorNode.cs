using UnityEngine;

namespace Batmind.Tree.Nodes.Decorators
{
    [System.Serializable]
    public class DecoratorNode : Node
    {
        [SerializeReference] public Node Child;
    }
}