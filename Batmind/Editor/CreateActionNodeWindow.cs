using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Batmind.Tree.Nodes.Actions;
using UnityEditor;
using UnityEngine;

namespace Batmind.Editor
{
    public class CreateActionNodeWindow : EditorWindow
    {
        public static Action<Type> OnSelected;

        private static Type ActionTypeItself = typeof(ActionNode);
        
        private static List<Type> ActionTypes { get; set; }
        private static string[] ActionTypeNames { get; set; }
        private static int _selectedTypeIndex = 0;
        
        public static void ShowWindow()
        {
            ActionTypes = new List<Type>();
            
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            
            foreach (var assembly in assemblies)
            {
                ActionTypes.AddRange(assembly.GetTypes().Where(t => t != ActionTypeItself && ActionTypeItself.IsAssignableFrom(t)));
            }

            ActionTypeNames = new string[ActionTypes.Count];

            for (var index = 0; index < ActionTypes.Count; index++)
            {
                var actionType = ActionTypes[index];

                ActionTypeNames[index] = actionType.Name;
            }

            GetWindow<CreateActionNodeWindow>("Create Action");
        }

        private void OnGUI()
        {
            _selectedTypeIndex = EditorGUILayout.Popup(_selectedTypeIndex, ActionTypeNames);

            if (GUILayout.Button("Select"))
            {
                Close();

                OnSelected?.Invoke(ActionTypes[_selectedTypeIndex]);

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
