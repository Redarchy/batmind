using Batmind.Board;
using UnityEngine;

namespace Batmind.Tree.Nodes.Actions
{
    [System.Serializable]
    public class WaitForSecondsEntryAccessor : ActionNode
    {
        [SerializeField] public EntryAccessor<float> _DurationAccessor;
        
        private float _passedDuration = 0;
        
        public override Status Process()
        {
            if (_passedDuration >= _DurationAccessor.Value.FloatValue)
            {
                return Status.Success;
            }

            _passedDuration += Time.deltaTime;

            return Status.Running;
        }

        public override void Reset()
        {
            base.Reset();
            
            _passedDuration = 0f;
        }

    }
}