﻿using System.Collections.Generic;
using UnityEngine;

namespace Batmind.Tree.Nodes.Composites
{
    [System.Serializable]
    public abstract class Composite : Node
    {
        [SerializeReference] public List<Node> Children;
        
        protected int _currentChild = 0;
        
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
    }
}