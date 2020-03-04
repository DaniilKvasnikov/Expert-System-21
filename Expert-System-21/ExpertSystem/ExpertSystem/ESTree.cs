using System;
using System.Collections;
using System.Collections.Generic;
using ExpertSystemTests.Parser;

namespace ExpertSystemTests.ExpertSystem
{
    public class ESTree
    {
        private List<char> OPERATORS = new List<char>(){'!', '+', '|', '^', '(', ')'};

        private Dictionary<char, ConnectorType> LST_OP = new Dictionary<char, ConnectorType>()
        {
            ['+'] = ConnectorType.AND,
            ['|'] = ConnectorType.OR,
            ['^'] = ConnectorType.XOR,
        };
        private Dictionary<char, AtomNode> atoms;
        private Stack<ConnectorNode> connectors;
        private List<ImplicationData> implication;
        private ConnectorNode root_node;
        private bool parsed;
        private bool is_root;
        
        public ESTree(FileParser parser)
        {
            atoms = new Dictionary<char, AtomNode>();
            connectors = new Stack<ConnectorNode>();
            implication = new List<ImplicationData>();
            root_node = new ConnectorNode(ConnectorType.AND, this);
            parsed = true;
            is_root = true;

            InitAtomsList(parser.Rules);
            SetAtomsState(parser.Rules, parser.Facts, parser.Queries);
            SetAtomsRelations(parser.Rules);
        }

        private void SetAtomsRelations(ArrayList rules)
        {
            if (atoms.Count == 0)
                throw new Exception("Atoms not found!");
            foreach (ESRule rule in rules)
            {
                Node left = (Node) SetAtomRelationsFromNpi(rule.NpiLeft);
                Node right = (Node) SetAtomRelationsFromNpi(rule.NpiRight);

                ConnectorNode connectorImply = (ConnectorNode) CreateConnector(ConnectorType.IMPLY);
                right.AddChildren(connectorImply);
                connectorImply.AddOperand(left);
                implication.Add(new ImplicationData(left, right));
                if (rule.Type == ImplicationType.EQUAL)
                {
                    ConnectorNode connector_imply_1 = (ConnectorNode) CreateConnector(ConnectorType.IMPLY);
                    left.AddChildren(connector_imply_1);
                    connector_imply_1.AddOperand(right);
                    implication.Add(new ImplicationData(right, left));
                }
            }
        }

        private object CreateConnector(ConnectorType type)
        {
            return new ConnectorNode(type, this);
        }

        private object SetAtomRelationsFromNpi(string ruleNpi)
        {
            var stack = new Stack();

            foreach (var x in ruleNpi)
            {
                if (!OPERATORS.Contains(x))
                    stack.Push(atoms[x]);
                else if (x == '!')
                {
                    Node child = (Node) stack.Pop();
                    var connector_not = new NegativeNode(child);
                    child.operand_parents.Add(connector_not);
                    stack.Push(connector_not);
                }
                else
                {
                    ConnectorNode new_connector;
                    Node pop0 = (Node) stack.Pop();
                    Node pop1 = (Node) stack.Pop();
                    if (pop0.GetType() == typeof(ConnectorNode) && ((ConnectorNode) pop0).Type == LST_OP[x])
                    {
                        ((ConnectorNode) pop0).AddOperand(pop1);
                        new_connector = (ConnectorNode) pop0;
                        connectors.Pop();
                    }
                    else if (pop1.GetType() == typeof(ConnectorNode) && ((ConnectorNode) pop1).Type == LST_OP[x])
                    {
                        ((ConnectorNode) pop0).AddOperand(pop0);
                        new_connector = (ConnectorNode) pop1;
                        connectors.Pop();
                    }
                    else
                    {
                        ConnectorNode connector_x = (ConnectorNode) CreateConnector(LST_OP[x]);
                        connector_x.AddOperands(new Node[]{(Node) pop0, (Node) pop1});
                        new_connector = connector_x;
                    }
                    connectors.Push(new_connector);
                    stack.Push(new_connector);
                }
            }

            return stack.Pop();
        }

        private void SetAtomsState(ArrayList parserRules, List<char> parserFacts, List<char> parserQueries)
        {
            List<char> atoms = new List<char>();
            foreach (ESRule rule in parserRules)
            {
                
                var atomsAdd = new List<char>();
                atomsAdd.AddRange(rule.GetAtomsPart(rule.NpiRight));
                if (rule.Type == ImplicationType.EQUAL)
                    atomsAdd.AddRange(rule.GetAtomsPart(rule.NpiLeft));
                atoms.AddRange(atomsAdd);
            }

            foreach (var atom in atoms)
                SetAtomState(atom, null);
            foreach (var atom in parserFacts)
                SetAtomState(atom, true);
        }

        private void SetAtomState(char atom, bool? value)
        {
            if (!atoms.ContainsKey(atom))
                throw new Exception("Atom not found! " + atom);
            Node node = atoms[atom];
            node.state = value;
            if (value != null && value.Value)
                node.StateFixed = true;
        }

        private void InitAtomsList(ArrayList parserRules)
        {
            foreach (ESRule rule in parserRules)
            {
                var atomsAdd = new List<char>();
                atomsAdd.AddRange(rule.GetAtomsPart(rule.NpiLeft));
                atomsAdd.AddRange(rule.GetAtomsPart(rule.NpiRight));
                CreateAtom(atomsAdd);
            }
        }

        private void CreateAtom(List<char> newAtoms)
        {
            foreach (var atom in newAtoms)
            {
                if (!atoms.ContainsKey(atom))
                    atoms.Add(atom, new AtomNode(atom, this));
            }
        }

        public bool? ResolveQuery(char query)
        {
            var atom = atoms[query];
            if (atom == null)
                throw new Exception("Неизвестный атом");
            var res = atom.Solve();
            if (res == null)
            {
                atom.SetState(false, true);
                res = false;
            }
            CheckErrors();
            
            return res;
        }

        private void CheckErrors()
        {
            foreach (var i in implication)
            {
                i.validate();
            }
        }

        public Dictionary<char, bool?> ResolveQuerys(List<char> queries)
        {
            //LINQ
            var results = new Dictionary<char, bool?>();
            foreach (var query in queries)
                results[query] = ResolveQuery(query);
            return results;
        }
    }
}