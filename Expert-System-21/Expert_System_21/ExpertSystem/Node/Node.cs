﻿using System;
using System.Collections.Generic;
using Expert_System_21.ExpertSystem.Types;
using Expert_System_21.Logs;
using Expert_System_21.MyExtensions;

namespace Expert_System_21.ExpertSystem.Node
{
    public class Node
    {
        public bool Visited;

        public Node()
        {
            Visited = false;
            State = false;
            StateFixed = false;
        }

        public List<Node> Children { get; } = new List<Node>();
        public List<Node> OperandParents { get; } = new List<Node>();
        public bool? State { get; protected set; }

        public bool StateFixed { get; private set; }

        public virtual void AddChildren(Node child)
        {
            if (!Children.Contains(child))
                Children.Add(child);
        }

        public virtual void SetState(bool? status, bool isFixed)
        {
            if (StateFixed && isFixed && State != null && State != status)
                throw new Exception("[Conflict] " + ToString() + " two different states");

            if (State != status)
                Logger.LogString(string.Format("{0} задан как {1}", this, status.ForPrint()));
            State = status;
            StateFixed = isFixed;
        }

        public virtual bool? Solve()
        {
            if (Visited)
                return State;

            bool? state = null;
            if (State != null)
            {
                Logger.LogString(string.Format("{0} уже имеет значение {1}", this, State));
                state = State;
                if (StateFixed)
                    return state;
            }

            var fixedNodeList = new List<bool?>();
            var unfixedNodeList = new List<bool?>();

            if (Children.Count > 0)
                Logger.LogString(string.Format("{0} запрашивает ответ у детей {1}", this, string.Join(", ", Children)));
            var (fixedNodeListNew, unfixedNodeListNew) = SolveGroupedNode(Children, false);
            fixedNodeList.AddRange(fixedNodeListNew);
            unfixedNodeList.AddRange(unfixedNodeListNew);

            if (OperandParents.Count > 0)
                Logger.LogString(string.Format("{0} запрашивает ответ у родителей {1}", this,
                    string.Join(", ", OperandParents)));
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

        private (List<bool?> fixedRet, List<bool?> unfixedRet) SolveGroupedNode(List<Node> nodes, bool checkingParents)
        {
            Visited = true;

            var fixedNodeList = new List<bool?>();
            var unfixedNodeList = new List<bool?>();

            foreach (var node in nodes)
            {
                if (checkingParents && node.GetType() == typeof(ConnectorNode) &&
                    ((ConnectorNode) node).Type != ConnectorType.AND)
                    continue;
                var nodeSolve = node.Solve();
                if (GetType() == typeof(NegativeNode) && node.GetType() == typeof(ConnectorNode) &&
                    ((ConnectorNode) node).Type == ConnectorType.IMPLY && !checkingParents)
                    nodeSolve = !nodeSolve;
                if (nodeSolve != null && node.StateFixed)
                    fixedNodeList.Add(nodeSolve);
                else if (nodeSolve != null)
                    unfixedNodeList.Add(nodeSolve);
            }

            Visited = false;
            return (fixedNodeList, unfixedNodeList);
        }
    }
}