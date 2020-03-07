using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Expert_System_21.Nodes;
using Microsoft.Msagl.Core.Geometry;
using Microsoft.Msagl.Core.Geometry.Curves;
using Microsoft.Msagl.Drawing;

namespace Expert_System_21
{
    public class PraphVisualizer
    {
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
                foreach (Nodes.Node nodeAtom in atom.Value.Children)
                {
                    graph.AddEdge(atom.Value.ToString(), nodeAtom.ToString());
                    if (nodeAtom.GetType() == typeof(ConnectorNode))
                    {
                        var connectorNode = (ConnectorNode) nodeAtom;
                        foreach (var operand in connectorNode.Operands)
                        {
                            graph.AddEdge(nodeAtom.ToString(), operand.ToString());
                        }
                    }
                }
            }
            
            foreach (var connector in tree.Connectors)
            {
                foreach (Nodes.Node nodeConnector in connector.Operands)
                {
                    graph.AddEdge(connector.ToString(), nodeConnector.ToString());
                }
            }
            
            foreach (var implication in tree.Implication)
            {
                graph.AddEdge(implication.Left.ToString(), implication.Right.ToString());
            }
            
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
    }
}