using Batmind.Board;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

namespace Batmind.Editor.Panels
{
    public class BlackboardInspectorPanel : VisualElement
    {
        private SerializedObjectHolder _blackboardHolder;
        private SerializedObject _serializedObject;
        private Blackboard _blackboard;
        private VisualElement _container;
        
        public BlackboardInspectorPanel(Blackboard blackboard)
        {
            _blackboard = blackboard;
            
            _blackboardHolder = ScriptableObject.CreateInstance<SerializedObjectHolder>();
            _blackboardHolder.SerializedBlackboard = _blackboard;
            
            _serializedObject = new SerializedObject(_blackboardHolder);
            
            SetStyle();
            AddContent();
        }

        private void SetStyle()
        {
            SetHeightByVisibility(true);
            style.width = 300f;
            style.position = Position.Absolute;
            style.right = 0;
            style.top = 50;
            
            var backgroundColor = Color.black;
            backgroundColor.a = 0.5f;
            style.backgroundColor = new StyleColor(backgroundColor); // Optional: set a background color for visibility

            var header = new VisualElement();
            
            var title = new Label("Blackboard Inspector Panel");
            header.Add(title);
            
            var closeButton = new Button(() =>
            {
                var willBeVisible = !_container.visible;
                SetHeightByVisibility(willBeVisible);
                
                _container.visible = willBeVisible;
            });
            closeButton.text = "X";
            header.Add(closeButton);
            header.style.flexDirection = FlexDirection.Row;
            header.StretchToParentWidth();
            header.style.alignSelf = Align.Center;
            header.style.justifyContent = Justify.SpaceBetween;
            
            Add(header);
        }

        private void SetHeightByVisibility(bool visible)
        {
            if (visible)
            {
                style.height = Length.Percent(50f);
            }
            else
            {
                style.height = 40;
            }
        }

        private void AddContent()
        {
            _container = new VisualElement();
            _container.StretchToParentSize();
            _container.style.marginTop = 65f;
            
            AddBlackboardInspector();
            Add(_container);
        }

        private void AddBlackboardInspector()
        {
            if (_blackboard == null)
            {
                return;
            }
            
            var scrollBarContent = GetScrollBar();
            scrollBarContent.style.bottom = 30;
            
            var iterator = _serializedObject.GetIterator();
            while (iterator.Next(true))
            {
                var property = iterator;
                if (property.name == "SerializedBlackboard")
                {
                    var propertyField = new PropertyField(property);
                    propertyField.Bind(_serializedObject);
                    scrollBarContent.Add(propertyField);
            
                    _container.Add(scrollBarContent);
                    
                    break;
                }
            }
        }

        private VisualElement GetScrollBar()
        {
            var scrollView = new ScrollView();
            scrollView.style.flexGrow = 1;

            return scrollView;
        }
        
        private class SerializedObjectHolder : ScriptableObject
        {
            [SerializeField] public Blackboard SerializedBlackboard;
            public string BoardName => nameof(SerializedBlackboard);

            public SerializedObjectHolder(Blackboard serializedBlackboard)
            {
                SerializedBlackboard = serializedBlackboard;
            }
        }
    }
    
}