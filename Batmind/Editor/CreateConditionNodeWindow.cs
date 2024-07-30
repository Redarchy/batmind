using System;
using System.Collections.Generic;
using System.Linq;
using Batmind.Tree.Nodes.Conditions;
using UnityEditor;
using UnityEngine;

namespace Batmind.Editor
{
    public class CreateConditionNodeWindow : EditorWindow
    {
        public static Action<Type> OnSelected;

        private static Type ConditionTypeItself = typeof(ConditionNode);
        
        private static List<Type> ConditionTypes { get; set; }
        private static string[] ConditionTypeNames { get; set; }
        
        private static int _selectedTypeIndex = 0;

        public static void ShowWindow()
        {
            ConditionTypes = new List<Type>();
            
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            
            foreach (var assembly in assemblies)
            {
                ConditionTypes.AddRange(assembly.GetTypes().Where(t => t != ConditionTypeItself && ConditionTypeItself.IsAssignableFrom(t)));
            }

            ConditionTypeNames = new string[ConditionTypes.Count];

            for (var index = 0; index < ConditionTypes.Count; index++)
            {
                var actionType = ConditionTypes[index];

                ConditionTypeNames[index] = actionType.Name;
            }

            GetWindow<CreateConditionNodeWindow>("Create Condition");
        }

        private void OnGUI()
        {
            _selectedTypeIndex = EditorGUILayout.Popup(_selectedTypeIndex, ConditionTypeNames);

            if (GUILayout.Button("Select"))
            {
                Close();

                OnSelected?.Invoke(ConditionTypes[_selectedTypeIndex]);

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
