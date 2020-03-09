using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Expert_System_21.MyExtensions;
using Expert_System_21.Nodes;
using Expert_System_21.Parser;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;
using Node = Microsoft.Msagl.Drawing.Node;

namespace Expert_System_21.Visualizer
{
    public class GraphVisualizer
    {
        private readonly Graph _graph;

        private readonly Dictionary<System.Type, NodeInfoVisualise> _nodeInfoVisualises =
            new Dictionary<System.Type, NodeInfoVisualise>
            {
                {typeof(AtomNode), new NodeInfoVisualise(Shape.Circle)},
                {typeof(ConnectorNode), new NodeInfoVisualise(Shape.Diamond)},
                {typeof(NegativeNode), new NodeInfoVisualise(Shape.Box)}
            };

        private readonly Dictionary<string, Node> _nodes = new Dictionary<string, Node>();
        private readonly FileParser _parser;

        public GraphVisualizer(ExpertSystemTree tree, FileParser parser)
        {
            var form = new Form();
            var viewer = new GViewer();
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

        private Color FactColor { get; } = Color.Gold;
        private Color TrueStateColor { get; } = Color.Green;
        private Color FalseStateColor { get; } = Color.Red;

        private NodeInfoVisualise DefaultNodeInfo { get; } = new NodeInfoVisualise(Color.Black, Shape.Circle);

        private void AddToGraph(Nodes.Node atom)
        {
            if (atom.Visited) return;
            atom.Visited = true;
            foreach (var atomParent in atom.OperandParents)
                AddToGraph(atomParent);
            AddNodes(atom, atom.Children);
            if (atom.GetType() == typeof(ConnectorNode))
                AddNodes(atom, ((ConnectorNode) atom).Operands);
            atom.Visited = false;
        }

        private void AddNodes(Nodes.Node currentNode, List<Nodes.Node> nodes)
        {
            foreach (var node in nodes)
            {
                var source = AddNode(currentNode);
                var target = AddNode(node);
                AddEdge(source, target);
                AddToGraph(node);
            }
        }

        private Node AddNode(Nodes.Node node)
        {
            var name = node.ToString();
            var nodeInfoVisualise = DefaultNodeInfo;
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

            var isFact = node.GetType() == typeof(AtomNode) && _parser.Facts.Contains(((AtomNode) node).Name);
            return AddNode(name, nodeInfoVisualise, isFact);
        }

        private Node AddNode(string name, NodeInfoVisualise nodeInfoVisualise, bool isFact)
        {
            if (_nodes.ContainsKey(name))
                return _nodes[name];
            var nodeGraph = _graph.AddNode(name);
            nodeGraph.Attr.Copy(nodeInfoVisualise);
            if (isFact)
                nodeGraph.Attr.Color = FactColor;
            _nodes.Add(name, nodeGraph);
            return nodeGraph;
        }

        private void AddEdge(Node source, Node target)
        {
            if (_graph.Edges.Any(edge => edge.Source == source.Id && edge.Target == target.Id))
                return;
            var newEdge = _graph.AddEdge(source.Id, target.Id);
            newEdge.Attr.ArrowheadAtTarget = ArrowStyle.None;
            newEdge.Attr.ArrowheadAtSource = ArrowStyle.Normal;
        }
    }
}