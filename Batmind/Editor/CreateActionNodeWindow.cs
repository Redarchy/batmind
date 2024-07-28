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
        
        private static List<Type> ActionTypes = Assembly.GetAssembly(ActionTypeItself).GetTypes().Where(t => t != ActionTypeItself && ActionTypeItself.IsAssignableFrom(t)).ToList();
        private static string[] ActionTypeNames = ActionTypes.Select(t => t.Name).ToArray();
        private static int _selectedTypeIndex = 0;

        public static void ShowWindow()
        {
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
