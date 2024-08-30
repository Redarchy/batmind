using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Batmind.Editor.Nodes;
using Batmind.Tree.Nodes.Composites;
using Batmind.Tree.Nodes.Decorators;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
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
        protected virtual Color BackgroundColor => Color.yellow;
        protected virtual string DefaultDescription => "";
        public Tree.Nodes.Node ImplicitTreeNode { get; protected set; }
        public List<Edge> Edges => _edges;
        protected Action<NodeView> OnSelectionChanged { get; set; }
        public override bool expanded
        {
            get => true;
            set { /* Do nothing to prevent changing this value */ }
        }

        protected void SetBaseView(Tree.Nodes.Node treeNode)
        {
            ImplicitTreeNode = treeNode;
            
            AddInputPort();
            AddOutputPort();
            SetTitleVisual();
            
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

        protected virtual void SetTitleVisual()
        {
            var titleParent = this.Q("title");
            titleParent.style.flexDirection = FlexDirection.Column;
            titleParent.style.paddingLeft = 6f;
            titleParent.style.paddingRight = 6f;
            titleParent.style.paddingTop = 6f;
            titleParent.style.paddingBottom = 6f;
            titleParent.style.fontSize = 0f;

            var titleLabel = this.Q("title-label");
            titleLabel.style.marginLeft = 0;
            titleLabel.style.marginRight = 0;
            titleLabel.style.marginTop = 0;
            titleLabel.style.marginBottom = 0;
            titleLabel.style.unityTextAlign = TextAnchor.UpperLeft;
            titleLabel.style.color = Color.white;
            
            var titleButtonContainer = titleParent.Q("title-button-container");
            titleParent.Remove(titleButtonContainer);

            var description = string.IsNullOrEmpty(ImplicitTreeNode.Description) ? DefaultDescription : ImplicitTreeNode.Description;
            var descriptionLabel = new Label(description);
            EditorApplication.update += () => descriptionLabel.text = string.IsNullOrEmpty(ImplicitTreeNode.Description) ? DefaultDescription : ImplicitTreeNode.Description;
            descriptionLabel.style.unityTextAlign = TextAnchor.LowerLeft;
            descriptionLabel.style.fontSize = 10f;
            descriptionLabel.focusable = true;
            titleParent.Add(descriptionLabel);
        }

        protected virtual void AddOrderModeLabel()
        {
            if (ImplicitTreeNode is not Composite composite)
            {
                return;
            }
            
            var titleParent = this.Q("title").parent;
            var orderModeLabel = new Label(GetOrderModeText());
            EditorApplication.update += () => orderModeLabel.text = GetOrderModeText();
            orderModeLabel.style.unityTextAlign = TextAnchor.LowerLeft;
            orderModeLabel.style.fontSize = 8f;
            orderModeLabel.style.color = Color.black;
            orderModeLabel.style.unityTextOutlineColor = Color.black;
            orderModeLabel.style.unityTextOutlineWidth = 0.7f;
            orderModeLabel.focusable = false;
            titleParent.Add(orderModeLabel);

            var currentPadding = titleParent.style.paddingBottom.value.value;
            currentPadding += orderModeLabel.style.height.value.value;
            titleParent.style.paddingBottom = currentPadding;

            string GetOrderModeText()
            {
                switch (composite._EditorOrderMode)
                {
                    case Composite.CompositeOrderMode.Priority:
                        return "Priority";
                        
                    case Composite.CompositeOrderMode.LeftToRight:
                        return "L2R";
                        
                    case Composite.CompositeOrderMode.TopToBottom:
                    default:
                        return "T2B";
                }
            }
        }
        
        protected virtual void AddInputPort()
        {
            var inputPort = GetPortInstance(Direction.Input, Port.Capacity.Single);
            inputPort.capabilities &= ~Capabilities.Deletable;
            inputPort.portName = Constants.InputPortLabel;
            inputPort.style.flexDirection = FlexDirection.Column;
            inputPort.style.height = 5f;
            inputPort.style.color = Color.cyan;
            inputPort.portColor = Color.white;

            var cap = inputPort.Q("cap");
            cap.style.paddingTop = 8f;
            cap.style.width = 10f;
            
            var connector = inputPort.Q("connector");
            connector.style.alignSelf = Align.Center;

            var portTypeName = inputPort.Q("type");
            portTypeName.style.width = 0;

            inputContainer.Add(inputPort);

            _inputPort = inputPort;
        }

        protected virtual void AddOutputPort()
        {
            var outputPort = GetPortInstance(Direction.Output, OutputPortCapacity);
            outputPort.capabilities &= ~Capabilities.Deletable;
            outputPort.portName = Constants.OutputPortLabel;
            outputPort.style.flexDirection = FlexDirection.Column;
            outputPort.style.alignSelf = Align.Stretch;
            outputPort.style.alignContent = Align.Stretch;
            outputPort.style.alignItems = Align.Stretch;
            outputPort.style.height = 5f;
            outputPort.style.color = Color.cyan;
            outputPort.portColor = Color.white;

            var cap = outputPort.Q("cap");
            cap.style.paddingTop = 7f;
            cap.style.width = 10f;

            var connector = outputPort.Q("connector");
            connector.style.alignSelf = Align.Center;

            var portTypeName = outputPort.Q("type");
            portTypeName.style.width = 0;
            
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
            nodeBorderVisualElement.style.overflow = Overflow.Visible;
            nodeBorderVisualElement.style.backgroundColor = BackgroundColor;
            
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

        public virtual IEnumerator OrderChildren()
        {
            if (ImplicitTreeNode is not Composite composite)
            {
                yield break;
            }

            var ownRect = GetPosition();
            var center = ownRect.center;
            var position = ownRect.position;
            var ownBottomY = position.y + ownRect.height;

            var connectedViews = this.GetOrderedEdges().Select(e => e.input.node as NodeView).ToList();

            if (connectedViews.Count <= 0)
            {
                yield break;
            }
            
            switch (composite._EditorOrderMode)
            {
                case Composite.CompositeOrderMode.Priority:
                    break;
                
                case Composite.CompositeOrderMode.LeftToRight:
                    
                    foreach (var connectedView in connectedViews)
                    {
                        var rect = connectedView.GetPosition();
                        rect.y = ownBottomY + 50f;
                        connectedView.SetPosition(rect);
                    }

                    break;
                
                case Composite.CompositeOrderMode.TopToBottom:

                    var previousY = ownBottomY + 25f;
                    
                    foreach (var connectedView in connectedViews)
                    {
                        var rect = connectedView.GetPosition();
                        rect.x = center.x - rect.width / 2f;
                        rect.y = previousY;
                        connectedView.SetPosition(rect);
                        
                        previousY += rect.height;
                        previousY += 25f;
                    }
                    
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }

            yield return null;
            
            foreach (var connectedView in connectedViews)
            {
                yield return connectedView.OrderChildren();
                yield return null;
            }
        }

        public virtual void OnInputConnectedTo(NodeView outputNodeView) { }
        
        public virtual void OnOutputConnectedTo(NodeView inputNodeView) { }
    }
    
    public class NodeView<TTreeNode> : NodeView
        where TTreeNode : Tree.Nodes.Node
    {
        public override string Title => TreeNode.GetType().Name;
        
        public TTreeNode TreeNode { get; private set; }

        public NodeView(Tree.Nodes.Node treeNode, Action<NodeView> onSelectionChanged)
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
            
            OnSelectionChanged?.Invoke(this);
        }
    }

}