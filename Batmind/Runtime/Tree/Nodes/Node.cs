using System;
using UnityEngine;

namespace Batmind.Tree.Nodes
{
    [Serializable]
    public class Node
    {
        [SerializeField, HideInInspector] private int _id;
        
        public int Priority;
        public int Id => _id;
        
        public Node(int priority = 0)
        {
            _id = GetHashCode();
            Priority = priority;
        }
        
        public virtual void Initialize() { }
        
        public virtual Status Process() => Status.Success;

        public virtual void Reset() { }
        
#if UNITY_EDITOR
        [SerializeField, HideInInspector] public Vector2 GraphPosition;

#endif
        
        public enum Status { Success, Failure, Running }
    }
}