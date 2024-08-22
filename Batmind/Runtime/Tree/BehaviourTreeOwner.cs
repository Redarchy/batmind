using Batmind.Tree.Nodes;
using UnityEngine;

namespace Batmind.Tree
{
    public class BehaviourTreeOwner : MonoBehaviour
    {
        [SerializeField] private BehaviourTree _Tree;
        [SerializeField] private bool _Run;
        [SerializeField] private bool _Validate;

        [SerializeReference] private BehaviourContext _context;
        
        public BehaviourTree Tree => _Tree;

        public bool IsRunning => _Run;
        public bool IsValidating => _Validate;
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
                if (_Validate)
                {
                    _Tree.Validate();
                }

                _Tree.Process();
            }
        }

        public void SetRunning(bool run)
        {
            _Run = run;
        }

        public void SetValidation(bool validate)
        {
            _Validate = validate;
        }
        
        public void SetTree(BehaviourTree tree)
        {
            _Tree = tree;
        }
        
        public void ClearTree()
        {
            _Tree.Clear();
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