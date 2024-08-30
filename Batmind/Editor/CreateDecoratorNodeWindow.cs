using System;
using System.Collections.Generic;
using System.Linq;
using Batmind.Tree.Nodes.Decorators;
using UnityEditor;
using UnityEngine;

namespace Batmind.Editor
{
    public class CreateDecoratorNodeWindow : EditorWindow
    {
        public static Action<Type> OnSelected;

        private static Type DecoratorTypeItself = typeof(DecoratorNode);
        
        private static List<Type> DecoratorTypes { get; set; }
        private static string[] ActionTypeNames { get; set; }
        private static int _selectedTypeIndex = 0;
        
        public static void ShowWindow()
        {
            DecoratorTypes = new List<Type>();
            
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            
            foreach (var assembly in assemblies)
            {
                DecoratorTypes.AddRange(assembly.GetTypes().Where(t => t != DecoratorTypeItself && DecoratorTypeItself.IsAssignableFrom(t)));
            }

            ActionTypeNames = new string[DecoratorTypes.Count];

            for (var index = 0; index < DecoratorTypes.Count; index++)
            {
                var actionType = DecoratorTypes[index];

                ActionTypeNames[index] = actionType.Name;
            }

            GetWindow<CreateDecoratorNodeWindow>("Create Decorator");
        }

        private void OnGUI()
        {
            _selectedTypeIndex = EditorGUILayout.Popup(_selectedTypeIndex, ActionTypeNames);

            if (GUILayout.Button("Select"))
            {
                Close();

                OnSelected?.Invoke(DecoratorTypes[_selectedTypeIndex]);

                ClearCallbacks();
            }
            
            if (GUILayout.Button("Cancel"))
            {
                Close();
                
                ClearCallbacks();
            }
        }

        private void ClearCallbacks()
        {
            OnSelected = null;
            _selectedTypeIndex = 0;
        }
    }
}
