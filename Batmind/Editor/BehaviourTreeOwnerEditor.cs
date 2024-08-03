using Batmind.Tree;
using Batmind.Tree.Nodes;
using Batmind.Utils;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Batmind.Editor
{
    [CustomEditor(typeof(BehaviourTreeOwner), true)]
    public class BehaviourTreeOwnerEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var behaviourTreeOwner = target as BehaviourTreeOwner;
            
            var container = GetContainer();

            AddButtons(container, behaviourTreeOwner);
            AddContext(container);
            
            return container;
        }

        private static VisualElement GetContainer()
        {
            var container = new VisualElement();
            container.style.backgroundColor = Color.gray;
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
            return container;
        }

        private void AddContext(VisualElement container)
        {
            var contextContainer = new VisualElement();
            contextContainer.BringToFront();
            
            var serializedContext = serializedObject.FindProperty("_context");
            var propertyField = new PropertyField(serializedContext);
            var n = new InspectorElement();
            propertyField.Bind(serializedObject);
            propertyField.BindProperty(serializedObject);

            contextContainer.Add(propertyField);
            
            container.Add(contextContainer);
        }

        private void AddButtons(VisualElement container, BehaviourTreeOwner behaviourTreeOwner)
        {
            var label = new Label("Batmind Behaviour Tree Owner");
            label.style.backgroundColor = new Color(0f, 0f, 0f, 0.5f);
            label.style.color = Color.white;
            label.style.fontSize = 15f;
            label.StretchToParentWidth();
            label.style.alignSelf = Align.Center;
            label.style.justifyContent = Justify.Center;
            label.style.unityTextAlign = TextAnchor.MiddleCenter;
            label.style.height = 30f;
            
            container.Add(label);

            var runToggle = new Toggle("Run");
            runToggle.value = behaviourTreeOwner.IsRunning;
            runToggle.RegisterValueChangedCallback(value =>
            {
                behaviourTreeOwner.SetRunning(value.newValue);
            });
            runToggle.style.color = Color.black;
            runToggle.style.alignSelf = Align.Center;
            runToggle.style.fontSize = 20f;
            runToggle.style.marginTop = 55f;
            runToggle.style.color = Color.cyan;
            
            container.Add(runToggle);

            var validateToggle = new Toggle("Validate");
            validateToggle.value = behaviourTreeOwner.IsValidating;
            validateToggle.RegisterValueChangedCallback(value =>
            {
                behaviourTreeOwner.SetValidation(value.newValue);
            });
            validateToggle.style.color = Color.black;
            validateToggle.style.alignSelf = Align.Center;
            validateToggle.style.fontSize = 20f;
            validateToggle.style.color = Color.cyan;
            
            container.Add(validateToggle);
            
            var openEditorButton = new Button(() =>
            {
                var instance = EditorWindow.GetWindow<GraphWindow>();
                var treeOwner = (BehaviourTreeOwner) target;
                
                var jsonCopy = treeOwner.Tree.ToJson();
                var treeClone = jsonCopy.FromJson<BehaviourTree>();
                
                instance.SetTarget(treeOwner, treeClone);
                instance.RecreateGUI();
                instance.Show();
            });
            openEditorButton.style.backgroundColor = new Color(0.2f, 0.6f, 0.2f, 1f);
            openEditorButton.style.color = new Color(1f, 1f, 1f, 1f);
            openEditorButton.style.fontSize = 22f;
            openEditorButton.style.marginTop = 20f;
            
            openEditorButton.text = "Open in Editor";
            openEditorButton.style.height = 50f;
            
            container.Add(openEditorButton);
            
            var clearAndSaveButton = new Button(() =>
            {
                var owner = behaviourTreeOwner;
                owner.ClearTree();
                EditorUtility.SetDirty(owner);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            });
            clearAndSaveButton.style.backgroundColor = new Color(0.6f, 0.2f, 0.2f, 1f);
            clearAndSaveButton.style.color = new Color(1f, 1f, 1f, 1f);
            clearAndSaveButton.style.fontSize = 22f;
            clearAndSaveButton.style.marginTop = 5f;
            clearAndSaveButton.style.marginBottom = 5f;
            
            clearAndSaveButton.text = "Clear and Save";
            clearAndSaveButton.style.height = 50f;
            
            container.Add(clearAndSaveButton);
        }
    }
}