using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities.Extentions;
using Edge = UnityEditor.Experimental.GraphView.Edge;

namespace Batmind.Editor
{
    public abstract class NodeView : Node
    {
        protected Port _inputPort;
        protected Port _outputPort;
        protected List<Edge> _edges = new();

        public Port InputPort => _inputPort;
        public Port OutputPort => _outputPort;
        public abstract string Title { get; }
        protected virtual Port.Capacity OutputPortCapacity => Port.Capacity.Single;
        public Tree.Nodes.Node ImplicitTreeNode { get; protected set; }
        public List<Edge> Edges => _edges;
        protected Action<Tree.Nodes.Node> OnSelectionChanged { get; set; }


        protected void SetBaseView(Tree.Nodes.Node treeNode)
        {
            ImplicitTreeNode = treeNode;
            
            AddInputPort();
            AddOutputPort();
            
            RefreshExpandedState();
            RefreshPorts();
            SetPortStyle();
            
            AddTitleLabel();
            SetStyle();
        }

        protected abstract void SetStyle();

        private void AddTitleLabel()
        {
            title = Title;
        }
        
        protected virtual void AddInputPort()
        {
            var inputPort = GetPortInstance(Direction.Input, Port.Capacity.Single);
            inputPort.capabilities &= ~Capabilities.Deletable;
            inputPort.portName = Constants.InputPortLabel;
            inputPort.style.flexDirection = FlexDirection.Column;
            inputPort.style.height = 5f;
            inputPort.style.color = Color.cyan;

            var cap = inputPort.Q("cap");
            cap.style.paddingTop = 8f;
            cap.style.width = 10f;
            
            inputContainer.Add(inputPort);

            _inputPort = inputPort;
        }

        protected virtual void AddOutputPort()
        {
            var outputPort = GetPortInstance(Direction.Output, OutputPortCapacity);
            outputPort.capabilities &= ~Capabilities.Deletable;
            outputPort.portName = Constants.OutputPortLabel;
            outputPort.style.flexDirection = FlexDirection.Column;
            outputPort.style.height = 5f;
            outputPort.style.color = Color.cyan;

            var cap = outputPort.Q("cap");
            cap.style.paddingTop = 7f;
            cap.style.width = 10f;

            outputContainer.Add(outputPort);
            
            _outputPort = outputPort;
        }

        private Port GetPortInstance(Direction nodeDirection, Port.Capacity capacity)
        {
            return InstantiatePort(Orientation.Vertical, nodeDirection, capacity, null);
        }
        
        public void ConnectOutputTo(Edge edge)
        {
            edge.output = _outputPort;
            _outputPort.Connect(edge);
            RefreshPorts();
            SetPortStyle();
        }
        
        public void ConnectInputTo(Edge edge)
        {
            edge.input = _inputPort;
            _inputPort.Connect(edge);
            RefreshPorts();
            SetPortStyle();
        }

        private void SetPortStyle()
        {
            var nodeBorderVisualElement = this.Q("node-border");
            
            var inputVisualElement = this.Q("input");
            if (inputVisualElement != null)
            {
                inputVisualElement.parent.Remove(inputVisualElement);
            }
            
            var outputVisualElement = this.Q("output");

            if (outputVisualElement != null)
            {
                outputVisualElement.parent.Remove(outputVisualElement);
            }

            var childrenOfParent = nodeBorderVisualElement.hierarchy.Children().ToList();
            nodeBorderVisualElement.Clear();

            if (inputVisualElement != null)
            {
                nodeBorderVisualElement.Add(inputVisualElement);
            }
            childrenOfParent.ForEach(element => nodeBorderVisualElement.Add(element));
            
            if (outputVisualElement != null)
            {
                nodeBorderVisualElement.Add(outputVisualElement);
            }
        }
    }
    
    public class NodeView<TTreeNode> : NodeView
        where TTreeNode : Tree.Nodes.Node
    {
        public override string Title => TreeNode.GetType().Name;
        
        public TTreeNode TreeNode { get; private set; }

        public NodeView(Tree.Nodes.Node treeNode, Action<Tree.Nodes.Node> onSelectionChanged)
        {
            OnSelectionChanged = onSelectionChanged;

            TreeNode = (TTreeNode) treeNode;
            SetBaseView(treeNode);
            SetPosition(new Rect(TreeNode.GraphPosition, Constants.DefaultNodeSize));
        }
        
        public sealed override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            TreeNode.GraphPosition = newPos.position;
        }
        
        protected override void SetStyle()
        {
            
        }
        
        public override void OnSelected()
        {
            base.OnSelected();
            
            OnSelectionChanged?.Invoke(TreeNode);
        }
    }

}