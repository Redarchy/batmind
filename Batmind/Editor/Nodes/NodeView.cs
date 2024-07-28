using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
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


        protected void SetBaseView(Tree.Nodes.Node treeNode)
        {
            ImplicitTreeNode = treeNode;
            
            AddInputPort();
            AddOutputPort();
            
            RefreshExpandedState();
            RefreshPorts();
            
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

            inputContainer.Add(inputPort);

            _inputPort = inputPort;
        }

        protected virtual void AddOutputPort()
        {
            var outputPort = GetPortInstance(Direction.Output, OutputPortCapacity);
            outputPort.capabilities &= ~Capabilities.Deletable;
            outputPort.portName = Constants.OutputPortLabel;

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
        }
        
        public void ConnectInputTo(Edge edge)
        {
            edge.input = _inputPort;
            _inputPort.Connect(edge);
            RefreshPorts();
        }

    }
    
    public class NodeView<TTreeNode> : NodeView
        where TTreeNode : Tree.Nodes.Node
    {
        public override string Title => TreeNode.GetType().Name;
        
        public TTreeNode TreeNode { get; private set; }

        public NodeView(Tree.Nodes.Node treeNode)
        {
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
    }

}