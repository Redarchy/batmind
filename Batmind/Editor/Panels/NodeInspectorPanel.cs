using Batmind.Tree.Nodes;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Batmind.Editor.Panels
{
    public class NodeInspectorPanel : VisualElement
    {
        private Node _selectedNode;
        private VisualElement _container;
        
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

        public void OnNodeSelected(Node node)
        {
            _selectedNode = node;
            
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
            
            var typeLabel = GetTypeLabel();
            _container.Add(typeLabel);

            var scrollBar = GetScrollBar();
            
            var holder = ScriptableObject.CreateInstance<SerializedObjectHolder>();
            holder.SerializedNode = _selectedNode;
            
            var serializedObject = new SerializedObject(holder);

            var property = serializedObject.FindProperty(holder.NodeName);
            
            while (property.NextVisible(true))
            {
                var propertyField = new PropertyField(property);
                propertyField.Bind(serializedObject);
                scrollBar.Add(propertyField);
            }
            
            _container.Add(scrollBar);
            
            Add(_container);
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