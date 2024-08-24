using System.Collections.Generic;
using System.Linq;
using Batmind.Tree.Nodes.Composites;
using UnityEditor.Experimental.GraphView;

namespace Batmind.Editor.Nodes
{
    public static class NodeViewExtensions
    {
                
        public static List<Edge> GetOrderedEdges(this NodeView nodeView)
        {
            if (nodeView.ImplicitTreeNode is not Composite composite)
            {
                return null;
            }
            
            var connections = nodeView.OutputPort.connections;
            
            switch (composite._EditorOrderMode)
            {
                case Composite.CompositeOrderMode.Priority:
                    return connections.OrderBy(connection => ((NodeView) connection.input.node).ImplicitTreeNode.Priority).ToList();
                    
                case Composite.CompositeOrderMode.TopToBottom:
                    return connections.OrderByDescending(c => c.input.node.GetPosition().y).ToList();                
                    
                case Composite.CompositeOrderMode.LeftToRight:
                default:
                    return connections.OrderBy(c => c.input.node.GetPosition().x).ToList();                
            }
        }

    }
}