using System.Collections.Generic;
using System.Reflection;
using Batmind.Board;
using UnityEngine;

namespace Batmind.Tree.Nodes.Composites
{
    [System.Serializable]
    public class Root : Node
    {
        [SerializeReference] public Node _Child;
        
        public override void Initialize()
        {
            base.Initialize();
            _Child.Initialize();
        }

        public override Status Process()
        {
            return _Child.Process();
        }

        public override void Reset()
        {
            base.Reset();
            
            _Child.Reset();
        }

        #region Setting Behaviour Context

        public override void SetBehaviourContext(BehaviourContext context)
        {
            base.SetBehaviourContext(context);
            _Child.SetBehaviourContext(context);
        }
        
        #endregion
        
        #region Setting Runtime Entry Accessors

        public void SetRuntimeEntryAccessors(Blackboard blackboard)
        {
            SetRuntimeEntryAccessors(_Child, blackboard);
        }
        
        private void SetRuntimeEntryAccessors(List<Node> nodes, Blackboard blackboard)
        {
            foreach (var node in nodes)
            {
                SetRuntimeEntryAccessors(node, blackboard);
            }
        }
        
        private void SetRuntimeEntryAccessors(Node node, Blackboard blackboard)
        {
            const string entryKeyHashName = "EntryKeyHash";
            const string runtimeEntryName = "RuntimeEntry";
            
            var entryAccessorType = typeof(EntryAccessor<>);
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
                    
                if (!blackboard.ContainsKeyWithHash(entryKeyHash, out var key))
                {
                    continue;
                }

                if (!blackboard.TryGetEntry(key, out var entry))
                {
                    continue;
                }
                    
                var runtimeEntryField = entryAccessorExplicitType.GetField(runtimeEntryName);
                runtimeEntryField.SetValue(entryAccessor, entry);
            }

            if (node is Composite composite)
            {
                SetRuntimeEntryAccessors(composite.Children, blackboard);
            }
        }

        #endregion

        public void Clear()
        {
            if (_Child != null)
            {
                _Child.Clear();
                _Child = null;
            }
        }
    }
}