using System.Collections.Generic;
using System.Reflection;
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
            SetRuntimeEntryAccessors(Root.Children);
        }
        
        public override void SetBehaviourContext(BehaviourContext context)
        {
            base.SetBehaviourContext(context);
            Validator.SetBehaviourContext(context);
            SetBehaviourContext(context, Root.Children);
        }

        private void SetBehaviourContext(BehaviourContext context, List<Node> nodes)
        {
            foreach (var node in nodes)
            {
                if (node is Composite composite)
                {
                    SetBehaviourContext(context, composite.Children);
                    continue;
                }

                node.SetBehaviourContext(context);
            }
        }

        private void SetRuntimeEntryAccessors(List<Node> nodes)
        {
            const string entryKeyHashName = "EntryKeyHash";
            const string runtimeEntryName = "RuntimeEntry";
            
            var entryAccessorType = typeof(EntryAccessor<>);

            foreach (var node in nodes)
            {
                if (node is Composite composite)
                {
                    SetRuntimeEntryAccessors(composite.Children);
                    continue;
                }
                
                var childType = node.GetType();
                var fields = childType.GetFields(BindingFlags.Public | BindingFlags.Instance);

                foreach (var fieldInfo in fields)
                {
                    if (!fieldInfo.FieldType.IsGenericType || fieldInfo.FieldType.GetGenericTypeDefinition() != entryAccessorType)
                    {
                        continue;
                    }

                    var entryAccessor = fieldInfo.GetValue(node);
                    var entryAccessorExplicitType = entryAccessor.GetType();
                    var entryKeyHashField = entryAccessorExplicitType.GetField(entryKeyHashName);
                    var entryKeyHash = (int) entryKeyHashField.GetValue(entryAccessor);
                    
                    if (!Blackboard.ContainsKeyWithHash(entryKeyHash, out var key))
                    {
                        continue;
                    }

                    if (!Blackboard.TryGetEntry(key, out var entry))
                    {
                        continue;
                    }
                    
                    var runtimeEntryField = entryAccessorExplicitType.GetField(runtimeEntryName);
                    runtimeEntryField.SetValue(entryAccessor, entry);
                }

            }
        }
        
        public void AddChild(Node child) => Root.Children.Add(child);

        public override Status Process()
        {
            // var status = Children[_currentChild].Process();
            //
            // _currentChild = (_currentChild + 1) % Children.Count;
            //
            // return status;

            while (_currentChild < Root.Children.Count)
            {
                var status = Root.Children[_currentChild].Process();

                if (status != Status.Success)
                {
                    return status;
                }

                _currentChild++;
            }

            return Status.Success;
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
            foreach (var child in Root.Children)
            {
                child.Reset();
            }
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            Blackboard.OnValidate();
        }
#endif
    }
}