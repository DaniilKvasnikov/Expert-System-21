using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Expert_System_21.MyExtensions;
using Expert_System_21.Nodes;
using Microsoft.Msagl.Drawing;
using Node = Expert_System_21.Nodes.Node;

namespace Expert_System_21.Visualizer
{
    public class PraphVisualizer
    {
        private readonly Dictionary<System.Type, NodeInfoVisualise> _nodeInfoVisualises = new Dictionary<System.Type, NodeInfoVisualise>()
        {
            {typeof(AtomNode), new NodeInfoVisualise(Shape.Circle)},
            {typeof(ConnectorNode), new NodeInfoVisualise(Shape.Diamond)},
            {typeof(NegativeNode), new NodeInfoVisualise(Shape.Box)},
        };
        
        private Color TrueStateColor = Color.Green;
        private Color FalseStateColor = Color.Red;

        private NodeInfoVisualise DefaultNodeInfo { get; } = new NodeInfoVisualise(Color.Black, Shape.Circle);
        private readonly Dictionary<string, Microsoft.Msagl.Drawing.Node> _nodes = new Dictionary<string, Microsoft.Msagl.Drawing.Node>();
        public PraphVisualizer(ESTree tree, Dictionary<char, bool?> result)
        {
            Form form = new Form();
            Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            Graph graph = new Graph("graph");
            foreach (var atom in tree.Atoms)
                AddToGraph(graph, atom.Value);
            viewer.Graph = graph;
            form.SuspendLayout();
            viewer.Dock = DockStyle.Fill;
            viewer.ToolBarIsVisible = false;
            form.Controls.Add(viewer);
            form.ResumeLayout();
            form.ShowDialog();
        }

        private void AddToGraph(Graph graph, Node atom)
        {
            if (atom.Visited) return;
            atom.Visited = true;
            foreach (Node atomParent in atom.OperandParents)
                AddToGraph(graph, atomParent);
            AddNodes(graph, atom, atom.Children);
            if (atom.GetType() == typeof(ConnectorNode))
                AddNodes(graph, atom, ((ConnectorNode) atom).Operands);
            atom.Visited = false;
        }

        private void AddNodes(Graph graph, Node currentNode, List<Node> nodes)
        {
            foreach (Node node in nodes)
            {
                var source = AddNode(graph, currentNode);
                var target = AddNode(graph, node);
                AddEdge(graph, source, target);
                AddToGraph(graph, node);
            }
        }

        private Microsoft.Msagl.Drawing.Node AddNode(Graph graph, Node node)
        {
            string name = node.ToString();
            NodeInfoVisualise nodeInfoVisualise = DefaultNodeInfo;
            if (_nodeInfoVisualises.ContainsKey(node.GetType()))
                nodeInfoVisualise = _nodeInfoVisualises[node.GetType()];
            switch (node.State)
            {
                case true:
                    nodeInfoVisualise.FillColor = TrueStateColor;
                    break;
                case false:
                    nodeInfoVisualise.FillColor = FalseStateColor;
                    break;
            }
            return AddNode(graph, name, nodeInfoVisualise);
        }

        private Microsoft.Msagl.Drawing.Node AddNode(Graph graph, string name, NodeInfoVisualise nodeInfoVisualise)
        {
            if (_nodes.ContainsKey(name))
                return _nodes[name];
            Microsoft.Msagl.Drawing.Node nodeGraph = graph.AddNode(name);
            nodeGraph.Attr.Copy(nodeInfoVisualise);
            _nodes.Add(name, nodeGraph);
            return nodeGraph;
        }


        private void AddEdge(Graph graph,
            Microsoft.Msagl.Drawing.Node source,
            Microsoft.Msagl.Drawing.Node target)
        {
            if (graph.Edges.Any(edge => edge.Source == source.Id && edge.Target == target.Id))
                return;
            var newEdge = graph.AddEdge(source.Id, target.Id);
            newEdge.Attr.ArrowheadAtTarget  = ArrowStyle.None;
            newEdge.Attr.ArrowheadAtSource  = ArrowStyle.Normal;
        }
    }
}