using System;
using System.Collections.Generic;
using Expert_System_21.Type;

namespace Expert_System_21.Nodes
{
    public class ConnectorNode : Node
    {
        public ConnectorNode(ConnectorType type)
        {
            Type = type;
            State = null;
            IsRoot = false;
        }

        public ConnectorType Type { get; }
        public List<Node> Operands { get; } = new List<Node>();
        public bool IsRoot { get; }

        public override void SetState(bool? status, bool isFixed)
        {
            base.SetState(status, isFixed);

            if (Type != ConnectorType.AND || status != true) return;
            foreach (var operand in Operands)
                operand.SetState(true, isFixed);
        }

        public void AddOperand(Node operand)
        {
            if (Type == ConnectorType.IMPLY && Operands.Count > 0)
                throw new Exception("An imply connection must only have one operand");
            Operands.Add(operand);
            if (IsRoot || Type == ConnectorType.IMPLY || operand.OperandParents.Contains(this))
                return;
            operand.OperandParents.Add(this);
        }


        public void AddOperands(Node[] newOperands)
        {
            foreach (var operand in newOperands)
                AddOperand(operand);
        }

        public override bool? Solve()
        {
            if (Visited)
                return State;
            Visited = true;
            if (Type == ConnectorType.IMPLY)
            {
                var state = Operands[0].Solve();
                SetState(state, Operands[0].StateFixed);
                Visited = false;
                return state;
            }

            bool? operandsResult = null;
            var foundNone = false;
            var hasFixedOperands = false;

            foreach (var operand in Operands)
            {
                var operandResult = operand.Solve();
                hasFixedOperands |= operand.StateFixed;
                foundNone |= operandResult == null;
                if (operandResult == null)
                    continue;
                if (operandsResult == null)
                    operandsResult = operandResult;
                else if (Type == ConnectorType.AND)
                    operandsResult &= operandResult;
                else if (Type == ConnectorType.OR)
                    operandsResult |= operandResult;
                else if (Type == ConnectorType.XOR)
                    operandsResult ^= operandResult;
            }

            Visited = false;
            if (foundNone && (Type == ConnectorType.OR && operandsResult == false ||
                              Type == ConnectorType.AND && operandsResult == true ||
                              Type == ConnectorType.XOR))
                return null;
            if (operandsResult != null)
            {
                SetState(operandsResult, hasFixedOperands);
                return operandsResult;
            }

            return base.Solve();
        }

        public override string ToString()
        {
            var str = "";
            str = string.Join(((char) Type).ToString(), Operands);
            if (Type == ConnectorType.IMPLY)
                str += (char) Type;
            return str;
        }
    }
}