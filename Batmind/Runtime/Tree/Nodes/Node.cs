using System;
using UnityEngine;

namespace Batmind.Tree.Nodes
{
    [Serializable]
    public class Node
    {
        public int Priority;
        protected BehaviourContext _context;

        public Node(int priority = 0)
        {
            Priority = priority;
        }
        
        public virtual void Initialize() { }
        
        public virtual void SetBehaviourContext(BehaviourContext context)
        {
            _context = context;
        }

        public virtual Status Process() => Status.Success;

        public virtual void Reset() { }
        
#if UNITY_EDITOR
        [SerializeField, HideInInspector] public Vector2 GraphPosition;
        [SerializeField] public string Description = "";
#endif
        
        public enum Status { Success, Failure, Running }

        public virtual void Clear()
        {
            
        }
    }
}