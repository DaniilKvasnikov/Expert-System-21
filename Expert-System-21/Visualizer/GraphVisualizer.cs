using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Expert_System_21.MyExtensions;
using Expert_System_21.Nodes;
using Expert_System_21.Parser;
using Microsoft.Msagl.Drawing;
using Node = Expert_System_21.Nodes.Node;

namespace Expert_System_21.Visualizer
{
    public class GraphVisualizer
    {
        private readonly Dictionary<System.Type, NodeInfoVisualise> _nodeInfoVisualises = new Dictionary<System.Type, NodeInfoVisualise>()
        {
            {typeof(AtomNode), new NodeInfoVisualise(Shape.Circle)},
            {typeof(ConnectorNode), new NodeInfoVisualise(Shape.Diamond)},
            {typeof(NegativeNode), new NodeInfoVisualise(Shape.Box)},
        };

        private Color FactColor { get; } = Color.Gold;
        private Color TrueStateColor { get; } = Color.Green;
        private Color FalseStateColor { get; } = Color.Red;

        private NodeInfoVisualise DefaultNodeInfo { get; } = new NodeInfoVisualise(Color.Black, Shape.Circle);
        private readonly Dictionary<string, Microsoft.Msagl.Drawing.Node> _nodes = new Dictionary<string, Microsoft.Msagl.Drawing.Node>();
        private readonly Graph _graph;
        private readonly FileParser _parser;
        public GraphVisualizer(ExpertSystemTree tree, FileParser parser)
        {
            Form form = new Form();
            Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            _parser = parser;
            _graph = new Graph("graph");
            foreach (var atom in tree.Atoms)
                AddToGraph(atom.Value);
            viewer.Graph = _graph;
            form.SuspendLayout();
            viewer.Dock = DockStyle.Fill;
            viewer.ToolBarIsVisible = false;
            form.Controls.Add(viewer);
            form.ResumeLayout();
            form.ShowDialog();
        }

        private void AddToGraph(Node atom)
        {
            if (atom.Visited) return;
            atom.Visited = true;
            foreach (Node atomParent in atom.OperandParents)
                AddToGraph(atomParent);
            AddNodes(atom, atom.Children);
            if (atom.GetType() == typeof(ConnectorNode))
                AddNodes(atom, ((ConnectorNode) atom).Operands);
            atom.Visited = false;
        }

        private void AddNodes(Node currentNode, List<Node> nodes)
        {
            foreach (Node node in nodes)
            {
                var source = AddNode(currentNode);
                var target = AddNode(node);
                AddEdge(source, target);
                AddToGraph(node);
            }
        }

        private Microsoft.Msagl.Drawing.Node AddNode(Node node)
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

            bool isFact = node.GetType() == typeof(AtomNode) && _parser.Facts.Contains(((AtomNode) node).Name);
            return AddNode(name, nodeInfoVisualise, isFact);
        }

        private Microsoft.Msagl.Drawing.Node AddNode(string name, NodeInfoVisualise nodeInfoVisualise, bool isFact)
        {
            if (_nodes.ContainsKey(name))
                return _nodes[name];
            Microsoft.Msagl.Drawing.Node nodeGraph = _graph.AddNode(name);
            nodeGraph.Attr.Copy(nodeInfoVisualise);
            if (isFact)
                nodeGraph.Attr.Color = FactColor;
            _nodes.Add(name, nodeGraph);
            return nodeGraph;
        }

        private void AddEdge(Microsoft.Msagl.Drawing.Node source, Microsoft.Msagl.Drawing.Node target)
        {
            if (_graph.Edges.Any(edge => edge.Source == source.Id && edge.Target == target.Id))
                return;
            var newEdge = _graph.AddEdge(source.Id, target.Id);
            newEdge.Attr.ArrowheadAtTarget  = ArrowStyle.None;
            newEdge.Attr.ArrowheadAtSource  = ArrowStyle.Normal;
        }
    }
}