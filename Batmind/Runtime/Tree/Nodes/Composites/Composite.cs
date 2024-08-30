#define BATMIND_HIDE_COMPOSITE_CHILDREN

using System.Collections.Generic;
using UnityEngine;

namespace Batmind.Tree.Nodes.Composites
{
    [System.Serializable]
    public abstract class Composite : Node
    {
        #if BATMIND_HIDE_COMPOSITE_CHILDREN
        [HideInInspector]
        #endif
        [SerializeReference] public List<Node> Children;
        
        protected int _currentChild = 0;

        public override void Initialize()
        {
            base.Initialize();
            foreach (var child in Children)
            {
                child.Initialize();
            }
        }

        public void AddChild(Node child) => Children.Add(child);
        
        public override Status Process() => Children[_currentChild].Process();

        public override void Reset()
        {
            _currentChild = 0;
            
            foreach (var child in Children)
            {
                child.Reset();
            }
        }

        public override void SetBehaviourContext(BehaviourContext context)
        {
            base.SetBehaviourContext(context);
            
            foreach (var child in Children)
            {
                child.SetBehaviourContext(context);
            }

        }

        public override void Clear()
        {
            base.Clear();
            Children.Clear();
        }
        
        #if UNITY_EDITOR

        [SerializeField] public CompositeOrderMode _EditorOrderMode;
        
        public enum CompositeOrderMode
        {
            Priority,
            LeftToRight,
            TopToBottom,
        }
        
        #endif
    }
}