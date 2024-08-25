using Batmind.Editor.Panels;
using Batmind.Tree;
using Batmind.Tree.Nodes;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Batmind.Editor
{
    public class GraphWindow : EditorWindow
    {
        private BehaviourTreeOwner _target;
        
        private TreeGraphView _treeGraphView;
        private NodeInspectorPanel _nodeInspectorPanel;
        private BlackboardInspectorPanel _blackboardInspectorPanel;
        private BehaviourTree _treeClone;

        private TreeGraphView ConstructGraphView(BehaviourTree tree)
        {
            var graphView = new TreeGraphView(tree)
            {
                name = $"{_target.gameObject.name}",
            };
            
            graphView.StretchToParentSize();

            return graphView;
        }

        private void OnSelectionChanged(NodeView nodeView)
        {
            _nodeInspectorPanel.OnNodeSelected(nodeView);
            _nodeInspectorPanel.visible = true;
        }

        public void SetTarget(BehaviourTreeOwner target, BehaviourTree treeClone)
        {
            _target = target;
            _treeClone = treeClone;
        }

        public void RecreateGUI()
        {
            CreateGUI();
        }
        
        private void CreateGUI()
        {
            if (_treeClone == null)
            {
                return;
            }
            
            _treeGraphView = ConstructGraphView(_treeClone);
            _treeGraphView.SetCallbacks(OnTreeSaved, OnTreeSavedAsAsset, OnSelectionChanged);
            
            rootVisualElement.Clear();
            rootVisualElement.Add(_treeGraphView);
            
            _nodeInspectorPanel = new NodeInspectorPanel();
            rootVisualElement.Add(_nodeInspectorPanel);
            _nodeInspectorPanel.visible = false;

            _blackboardInspectorPanel = new BlackboardInspectorPanel(_treeClone.Blackboard);
            rootVisualElement.Add(_blackboardInspectorPanel);
            _blackboardInspectorPanel.visible = true;
        }

        private void OnTreeSavedAsAsset(BehaviourTree savedTree, string fileName)
        {
            var path = $"Assets/Data/Batmind/{fileName}.asset";
            
            var savedAsset = ScriptableObject.CreateInstance<BehaviourTreeContainer>();
            savedAsset.Set(savedTree);
            
            AssetDatabase.CreateAsset(savedAsset, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.SetDirty(savedAsset);
        }

        private void OnTreeSaved(BehaviourTree savedTree)
        {
            _target.SetTree(savedTree);
            EditorUtility.SetDirty(_target);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}