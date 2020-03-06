using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Expert_System_21.ExpertSystem;
using Expert_System_21.Nodes;
using Expert_System_21.Parser;
using Expert_System_21.Type;

namespace Expert_System_21
{
    public class ESTree
    {
        private static readonly List<char> Operators = new List<char>(){'!', '+', '|', '^', '(', ')'};

        private static readonly Dictionary<char, ConnectorType> ListOperations = new Dictionary<char, ConnectorType>()
        {
            ['+'] = ConnectorType.AND,
            ['|'] = ConnectorType.OR,
            ['^'] = ConnectorType.XOR,
        };

        private readonly Dictionary<char, AtomNode> _atoms = new Dictionary<char, AtomNode>();
        private readonly Stack<ConnectorNode> _connectors = new Stack<ConnectorNode>();
        private readonly List<ImplicationData> _implication = new List<ImplicationData>();

        public ESTree(FileParser parser)
        {
            if (parser.Rules.Count == 0)
                throw new Exception("Rules not found");
            if (parser.Queries.Count == 0)
                throw new Exception("Queries not found");
            InitAtomsList(parser.Rules);
            SetStateAtoms(parser.Rules, parser.Facts);
            SetAtomsRelations(parser.Rules);
        }

        private void InitAtomsList(ArrayList rules)
        {
            foreach (ESRule rule in rules)
            {
                var allAtoms = new List<char>();
                allAtoms.AddRange(rule.GetAtomsPart(rule.NpiLeft));
                allAtoms.AddRange(rule.GetAtomsPart(rule.NpiRight));
                AddNewAtomsNode(allAtoms);
            }
        }

        private void SetAtomsRelations(ArrayList rules)
        {
            if (_atoms.Count == 0)
                throw new Exception("Atoms not found!");
            foreach (ESRule rule in rules)
            {
                Node left = SetAtomRelationsFromRPN(rule.NpiLeft);
                Node right = SetAtomRelationsFromRPN(rule.NpiRight);

                var connectorImply = new ConnectorNode(ConnectorType.IMPLY);
                right.AddChildren(connectorImply);
                connectorImply.AddOperand(left);
                _implication.Add(new ImplicationData(left, right));
                if (rule.Type != ImplicationType.EQUAL) continue;
                var connectorEqual = new ConnectorNode(ConnectorType.IMPLY);
                left.AddChildren(connectorEqual);
                connectorEqual.AddOperand(right);
                _implication.Add(new ImplicationData(right, left));
            }
        }

        private Node SetAtomRelationsFromRPN(string rulesRPN)
        {
            var stack = new Stack<Node>();

            foreach (var ruleRPN in rulesRPN)
            {
                if (!Operators.Contains(ruleRPN))
                {
                    stack.Push(_atoms[ruleRPN]);
                }
                else if (ruleRPN == '!')
                {
                    stack.Push(new NegativeNode(stack.Pop()));
                }
                else
                {
                    ConnectorNode newConnector;
                    if (stack.Count < 2)
                        throw new Exception(rulesRPN);
                    Node atom1 = stack.Pop();
                    Node atom2 = stack.Pop();
                    if (atom1.GetType() == typeof(ConnectorNode) && ((ConnectorNode) atom1).Type == ListOperations[ruleRPN])
                    {
                        ((ConnectorNode) atom1).AddOperand(atom2);
                        newConnector = (ConnectorNode) atom1;
                        _connectors.Pop();
                    }
                    else if (atom2.GetType() == typeof(ConnectorNode) && ((ConnectorNode) atom2).Type == ListOperations[ruleRPN])
                    {
                        ((ConnectorNode) atom2).AddOperand(atom1);
                        newConnector = (ConnectorNode) atom2;
                        _connectors.Pop();
                    }
                    else
                    {
                        newConnector = new ConnectorNode(ListOperations[ruleRPN]);
                        newConnector.AddOperands(new[]{atom1, atom2});
                    }
                    _connectors.Push(newConnector);
                    stack.Push(newConnector);
                }
            }

            return stack.Pop();
        }

        private void SetStateAtoms(ArrayList rules, List<char> trueFacts)
        {
            var nullStateAtoms = new List<char>();
            foreach (ESRule rule in rules)
            {
                nullStateAtoms.AddRange(rule.GetAtomsPart(rule.NpiRight));
                if (rule.Type == ImplicationType.EQUAL)
                    nullStateAtoms.AddRange(rule.GetAtomsPart(rule.NpiLeft));
            }

            foreach (var nullStateAtom in nullStateAtoms)
                SetAtomState(nullStateAtom, null);
            foreach (var trueFact in trueFacts)
                SetAtomState(trueFact, true);
        }

        private void SetAtomState(char atom, bool? state)
        {
            if (!_atoms.ContainsKey(atom))
                throw new ArgumentNullException("_atoms[atom]");
            Node node = _atoms[atom];
            node.SetState(state, state != null && state.Value);
        }

        private void AddNewAtomsNode(List<char> newAtoms)
        {
            foreach (var atom in newAtoms.Where(atom => !_atoms.ContainsKey(atom)))
            {
                _atoms.Add(atom, new AtomNode());
            }
        }

        public bool? ResolveQuery(char query)
        {
            if (!_atoms.ContainsKey(query))
                throw new ArgumentNullException("_atoms[atom]");
            var atom = _atoms[query];
            bool? res = atom.Solve();
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
            foreach (var i in _implication)
            {
                i.Validate();
            }
        }

        public Dictionary<char, bool?> ResolveQuerys(List<char> queries)
        {
            var results = new Dictionary<char, bool?>();
            foreach (var query in queries)
                results[query] = ResolveQuery(query);
            return results;
        }
    }
}