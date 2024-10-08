using UnityEngine.UIElements;

namespace Batmind.Editor
{
    public class GraphViewContextualMenu
    {
        public static void Build(ContextualMenuPopulateEvent data, TreeGraphView treeGraphView)
        {
            data.menu.ClearItems();
            
            
            data.menu.AppendAction("Select a Node To Add", null, DropdownMenuAction.Status.Disabled);
            data.menu.AppendSeparator();
            
            data.menu.AppendAction("Sequence", (x) => treeGraphView.AddNewSequence());
            data.menu.AppendAction("Selector", (x) => treeGraphView.AddNewSelector());
            
            data.menu.AppendSeparator();
            data.menu.AppendAction("Decorator", (x) => treeGraphView.AddNewDecorator());
            
            data.menu.AppendSeparator();
            data.menu.AppendAction("Action", (x) => treeGraphView.AddNewAction());
            
            data.menu.AppendSeparator();
            data.menu.AppendAction("Condition", (x) => treeGraphView.AddNewCondition());
        }
    }
}