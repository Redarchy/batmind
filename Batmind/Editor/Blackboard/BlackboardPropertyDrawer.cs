using System;
using System.Text;
using Batmind.Board;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Batmind.Editor
{
    [CustomPropertyDrawer(typeof(Blackboard))]
    public class BlackboardPropertyDrawer : PropertyDrawer
    {
        private VisualElement _container;
        public static Blackboard Blackboard { get; private set; }
        public static Action BlackboardModifiedCallback;
        private SerializedProperty _property;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            _property = property;
            Blackboard = property.boxedValue as Blackboard;

            _container = new VisualElement();
            
            FillContainer(property);
            
            return _container;
        }
        
        private void AddNewParameter(SerializedProperty property)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("NewKey");

            var blackboard = property.boxedValue as Blackboard;
            
            while (blackboard.ContainsKeyWithName(stringBuilder.ToString()))
            {
                stringBuilder.Append("0");
            }
            
            var key = blackboard.GetOrRegisterKey(stringBuilder.ToString());
            var value = new BlackboardEntry
            {
                Key = key,
                ValueType = AnyValue.ValueType.Int,
                Value = default
            };
            
            blackboard.SetValue(key, value);

            property.boxedValue = blackboard;
            
            UpdateValues(property.serializedObject);
            FillContainer(property);
        }

        private void FillContainer(SerializedProperty property)
        {
            _container.Clear();
            
            _container.style.flexDirection = FlexDirection.Column;

            var serializedObject = property.serializedObject;
            var entriesProperty = property.FindPropertyRelative("_entries");
            
            var header = new VisualElement();
            header.StretchToParentWidth();
            header.style.flexDirection = FlexDirection.Row;
            header.style.alignSelf = Align.Stretch;
            header.style.justifyContent = Justify.SpaceAround;
            header.style.backgroundColor = new Color(0f, 0f, 0f, 0.5f);
            header.style.height = 25f;
            header.style.position = Position.Relative;

            var elementPercentageWidth = Length.Percent(100/3f);
            
            var keyLabel = new Label("Key");
            keyLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
            keyLabel.style.alignContent = Align.Center;
            keyLabel.style.width = elementPercentageWidth;
            
            var typeLabel = new Label("Type");
            typeLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
            typeLabel.style.width = elementPercentageWidth;
            
            var valueLabel = new Label("Value");
            valueLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
            valueLabel.style.alignContent = Align.Center;
            valueLabel.style.width = elementPercentageWidth;

            header.Add(keyLabel);
            header.Add(typeLabel);
            header.Add(valueLabel);
            
            _container.Add(header);
            
            for (var i = 0; i < entriesProperty.arraySize; i++)
            {
                var entryProperty = entriesProperty.GetArrayElementAtIndex(i);
                var keyNameProperty = entryProperty.FindPropertyRelative("Key").FindPropertyRelative("name");
                var valueTypeProperty = entryProperty.FindPropertyRelative("ValueType");
                var valueProperty = entryProperty.FindPropertyRelative("Value");

                var elementContainer = new VisualElement();

                var keyNameField = new PropertyField(keyNameProperty, "");
                keyNameField.style.width = elementPercentageWidth;
                keyNameField.RegisterValueChangeCallback(e => UpdateValues(serializedObject));
                elementContainer.Add(keyNameField);
            
                var valueTypeField = new PropertyField(valueTypeProperty, "");
                valueTypeField.style.width = elementPercentageWidth;
                valueTypeField.RegisterValueChangeCallback(e => UpdateValues(serializedObject));
                elementContainer.Add(valueTypeField);

                switch ((AnyValue.ValueType) valueTypeProperty.enumValueIndex)
                {
                    case AnyValue.ValueType.Int:
                        var intValue = valueProperty.FindPropertyRelative("IntValue");
                        var intField = new PropertyField(intValue, "");
                        intField.style.width = elementPercentageWidth;
                        elementContainer.Add(intField);
                        break;
                    case AnyValue.ValueType.Float:
                        var floatValue = valueProperty.FindPropertyRelative("FloatValue");
                        var floatField = new PropertyField(floatValue, "");
                        floatField.style.width = elementPercentageWidth;
                        elementContainer.Add(floatField);
                        break;
                    case AnyValue.ValueType.Bool:
                        var boolValue = valueProperty.FindPropertyRelative("BoolValue");
                        var boolField = new PropertyField(boolValue, "");
                        boolField.style.width = elementPercentageWidth;
                        elementContainer.Add(boolField);
                        break;
                    case AnyValue.ValueType.String:
                        var stringValue = valueProperty.FindPropertyRelative("StringValue");
                        var stringField = new PropertyField(stringValue, "");
                        stringField.style.width = elementPercentageWidth;
                        elementContainer.Add(stringField);
                        break;
                    case AnyValue.ValueType.Vector3:
                        var vector3Value = valueProperty.FindPropertyRelative("Vector3Value");
                        var vector3Field = new PropertyField(vector3Value, "");
                        vector3Field.style.width = elementPercentageWidth;
                        elementContainer.Add(vector3Field);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            
                elementContainer.style.flexDirection = FlexDirection.Row;
                elementContainer.style.width = _container.style.width;
                elementContainer.style.alignSelf = Align.Stretch;
                elementContainer.style.alignContent = Align.Stretch;
                elementContainer.style.justifyContent = Justify.SpaceAround;
                elementContainer.style.alignItems = Align.Stretch;
                elementContainer.style.height = 35f;
                elementContainer.style.backgroundColor = new Color(0f, 0f, 0f, 0.35f);

                _container.Add(elementContainer);
                _container.Add(elementContainer);
                _container.Add(elementContainer);
                _container.style.alignItems = Align.FlexStart;
            }
            
            var footer = new VisualElement();
            var addButton = new Button(() =>
            {
                AddNewParameter(property);
            });
            addButton.text = "+";
            footer.style.left = 5;

            footer.Add(addButton);
            _container.Add(footer);
            
            _container.Bind(property.serializedObject);
            EditorUtility.SetDirty(serializedObject.targetObject);
            property.serializedObject.ApplyModifiedProperties();
            property.serializedObject.Update();
        }

        private void UpdateValues(SerializedObject serializedObject)
        {
            Blackboard = _property.boxedValue as Blackboard;
            EditorUtility.SetDirty(serializedObject.targetObject);
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
            
            BlackboardModifiedCallback?.Invoke();
        }
    }
}