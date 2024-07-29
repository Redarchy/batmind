using Batmind.Tree.Nodes;
using UnityEngine;

namespace Batmind.Tree
{
    [System.Serializable]
    public class BehaviourContext
    {
        
    }
    
    public class BehaviourTreeOwner : MonoBehaviour
    {
        [SerializeField] private BehaviourTree _Tree;
        [SerializeField] private bool _Run;

        [SerializeReference] private BehaviourContext _context;
        
        public BehaviourTree Tree => _Tree;

        public bool IsRunning => _Run;
        public BehaviourContext Context => _context;

        protected virtual void Awake()
        {
            _Tree.Initialize();
        }

        public void SetBehaviourContext(BehaviourContext context)
        {
            _context = context;
            
            _Tree.SetBehaviourContext(context);
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
            if (_Tree != null)
            {
                _Tree.OnValidate();
            }
        }
#endif
    }
}