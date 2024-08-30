#define BATMIND_HIDE_COMPOSITE_CHILDREN

using UnityEngine;

namespace Batmind.Tree.Nodes.Decorators
{
    [System.Serializable]
    public class DecoratorNode : Node
    {
#if !BATMIND_HIDE_COMPOSITE_CHILDREN
        [HideInInspector]
#endif
        [SerializeReference]
        public Node Child;

        
        public override void Initialize()
        {
            base.Initialize();
            Child.Initialize();
        }

        public override void SetBehaviourContext(BehaviourContext context)
        {
            base.SetBehaviourContext(context);
            Child.SetBehaviourContext(context);
        }

        public override void Reset()
        {
            Child.Reset();
        }

        public override void Clear()
        {
            base.Clear();
            
            Child?.Clear();
            Child = null;
        }
    }
}