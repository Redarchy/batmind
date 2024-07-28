using System;
using UnityEditor;
using UnityEngine;

namespace Batmind.Editor
{
    public class SaveAsAssetWindow : EditorWindow
    {
        public static string _assetName = "";

        public static Action<string> OnSaved;
        
        public static void ShowWindow()
        {
            GetWindow<SaveAsAssetWindow>("Save As Asset");
        }

        private void OnGUI()
        {
            GUILayout.Label("Asset name:", EditorStyles.boldLabel);

            _assetName = EditorGUILayout.TextArea(_assetName, GUILayout.Height(100));

            if (GUILayout.Button("Save"))
            {
                Close();

                OnSaved?.Invoke(_assetName);

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
            OnSaved = null;
        }
    }
}