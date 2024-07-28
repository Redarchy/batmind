using System;
using Batmind.Tree.Nodes;
using UnityEngine;

namespace Batmind.Tree
{
    public class BehaviourTreeOwner : MonoBehaviour
    {
        [SerializeField] private BehaviourTree _Tree;
        [SerializeField] private bool _Run;
        
        public BehaviourTree Tree => _Tree;

        public bool IsRunning => _Run;

        private void Awake()
        {
            _Tree.Initialize();
        }

        private void Update()
        {
            if (_Run)
            {
                _Tree.Process();
            }
        }

        public void SetRunning(bool run)
        {
            _Run = run;
        }
        
        public void SetTree(BehaviourTree tree)
        {
            _Tree = tree;
        }
        
        public void ClearTree()
        {
            _Tree.Children.Clear();
        }
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            _Tree.OnValidate();
        }
#endif
    }
}