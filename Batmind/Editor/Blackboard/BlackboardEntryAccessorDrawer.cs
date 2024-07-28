using System.Collections.Generic;
using System.Linq;
using Batmind.Board;
using UnityEditor;
using UnityEngine.UIElements;

namespace Batmind.Editor
{
    [CustomPropertyDrawer(typeof(EntryAccessor<>), true)]
    public class BlackboardEntryAccessorDrawer : PropertyDrawer
    {
        private const string EntryKeyHashFieldName = "EntryKeyHash";

        private VisualElement _container;
        private List<BlackboardEntry> _blackboardEntries;
        private int _assignedIndex;
        private List<string> _blackboardEntryNames;
        private DropdownField _entryDropdownField;
        private SerializedProperty _property;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            _container = new VisualElement();
            
            FillContainer(property);
            
            return _container;
        }

        private void FillContainer(SerializedProperty property)
        {
            _property = property;
            
            _container.Clear();
            _container.style.flexDirection = FlexDirection.Row;
            _container.style.alignContent = Align.Stretch;
            _container.style.justifyContent = Justify.SpaceBetween;
            
            BlackboardPropertyDrawer.BlackboardModifiedCallback += OnBlackboardModified;

            var propertyName = new Label(property.name);
            
            _entryDropdownField = new DropdownField();
            SetEntryDropdownField();
            
            _container.Add(propertyName);
            _container.Add(_entryDropdownField);
        }

        private void OnBlackboardModified()
        {
            SetEntryDropdownField();
        }

        private void SetEntryDropdownField()
        {
            var genericType = fieldInfo.FieldType.GetGenericArguments()[0];
            var anyValueType = AnyValue.FromExplicitType(genericType);

            var hashKeyProperty = _property.FindPropertyRelative(EntryKeyHashFieldName);
            var hashedKey = hashKeyProperty.intValue;
            
            _blackboardEntries = new List<BlackboardEntry>();
            _blackboardEntryNames = new List<string>();
            
            BlackboardPropertyDrawer.Blackboard.GetAllEntriesWithValueTypeNonAlloc(anyValueType, _blackboardEntries);
            _blackboardEntryNames = _blackboardEntries.Select(e => e.Key.name).ToList();

            _entryDropdownField.style.flexGrow = 1f;
            
            if (_blackboardEntryNames.Count <= 0)
            {
                _entryDropdownField.choices = new List<string> {"No Match"};
                _entryDropdownField.index = 0;
                
                return;
            }
            
            var isAssigned = BlackboardPropertyDrawer.Blackboard.ContainsKeyWithHash(hashedKey, out var key);
            if (!isAssigned)
            {
                _assignedIndex = 0;
                hashedKey = _blackboardEntries[0].Key.hashedKey;
                _property.FindPropertyRelative(EntryKeyHashFieldName).intValue = hashedKey;
                _property.serializedObject.ApplyModifiedProperties();
            }
            
            _assignedIndex = _blackboardEntries.FindIndex(item => item.Key.hashedKey == hashedKey);
            
            _entryDropdownField.choices = _blackboardEntryNames;
            _entryDropdownField.index = _assignedIndex;
            _entryDropdownField.RegisterValueChangedCallback(changeEvent =>
            {
                _assignedIndex = _entryDropdownField.index;
                var selectedEntry = _blackboardEntries[_assignedIndex];
                
                _property.serializedObject.Update();
                _property.FindPropertyRelative(EntryKeyHashFieldName).intValue = selectedEntry.Key.hashedKey;
                _property.serializedObject.ApplyModifiedProperties();
            });
        }
    }
}