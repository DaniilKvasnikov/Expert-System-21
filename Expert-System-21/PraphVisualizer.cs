using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Expert_System_21.Nodes;
using Microsoft.Msagl.Core.Geometry;
using Microsoft.Msagl.Core.Geometry.Curves;
using Microsoft.Msagl.Drawing;
using Node = Expert_System_21.Nodes.Node;

namespace Expert_System_21
{
    public class PraphVisualizer
    {
        private Dictionary<System.Type, Color> colors = new Dictionary<System.Type, Color>()
        {
            {typeof(AtomNode), Color.Green},
            {typeof(ConnectorNode), Color.Red},
            {typeof(NegativeNode), Color.Blue},
        };
        public Color DefaultColor { get; } = Color.Black;
        private Dictionary<string, Microsoft.Msagl.Drawing.Node> _nodes = new Dictionary<string, Microsoft.Msagl.Drawing.Node>();
        public PraphVisualizer(ESTree tree, Dictionary<char, bool?> result)
        {
            //create a form 
            Form form = new Form();
            //create a viewer object 
            Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            //create a graph object 
            Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("graph");

            //create the graph content 
            foreach (var atom in tree.Atoms)
            {
                AddToGraph(graph, atom.Value);
            }
            
            // foreach (var connector in tree.Connectors)
            // {
            //     foreach (Nodes.Node nodeConnector in connector.Operands)
            //     {
            //         graph.AddEdge(connector.ToString(), nodeConnector.ToString());
            //     }
            // }
            //
            // foreach (var implication in tree.Implication)
            // {
            //     graph.AddEdge(implication.Left.ToString(), implication.Right.ToString());
            // }
            
            // graph.AddEdge("A", "B");
            // graph.AddEdge("B", "C");
            // graph.AddEdge("A", "C").Attr.Color = Microsoft.Msagl.Drawing.Color.Green;
            // graph.FindNode("A").Attr.FillColor = Microsoft.Msagl.Drawing.Color.Magenta;
            // graph.FindNode("B").Attr.FillColor = Microsoft.Msagl.Drawing.Color.MistyRose;
            //
            // Microsoft.Msagl.Drawing.Node c = graph.FindNode("C");
            // c.Attr.FillColor = Microsoft.Msagl.Drawing.Color.PaleGreen;
            // c.Attr.Shape = Microsoft.Msagl.Drawing.Shape.Diamond;
				
            //bind the graph to the viewer 
            viewer.Graph = graph;
            //associate the viewer with the form 
            form.SuspendLayout();
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            form.Controls.Add(viewer);
            form.ResumeLayout();
            //show the form 
            form.ShowDialog();
        }

        private void AddToGraph(Graph graph, Node atom)
        {
            if (atom.Visited) return;
            atom.Visited = true;
            foreach (Node atomChild in atom.Children)
            {
                var source = AddNode(graph, atom);
                var target = AddNode(graph, atomChild);
                AddEdge(graph, source, target);
                AddToGraph(graph, atomChild);
            }
            foreach (Node atomParent in atom.OperandParents)
            {
                var source = AddNode(graph, atomParent);
                var target = AddNode(graph, atom);
                AddEdge(graph, source, target);
                AddToGraph(graph, atomParent);
            }

            if (atom.GetType() == typeof(ConnectorNode))
            {
                var connectorNode = (ConnectorNode) atom;
                foreach (Node operand in connectorNode.Operands)
                {
                    var source = AddNode(graph, connectorNode);
                    var target = AddNode(graph, operand);
                    AddEdge(graph, source, target);
                    AddToGraph(graph, operand);
                }
            }
            atom.Visited = false;
        }

        private Microsoft.Msagl.Drawing.Node AddNode(Graph graph, Node node)
        {
            string name = node.ToString();
            Color color = Color.White;
            if (colors.ContainsKey(node.GetType()))
                color = colors[node.GetType()];
            else
            {
                color = DefaultColor;
            }
            return AddNode(graph, name, color);
        }

        private Microsoft.Msagl.Drawing.Node AddNode(Graph graph, string name, Color color)
        {
            if (_nodes.ContainsKey(name))
                return _nodes[name];
            Microsoft.Msagl.Drawing.Node nodeGraph = graph.AddNode(name);
            nodeGraph.Attr.Color = color;
            _nodes.Add(name, nodeGraph);
            return nodeGraph;
        }


        private Microsoft.Msagl.Drawing.Edge AddEdge(Graph graph,
                                                    Microsoft.Msagl.Drawing.Node source,
                                                    Microsoft.Msagl.Drawing.Node target)
        {
            foreach (var edge in graph.Edges)
            {
                if (edge.Source == source.Id && edge.Target == target.Id)
                    return edge;
            }
            return graph.AddEdge(source.Id, target.Id);
        }
    }
}