using System;
using System.Collections.Generic;
using System.Linq;
using Batmind.Tree;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Batmind.Editor
{
    [CustomPropertyDrawer(typeof(BehaviourContext), true)]
    public class BehaviourContextDrawer : PropertyDrawer
    {
        private List<Type> TypeList { get; set; }
        private List<string> TypeNameList { get; set; }
        
        private int _currentSelectedIndex;
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            TypeList = GetTypeList();
            TypeNameList = GetTypeNameList();
            
            var container = GetContainer();
            
            var currentSelectedName = string.Empty;
            _currentSelectedIndex = 0;
            
            if (property == null || property.managedReferenceValue == null)
            {
                property.boxedValue = Activator.CreateInstance(TypeList[0]);
                property.serializedObject.ApplyModifiedProperties();

                currentSelectedName = TypeNameList[0];
                _currentSelectedIndex = TypeNameList.IndexOf(currentSelectedName);
                property.serializedObject.Update();
            }

            currentSelectedName = property.managedReferenceValue.GetType().Name;
            _currentSelectedIndex = TypeNameList.IndexOf(currentSelectedName);
            
            var dropdownField = new DropdownField();
            dropdownField.choices = TypeNameList;
            dropdownField.index = _currentSelectedIndex;
            dropdownField.RegisterValueChangedCallback(value =>
            {
                if (_currentSelectedIndex == dropdownField.index)
                {
                    return;
                }
                
                _currentSelectedIndex = dropdownField.index;
                
                property.boxedValue = Activator.CreateInstance(TypeList[_currentSelectedIndex]);
                property.serializedObject.ApplyModifiedProperties();
                property.serializedObject.Update();
            });
            
            container.Add(dropdownField);
            
            container.Add(new PropertyField(property));
            
            return container;
        }

        private static VisualElement GetContainer()
        {
            var container = new VisualElement();
            container.style.backgroundColor = new Color(0f, 0f, 0f, 0.35f);
            container.style.marginTop = 15f;
            container.style.marginBottom = 15f;
            container.style.marginLeft = 15f;
            container.style.marginRight = 15f;
            container.style.borderBottomLeftRadius = 8f;
            container.style.borderBottomRightRadius = 8f;
            container.style.borderTopLeftRadius = 8f;
            container.style.borderTopRightRadius = 8f;
            container.style.borderBottomWidth = 3f;
            container.style.borderTopWidth = 3f;
            container.style.borderLeftWidth = 3f;
            container.style.borderRightWidth = 3f;
            container.style.borderBottomColor = Color.black;
            container.style.borderTopColor = Color.black;
            container.style.borderLeftColor = Color.black;
            container.style.borderRightColor = Color.black;
            
            var label = new Label("Behaviour Context");
            label.style.backgroundColor = new Color(0f, 0f, 0f, 0.2f);
            label.style.color = Color.white;
            label.style.fontSize = 15f;
            label.style.alignSelf = Align.Center;
            label.style.justifyContent = Justify.SpaceAround;
            label.style.unityTextAlign = TextAnchor.MiddleCenter;
            label.style.alignContent = Align.Stretch;
            label.style.alignSelf = Align.Stretch;
            label.style.height = 30f;
            
            container.Add(label);
            
            return container;
        }

        private List<Type> GetTypeList()
        {
            var list = new List<Type>();
            var baseType = typeof(BehaviourContext);
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            
            foreach (var assembly in assemblies)
            {
                list.AddRange(assembly.GetTypes().Where(t => baseType.IsAssignableFrom(t)));
            }
            
            return list;
        }

        private List<string> GetTypeNameList()
        {
            var list = new List<string>();
            
            foreach (var type in TypeList)
            {
                list.Add(type.Name);
            }
            
            return list;
        }
    }
}