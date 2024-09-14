using UnityEngine;

namespace Batmind.Tree.Nodes.Actions
{
    [System.Serializable]
    public class WaitForSeconds : ActionNode
    {
        [SerializeField] private float _Duration;
        
        private float _passedDuration = 0;
        
        public override Status Process()
        {
            if (_passedDuration >= _Duration)
            {
                _passedDuration = 0f;
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