using System;
using System.Collections;
using System.Linq;
using Batmind.Editor;
using Batmind.Tree.Nodes.Decorators;
using Batmind.Utils;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Node = Batmind.Tree.Nodes.Node;

namespace Plugins.Batmind.Batmind.Editor.Nodes
{
    public class DecoratorNodeView  : NodeView<DecoratorNode>
    {
        protected override Port.Capacity OutputPortCapacity => Port.Capacity.Single;
        private readonly Action<Node> _onSelectionChanged;
        public Type ExplicitNodeType { get; set; }
        protected override Color BackgroundColor => ColorExtensions.FromInt(182, 226,  150);

        public DecoratorNodeView(DecoratorNode decoratorNode, Action<NodeView> onSelectionChanged) 
            : base(decoratorNode, onSelectionChanged)
        {
            
        }

        public override void OnOutputConnectedTo(NodeView inputNodeView)
        {
            base.OnOutputConnectedTo(inputNodeView);
            EditorCoroutineUtility.StartCoroutine(OrderChildren(), this);
        }

        public override IEnumerator OrderChildren()
        {
            var ownRect = GetPosition();
            var center = ownRect.center;
            var position = ownRect.position;

            var connectedViews = this._outputPort.connections.Select(e => e.input.node as NodeView).ToList();

            if (connectedViews.Count <= 0)
            {
                yield break;
            }

            var contents = this.Q("contents");
            
            foreach (var connectedView in connectedViews)
            {
                contents.Add(connectedView);
                var childRect = connectedView.GetPosition();
                childRect.position = Vector2.zero;
                connectedView.SetPosition(childRect);
                
                // To make the inverter wrap the connected node
                connectedView.style.position = Position.Relative;
            }
            
            yield return null;
            
            foreach (var connectedView in connectedViews)
            {
                yield return connectedView.OrderChildren();
                yield return null;
            }
        }
    }
}