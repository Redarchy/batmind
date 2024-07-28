using Batmind.Tree.Nodes;
using UnityEngine;

namespace Batmind.Tree
{
    public class BehaviourTreeContainer : ScriptableObject
    {
        [SerializeField, HideInInspector] private BehaviourTree _Tree;

        public BehaviourTree Tree => _Tree;
        
        public void Set(BehaviourTree tree)
        {
            _Tree = tree;
        }
    }
}