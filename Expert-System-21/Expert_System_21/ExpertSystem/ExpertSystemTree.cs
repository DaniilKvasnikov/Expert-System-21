using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Expert_System_21.ExpertSystem;
using Expert_System_21.Logs;
using Expert_System_21.Nodes;
using Expert_System_21.Parser;
using Expert_System_21.Type;

namespace Expert_System_21
{
    public class ExpertSystemTree
    {
        private static readonly List<char> Operators = new List<char> {'!', '+', '|', '^', '(', ')'};

        private static readonly Dictionary<char, ConnectorType> ListOperations = new Dictionary<char, ConnectorType>
        {
            ['+'] = ConnectorType.AND,
            ['|'] = ConnectorType.OR,
            ['^'] = ConnectorType.XOR
        };

        public ExpertSystemTree(FileParser parser)
        {
            InitAtomsList(parser.Rules);
            SetStateAtoms(parser.Rules, parser.Facts);
            SetAtomsRelations(parser.Rules);
        }

        public Dictionary<char, AtomNode> Atoms { get; } = new Dictionary<char, AtomNode>();
        public Stack<ConnectorNode> Connectors { get; } = new Stack<ConnectorNode>();
        public List<ImplicationData> Implication { get; } = new List<ImplicationData>();

        private void InitAtomsList(ArrayList rules)
        {
            foreach (ExpertSystemRule rule in rules)
            {
                var allAtoms = new List<char>();
                allAtoms.AddRange(rule.GetAtomsPart(rule.NpiLeft));
                allAtoms.AddRange(rule.GetAtomsPart(rule.NpiRight));
                AddNewAtomsNode(allAtoms);
            }
        }

        private void SetAtomsRelations(ArrayList rules)
        {
            foreach (ExpertSystemRule rule in rules)
            {
                var left = SetAtomRelationsFromRPN(rule.NpiLeft);
                var right = SetAtomRelationsFromRPN(rule.NpiRight);

                var connectorImply = new ConnectorNode(ConnectorType.IMPLY);
                right.AddChildren(connectorImply);
                connectorImply.AddOperand(left);
                Implication.Add(new ImplicationData(left, right));
                if (rule.Type != ImplicationType.EQUAL) continue;
                var connectorEqual = new ConnectorNode(ConnectorType.IMPLY);
                left.AddChildren(connectorEqual);
                connectorEqual.AddOperand(right);
                Implication.Add(new ImplicationData(right, left));
            }
        }

        private Node SetAtomRelationsFromRPN(string rulesRPN)
        {
            var stack = new Stack<Node>();

            foreach (var ruleRPN in rulesRPN)
                if (!Operators.Contains(ruleRPN))
                {
                    stack.Push(Atoms[ruleRPN]);
                }
                else if (ruleRPN == '!')
                {
                    stack.Push(new NegativeNode(stack.Pop()));
                }
                else
                {
                    ConnectorNode newConnector;
                    var atom1 = stack.Pop();
                    var atom2 = stack.Pop();
                    if (atom1.GetType() == typeof(ConnectorNode) &&
                        ((ConnectorNode) atom1).Type == ListOperations[ruleRPN])
                    {
                        ((ConnectorNode) atom1).AddOperand(atom2);
                        newConnector = (ConnectorNode) atom1;
                        Connectors.Pop();
                    }
                    else if (atom2.GetType() == typeof(ConnectorNode) &&
                             ((ConnectorNode) atom2).Type == ListOperations[ruleRPN])
                    {
                        ((ConnectorNode) atom2).AddOperand(atom1);
                        newConnector = (ConnectorNode) atom2;
                        Connectors.Pop();
                    }
                    else
                    {
                        newConnector = new ConnectorNode(ListOperations[ruleRPN]);
                        newConnector.AddOperands(new[] {atom1, atom2});
                    }

                    Connectors.Push(newConnector);
                    stack.Push(newConnector);
                }

            return stack.Pop();
        }

        private void SetStateAtoms(ArrayList rules, List<char> trueFacts)
        {
            var nullStateAtoms = new List<char>();
            foreach (ExpertSystemRule rule in rules)
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
            if (!Atoms.ContainsKey(atom))
                throw new ArgumentNullException("_atoms[atom]");
            Node node = Atoms[atom];
            node.SetState(state, state != null && state.Value);
        }

        private void AddNewAtomsNode(List<char> newAtoms)
        {
            foreach (var atom in newAtoms.Where(atom => !Atoms.ContainsKey(atom))) Atoms.Add(atom, new AtomNode(atom));
        }

        public bool? ResolveQuery(char query)
        {
            Logger.LogString(string.Format("Получаем значение {0}", query));

            if (!Atoms.ContainsKey(query))
                throw new ArgumentNullException("_atoms[atom]");
            var atom = Atoms[query];
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
            Logger.LogString("Выполняем проверку на всех последствиях.");
            foreach (var i in Implication) i.Validate();
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