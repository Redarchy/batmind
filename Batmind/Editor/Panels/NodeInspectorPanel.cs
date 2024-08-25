using System;
using System.Collections.Generic;
using System.Linq;
using Batmind.Tree.Nodes;
using Batmind.Tree.Nodes.Composites;
using Batmind.Utils;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Batmind.Editor.Panels
{
    public class NodeInspectorPanel : VisualElement
    {
        private Node _selectedNode;
        private NodeView _selectedNodeView;
        private VisualElement _container;
        private SerializedObject _serializedObject;

        public NodeInspectorPanel()
        {
            SetStyle();

            _container = new VisualElement();
            Add(_container);
            _container.StretchToParentSize();
            _container.style.marginTop = 35f;
        }

        private void SetStyle()
        {
            Clear();

            var header = new VisualElement();
            
            var title = new Label("Node Inspector Panel");
            header.Add(title);
            
            var closeButton = new Button(() => visible = false);
            closeButton.text = "X";
            header.Add(closeButton);
            header.style.flexDirection = FlexDirection.Row;
            header.StretchToParentWidth();
            header.style.alignSelf = Align.Center;
            header.style.justifyContent = Justify.SpaceBetween;
            
            Add(header);
            
            style.height = Length.Percent(50f);
            style.width = Length.Percent(20f);
            style.position = Position.Absolute;
            style.bottom = 0;
            style.left = 0;
            
            var backgroundColor = Color.black;
            backgroundColor.a = 0.5f;
            style.backgroundColor = new StyleColor(backgroundColor); // Optional: set a background color for visibility
        }

        public float CalculateHeight()
        {
            float height = 0f;
            foreach (var child in Children())
            {
                height += child.resolvedStyle.height;
            }
            return height;
        }

        public void OnNodeSelected(NodeView nodeView)
        {
            _selectedNodeView = nodeView;
            _selectedNode = nodeView.ImplicitTreeNode;
            
            CreateGUI();
        }

        private void CreateGUI()
        {
            SetStyle();
            _container.Clear();
            
            if (_selectedNode == null)
            {
                return;
            }

            var headerContainer = new VisualElement();
            headerContainer.style.flexDirection = FlexDirection.Row;
            headerContainer.style.justifyContent = Justify.SpaceBetween;
            
            var typeLabel = GetTypeLabel();
            headerContainer.Add(typeLabel);
            
            if (_selectedNode is Composite)
            {
                var orderChildrenButton = new Button();
                orderChildrenButton.style.color = Color.white;
                orderChildrenButton.style.backgroundColor = Color.green.WithG(0.5f);
                orderChildrenButton.text = "Order Children";
                orderChildrenButton.clicked += () =>
                {
                    _selectedNodeView.OrderChildren();
                };
                
                headerContainer.Add(orderChildrenButton);
            }

            _container.Add(headerContainer);
            
            var scrollBar = GetScrollBar();
            
            var holder = ScriptableObject.CreateInstance<SerializedObjectHolder>();
            holder.SerializedNode = _selectedNode;
            
            _serializedObject = new SerializedObject(holder);

            var property = _serializedObject.FindProperty(holder.NodeName);
            
            while (property.NextVisible(true))
            {

                // Since PropertyFields does not cover SerializeReferenced fields
                if (property.propertyType == SerializedPropertyType.ManagedReference)
                { 
                    // Create a dropdown to let the user select the type
                    var typeDropdown = CreateTypeDropdown(property.Copy());
                    scrollBar.Add(typeDropdown);

                    continue;
                }

                var propertyField = new PropertyField(property);
                propertyField.Bind(_serializedObject);
                scrollBar.Add(propertyField);
            }
            
            _container.Add(scrollBar);
            
            Add(_container);
        }
        
        private VisualElement CreateTypeDropdown(SerializedProperty property)
        {
            var typeNameParts = property.managedReferenceFieldTypename.Split(" ");
            var typeName = typeNameParts[1] + ", " + typeNameParts[0];
            var baseType = Type.GetType(typeName);
            
            var (typeNames, asmQualifiedNames) = GetAvailableTypes(baseType);

            if (property.managedReferenceValue == null && asmQualifiedNames.Count > 0)
            {
                var selectedType = Type.GetType(asmQualifiedNames[0]);
                property.serializedObject.Update();
                property.managedReferenceValue = Activator.CreateInstance(selectedType);
                var isDifferent = property.serializedObject.CopyFromSerializedPropertyIfDifferent(property);
                property.serializedObject.ApplyModifiedProperties();
            }

            var currentType = Type.GetTypeFromHandle(Type.GetTypeHandle(property.boxedValue));
            var currentTypeIndex = typeNames.IndexOf(currentType.Name);
            
            var dropdown = new PopupField<string>($"{property.displayName}", typeNames, currentTypeIndex);
            
            dropdown.RegisterValueChangedCallback(evt =>
            {
                var index = typeNames.IndexOf(evt.newValue);
                var asmQualifiedName = asmQualifiedNames[index];
                var selectedType = Type.GetType(asmQualifiedName);
                if (selectedType != null)
                {
                    // Assign the new type to the SerializeReference field
                    property.serializedObject.Update();
                    property.managedReferenceValue = Activator.CreateInstance(selectedType);
                    var isDifferent = property.serializedObject.CopyFromSerializedPropertyIfDifferent(property);
                    property.serializedObject.ApplyModifiedProperties();
                    
                    CreateGUI();
                }
            });

            
            return dropdown;
        }
        
        private VisualElement GetScrollBar()
        {
            var scrollView = new ScrollView();
            scrollView.style.flexGrow = 1;

            return scrollView;
        }

        private Label GetTypeLabel()
        {
            var typeLabel = new Label($"Type: {_selectedNode.GetType().Name}");
            typeLabel.style.color = Color.yellow;
            
            return typeLabel;
        }

        private (List<string>, List<string>) GetAvailableTypes(Type baseType)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var typeNames = new List<string>();
            var asmQualifiedNames = new List<string>();
            
            foreach (var assembly in assemblies)
            {
                typeNames.AddRange(assembly.GetTypes().Where(t => t != baseType && baseType.IsAssignableFrom(t)).Select(t => t.Name));
                asmQualifiedNames.AddRange(assembly.GetTypes().Where(t => t != baseType && baseType.IsAssignableFrom(t)).Select(t => t.AssemblyQualifiedName));
                
            }
            
            return (typeNames, asmQualifiedNames);
        }
        
        private class SerializedObjectHolder : ScriptableObject
        {
            [SerializeReference] public Node SerializedNode;
            public string NodeName => nameof(SerializedNode);

            public SerializedObjectHolder(Node serializedNode)
            {
                SerializedNode = serializedNode;
            }
        }
    }
}