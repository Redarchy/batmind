using Batmind.Board;
using Batmind.Tree.Nodes.Composites;
using UnityEngine;

namespace Batmind.Tree.Nodes
{
    [System.Serializable]
    public class BehaviourTree : Node
    {
        [SerializeField] public Validator Validator;
        [SerializeField] public Root Root;
        [SerializeField] public Blackboard Blackboard;
        
        protected int _currentChild = 0;

        public override void Initialize()
        {
            Root.SetRuntimeEntryAccessors(Blackboard);
        }
        
        public override void SetBehaviourContext(BehaviourContext context)
        {
            base.SetBehaviourContext(context);
            Validator.SetBehaviourContext(context);
            Root.SetBehaviourContext(context);
        }
        
        public override Status Process()
        {
            // var status = Children[_currentChild].Process();
            //
            // _currentChild = (_currentChild + 1) % Children.Count;
            //
            // return status;
            //
            // while (_currentChild < Root.Children.Count)
            // {
            //     var status = Root.Children[_currentChild].Process();
            //
            //     if (status != Status.Success)
            //     {
            //         return status;
            //     }
            //
            //     _currentChild++;
            // }

            return Root.Process();
        }

        public void Validate()
        {
            if (Validator.Process() == Status.Failure)
            {
                Reset();
            }
        }

        public override void Reset()
        {
            Root.Reset();
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            Blackboard.OnValidate();
        }
#endif
        public void Clear()
        {
            Blackboard.ClearEntries();
            Root.Clear();
            Validator.Clear();
        }
    }
}