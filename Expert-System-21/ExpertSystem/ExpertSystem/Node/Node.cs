using System.Collections;
using System.Collections.Generic;
using Expert_System_21.Type;

namespace Expert_System_21.Nodes
{
    public class Node
    {
        public ArrayList Children { get; } = new ArrayList();
        public ArrayList OperandParents { get; } = new ArrayList();
        public bool Visited;
        protected bool? State;
        private bool _stateFixed;

        public bool StateFixed => _stateFixed;

        public Node()
        {
            Visited = false;
            State = false;
            _stateFixed = false;
        }

        public virtual void AddChildren(Node child)
        {
            if (!Children.Contains(child))
                Children.Add(child);
        }

        public virtual void SetState(bool? status, bool isFixed)
        {
            State = status;
            _stateFixed = isFixed;
        }

        public virtual bool? Solve()
        {
            if (Visited)
                return State;

            bool? state = null;
            if (State != null)
            {
                state = State;
                if (_stateFixed)
                    return state;
            }
            
            var fixedNodeList = new List<bool?>();
            var unfixedNodeList = new List<bool?>();

            var (fixedNodeListNew, unfixedNodeListNew) = SolveGroupedNode(Children, false);
            fixedNodeList.AddRange(fixedNodeListNew);
            unfixedNodeList.AddRange(unfixedNodeListNew);

            SolveGroupedNode(OperandParents, true);

            var nodeList = fixedNodeList.Count != 0 ? fixedNodeList : unfixedNodeList;
            if (nodeList.Count != 0)
                state = nodeList.Contains(true);

            var isFixed = fixedNodeList.Count != 0;

            var needReverse = true;
            if (state == null)
            {
                needReverse = false;
                state = State;
            }

            if (state == null) return null;
            if (GetType() == typeof(NegativeNode) && needReverse)
                state = !state;
            SetState(state, isFixed);
            return state;
        }

        private (List<bool?> fixedRet, List<bool?> unfixedRet) SolveGroupedNode(ArrayList nodes, bool checkingParents)
        {
            Visited = true;

            var fixedNodeList = new List<bool?>();
            var unfixedNodeList = new List<bool?>();

            foreach (Node node in nodes)
            {
                if (checkingParents && node.GetType() == typeof(ConnectorNode) && ((ConnectorNode) node).Type != ConnectorType.AND)
                    continue;
                var nodeSolve = node.Solve();
                if (GetType() == typeof(NegativeNode) && node.GetType() == typeof(ConnectorNode) && ((ConnectorNode) node).Type == ConnectorType.IMPLY && !checkingParents)
                    nodeSolve = !nodeSolve;
                if (nodeSolve != null && node._stateFixed)
                    fixedNodeList.Add(nodeSolve);
                else if (nodeSolve != null)
                    unfixedNodeList.Add(nodeSolve);
            }

            Visited = false;
            return (fixedNodeList, unfixedNodeList);
        }
    }
}