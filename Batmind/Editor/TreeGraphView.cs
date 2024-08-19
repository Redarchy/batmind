﻿using System;
using System.Collections.Generic;
using System.Linq;
using Batmind.Batmind.Editor.Nodes;
using Batmind.Editor.Nodes;
using Batmind.Tree.Nodes;
using Batmind.Tree.Nodes.Actions;
using Batmind.Tree.Nodes.Composites;
using Batmind.Tree.Nodes.Conditions;
using Batmind.Tree.Nodes.Decorators;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Node = Batmind.Tree.Nodes.Node;

namespace Batmind.Editor
{
    public class TreeGraphView : GraphView
    {
        private BehaviourTree _tree;
        private Action<Node> _onSelectionChanged;
        private Action _onTreeCleared;
        private Action<BehaviourTree> _onTreeSaved;
        private Action<BehaviourTree, string> _onTreeSavedAsAsset;
        private Vector2 _mousePosition;

        public TreeGraphView(BehaviourTree tree)
        {
            _tree = tree;

            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new FreehandSelector());
            this.AddManipulator(new EdgeManipulator());
            
            this.RegisterCallback<MouseUpEvent>(OnMouseUp);

            CreateToolbar();
            CreateGraph();
            
            var background = new GridBackground();
            Insert(0, background);
            background.StretchToParentSize();
            background.SendToBack();
        }

        private void OnMouseUp(MouseUpEvent mouseUpEvent)
        {
            var mousePosition = mouseUpEvent.mousePosition;
            _mousePosition = parent.ChangeCoordinatesTo(contentViewContainer, mousePosition);
        }

        public void SetCallbacks(Action<BehaviourTree> onTreeSaved, Action<BehaviourTree, string> onTreeSavedAsAsset,
            Action<Node> onSelectionChanged)
        {
            _onTreeSaved = onTreeSaved;
            _onTreeSavedAsAsset = onTreeSavedAsAsset;
            _onSelectionChanged = onSelectionChanged;
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            if (evt.target is not TreeGraphView)
            {
                return;
            }
            
            GraphViewContextualMenu.Build(evt, this);
        }
        
        private void CreateGraph()
        {
            CreateValidatorGraph();
            AddElement(CreateView(_tree.Root));
        }

        private void CreateValidatorGraph()
        {
            var validatorNode = _tree.Validator;
            AddElement(CreateView(validatorNode));
        }

        #region Save

        private void SaveBehaviourTree()
        {
            var validatorNodeView = GetValidatorNodeView();
            var rootNodeView = GetRootNode();

            var tree = new BehaviourTree
            {
                Priority = 0,
                GraphPosition = default,
                Validator = new Validator(),
                Root = new Root(),
                Blackboard = _tree.Blackboard
            };

            tree.Validator = validatorNodeView.TreeNode;
            tree.Root = rootNodeView.TreeNode;

            IterateThroughConnections(rootNodeView);
            
            _tree = tree;
        }
        
        private RootNodeView GetRootNode()
        {
            foreach (var node in graphElements)
            {
                if (node is RootNodeView rootNodeView)
                {
                    return rootNodeView;
                }
            }

            return null;
        }
        
        private ValidatorNodeView GetValidatorNodeView()
        {
            foreach (var node in graphElements)
            {
                if (node is ValidatorNodeView validatorNodeView)
                {
                    return validatorNodeView;
                }
            }

            return null;
        }

        private void IterateThroughConnections(NodeView nodeView)
        {
            var outputPort = nodeView.OutputPort;

            if (outputPort == null || !outputPort.connected)
            {
                return;
            }

            nodeView.ImplicitTreeNode.GraphPosition = nodeView.GetPosition().position;
            if (nodeView is RootNodeView rootNodeView)
            {
                rootNodeView.TreeNode.Children.Clear();
                var connectedNodes = outputPort.connections.OrderBy(connection =>
                {
                    return (connection.input.node as NodeView).ImplicitTreeNode.Priority;
                }).ToList();
                
                var connectedNodesCount = connectedNodes.Count;
                
                for (var i = 0; i < connectedNodesCount; i++)
                {
                    var connectedNodeView = connectedNodes[i].input.node as NodeView;
                    if (connectedNodeView == null)
                    {
                        continue;
                    }
                    
                    rootNodeView.TreeNode.AddChild(connectedNodeView.ImplicitTreeNode);
                    IterateThroughConnections(connectedNodeView);
                }
            }
            else if (nodeView is SequenceNodeView sequenceNodeView)
            {
                sequenceNodeView.TreeNode.Children.Clear();
                
                var connectedNodes = outputPort.connections.ToList();
                var connectedNodesCount = connectedNodes.Count;
                
                for (var i = 0; i < connectedNodesCount; i++)
                {
                    var connectedNodeView = connectedNodes[i].input.node as NodeView;
                    if (connectedNodeView == null)
                    {
                        continue;
                    }
                    
                    sequenceNodeView.TreeNode.AddChild(connectedNodeView.ImplicitTreeNode);
                    IterateThroughConnections(connectedNodeView);
                }
            }
            else if (nodeView is SelectorNodeView selectorNodeView)
            {
                selectorNodeView.TreeNode.Children.Clear();
                
                var connectedNodes = outputPort.connections.ToList();
                var connectedNodesCount = connectedNodes.Count;
                
                for (var i = 0; i < connectedNodesCount; i++)
                {
                    var connectedNodeView = connectedNodes[i].input.node as NodeView;
                    if (connectedNodeView == null)
                    {
                        continue;
                    }
                    
                    selectorNodeView.TreeNode.AddChild(connectedNodeView.ImplicitTreeNode);
                    IterateThroughConnections(connectedNodeView);
                }
            }
            else if (nodeView is ValidatorNodeView validatorNodeView)
            {
                validatorNodeView.TreeNode.Children.Clear();

                var connectedNodes = outputPort.connections.ToList();
                var connectedNodesCount = connectedNodes.Count;
                
                for (var i = 0; i < connectedNodesCount; i++)
                {
                    var connectedNodeView = connectedNodes[i].input.node as NodeView;
                    if (connectedNodeView == null)
                    {
                        continue;
                    }
                    
                    validatorNodeView.TreeNode.AddChild(connectedNodeView.ImplicitTreeNode);
                    IterateThroughConnections(connectedNodeView);
                }
            }
        }

        #endregion

        private void OnSelectedNodeChanged(Node node)
        {
            _onSelectionChanged?.Invoke(node);
        }


        #region View Creation

        private GraphElement CreateView(Node node)
        {
            switch (node)
            {
                case BehaviourTree behaviourTree:
                    return CreateSubTreeView(behaviourTree);
                case Composite compositeNode:
                    return CreateCompositeView(compositeNode);
                case DecoratorNode decoratorNode:
                    return CreateDecoratorView(decoratorNode);
                case ActionNode actionNode:
                    return CreateActionNodeView(actionNode);
                case ConditionNode conditionNode:
                    return CreateConditionNodeView(conditionNode);
                default:
                    throw new Exception("Type of node is not found!");
            }
        }

        private GraphElement CreateConditionNodeView(ConditionNode conditionNode)
        {
            var conditionNodeView = new ConditionNodeView(conditionNode, OnSelectedNodeChanged);
            
            return conditionNodeView;
        }

        private GraphElement CreateActionNodeView(ActionNode actionNode)
        {
            var actionNodeView = new ActionNodeView(actionNode, OnSelectedNodeChanged);
            
            return actionNodeView;
        }

        private GraphElement CreateSubTreeView(BehaviourTree treeNode)
        {
            var subTreeNodeView = new SubTreeNodeView(treeNode);

            foreach (var node in treeNode.Root.Children)
            {
                AddElement(CreateView(node));
            }
            
            return subTreeNodeView;
        }

        private GraphElement CreateCompositeView(Composite compositeNode)
        {
            NodeView compositeNodeView = null;
            
            switch (compositeNode)
            {
                case Root root:
                    compositeNodeView = new RootNodeView(root, OnSelectedNodeChanged);
                    break;
                case Selector selector:
                    compositeNodeView = new SelectorNodeView(selector, OnSelectedNodeChanged);
                    break;
                case Validator validator:
                    compositeNodeView = new ValidatorNodeView(validator, OnSelectedNodeChanged);
                    break;
                case PrioritySelector prioritySelector:
                    compositeNodeView = new PrioritySelectorNodeView(prioritySelector, OnSelectedNodeChanged);
                    break;
                case RandomOrderSelector randomOrderSelector:
                    compositeNodeView = new RandomOrderSelectorNodeView(randomOrderSelector, OnSelectedNodeChanged);
                    break;
                case Sequence sequence:
                    compositeNodeView = new SequenceNodeView(sequence, OnSelectedNodeChanged);
                    break;
                default:
                    throw new Exception("Type of node is not found!");
            }
            
            foreach (var node in compositeNode.Children)
            {
                var childNodeView = CreateView(node) as NodeView;

                var edge = new Edge();
                compositeNodeView.ConnectOutputTo(edge);
                childNodeView.ConnectInputTo(edge);
                
                AddElement(childNodeView);
                Add(edge);
            }
            
            return compositeNodeView;
        }
        
        private GraphElement CreateDecoratorView(Node node)
        {
            switch (node)
            {
                case Inverter inverter:
                    return new InverterNodeView(inverter, OnSelectedNodeChanged);
                default:
                    throw new Exception("Type of node is not found!");
            }
        }

        #endregion
        
        private void CreateToolbar()
        {
            var buttonToolbar = new Toolbar();

            AddToolbarButton(buttonToolbar, "Clear", () =>
            {
                graphElements.ForEach(RemoveElement);                
                _tree.Root.Children.Clear();
            });
            
            AddToolbarButton(buttonToolbar, "Save Graph", () =>
            {
                SaveBehaviourTree();
                _onTreeSaved?.Invoke(_tree);
            });
            AddToolbarButton(buttonToolbar, "Save as Asset", () =>
            {
                SaveAsAssetWindow.OnSaved += savedFileName =>
                {
                    _onTreeSavedAsAsset?.Invoke(_tree, savedFileName);
                };
                
                SaveAsAssetWindow.ShowWindow();
            });
            
            Add(buttonToolbar);
            
            void AddToolbarButton(Toolbar toolbar, string buttonName, Action callback)
            {
                var addNodeButton = new Button(callback)
                {
                    text = buttonName
                };
                toolbar.Add(addNodeButton);
            }

        }
        
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();
            var selectedPort = startPort;

            ports.ForEach(targetPort =>
            {
                
                if (selectedPort != targetPort &&
                    selectedPort.node != targetPort.node &&
                    selectedPort.direction != targetPort.direction &&
                    AreConnectable(selectedPort, targetPort)
                    )
                {
                    compatiblePorts.Add(targetPort);
                }
            });

            return compatiblePorts;

            bool AreConnectable(Port outputPort, Port inputPort)
            {
                if (outputPort.capacity == Port.Capacity.Single && outputPort.connected)
                {
                    return false;
                }
                
                if (inputPort.capacity == Port.Capacity.Single && inputPort.connected)
                {
                    return false;
                }

                return true;
            }
            
        }

        #region Node View Adders

        public void AddNewSelector()
        {
            var selector = new Selector
            {
                Priority = 0,
                GraphPosition = Vector2.zero,
                Children = new List<Node>()
            };
            
            _tree.Root.Children.Add(selector);

            var compositeView = CreateCompositeView(selector);
            var position = compositeView.GetPosition();
            position.position = _mousePosition;

            AddElement(compositeView);
            
            compositeView.SetPosition(position);
        }

        public void AddNewPrioritySelector()
        {
            var selector = new PrioritySelector
            {
                Priority = 0,
                GraphPosition = Vector2.zero,
                Children = new List<Node>()
            };
            
            _tree.Root.Children.Add(selector);

            AddElement(CreateCompositeView(selector));
        }

        public void AddNewRandomOrderSelector()
        {
            var selector = new RandomOrderSelector
            {
                Priority = 0,
                GraphPosition = Vector2.zero,
                Children = new List<Node>()
            };
            
            _tree.Root.Children.Add(selector);

            AddElement(CreateCompositeView(selector));
        }
        
        public void AddNewSequence()
        {
            var sequence = new Sequence
            {
                Priority = 0,
                GraphPosition = Vector2.zero,
                Children = new List<Node>()
            };
            
            _tree.Root.Children.Add(sequence);
            
            AddElement(CreateCompositeView(sequence));
        }

        public void AddNewAction()
        {
            CreateActionNodeWindow.OnSelected = CreateActionWithType;
                
            CreateActionNodeWindow.ShowWindow();

            void CreateActionWithType(Type actionType)
            {
                var actionNode = Activator.CreateInstance(actionType) as ActionNode;
                var actionNodeView = CreateView(actionNode) as ActionNodeView;
                actionNodeView.ExplicitNodeType = actionType;
                AddElement(actionNodeView);
            }
        }

        public void AddNewCondition()
        {
            CreateConditionNodeWindow.OnSelected = CreateConditionWithType;
                
            CreateConditionNodeWindow.ShowWindow();

            void CreateConditionWithType(Type conditionType)
            {
                var conditionNode = Activator.CreateInstance(conditionType) as ConditionNode;
                var conditionNodeView = CreateView(conditionNode) as ConditionNodeView;
                conditionNodeView.ExplicitNodeType = conditionType;
                AddElement(conditionNodeView);
            }
        }

        #endregion
    }

}