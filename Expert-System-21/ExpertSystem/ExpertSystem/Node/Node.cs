using System.Collections;
using System.Collections.Generic;
using Expert_System_21.Type;

namespace Expert_System_21.Nodes
{
    public class Node
    {
        protected ArrayList children;
        public ArrayList operand_parents;
        protected bool visited;
        public bool? state;
        protected bool stateFixed;
        protected ESTree tree;
        
        public bool StateFixed
        {
            get => stateFixed;
            set => stateFixed = value;
        }

        public Node(ESTree tree)
        {
            children = new ArrayList();
            operand_parents = new ArrayList();
            visited = false;
            state = false;
            stateFixed = false;
            this.tree = tree;
        }

        public virtual void AddChildren(Node child)
        {
            if (!children.Contains(child))
                children.Add(child);
        }

        public virtual void SetState(bool? status, bool isFixed)
        {
            state = status;
            stateFixed = isFixed;
        }

        public virtual bool? Solve()
        {
            if (visited)
                return this.state;

            bool? state = null;
            if (this.state != null)
            {
                state = this.state;
                if (stateFixed == true)
                    return state;
            }
            
            var fixedRet = new List<bool?>();
            var unfixedRet = new List<bool?>();

            var res = SolveGroupedNode(children, false);
            fixedRet.AddRange(res.fixedRet);
            unfixedRet.AddRange(res.unfixedRet);

            SolveGroupedNode(operand_parents, true);

            var ret = fixedRet.Count != 0 ? fixedRet : unfixedRet;
            if (ret.Count != 0)
                state = ret.Contains(true);

            var isFixed = fixedRet.Count != 0;

            var needReverse = true;
            if (state == null)
            {
                needReverse = false;
                state = this.state;
            }

            if (state != null)
            {
                if (GetType() == typeof(NegativeNode) && needReverse)
                    state = state != null ? !state : null;
                SetState(state, isFixed);
                return state;
            }

            return null;
        }

        private (List<bool?> fixedRet, List<bool?> unfixedRet) SolveGroupedNode(ArrayList nodes, bool checkingParents)
        {
            visited = true;

            var fixedRes = new List<bool?>();
            var unfixedList = new List<bool?>();

            foreach (var node in nodes)
            {
                if (checkingParents && node.GetType() == typeof(ConnectorNode) && ((ConnectorNode) node).Type != ConnectorType.AND)
                    continue;
                var r = ((Node) node).Solve();
                if (GetType() == typeof(NegativeNode) && node.GetType() == typeof(ConnectorNode) && ((ConnectorNode) node).Type == ConnectorType.IMPLY && !checkingParents)
                    r = (r != null) ? !r : null;
                if (r != null && ((Node) node).stateFixed)
                    fixedRes.Add(r);
                else if (r != null)
                    unfixedList.Add(r);
            }

            visited = false;
            return (fixedRes, unfixedList);
        }
    }
}