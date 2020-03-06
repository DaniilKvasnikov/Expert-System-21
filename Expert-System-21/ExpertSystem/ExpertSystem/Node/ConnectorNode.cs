using System;
using System.Collections.Generic;
using Expert_System_21.Type;

namespace Expert_System_21.Nodes
{
    public class ConnectorNode : Node
    {
        public ConnectorType Type { get; }
        public List<Node> Operands { get; }
        public bool IsRoot { get; }

        public override void SetState(bool? status, bool isFixed)
        {
            base.SetState(status, isFixed);

            if (Type == ConnectorType.AND && status == true)
            {
                foreach (var operand in Operands)
                {
                    operand.SetState(true, isFixed);
                }
            }
        }

        public ConnectorNode(ConnectorType type, ESTree esTree): base(esTree)
        {
            Type = type;
            Operands = new List<Node>();
            state = null;
            IsRoot = false;
        }
        
        public void AddOperand(Node operand)
        {
            if (Type == ConnectorType.IMPLY && Operands.Count > 0)
                throw new Exception("An imply connection must only have one operand");
            Operands.Add(operand);
            if (!IsRoot && Type != ConnectorType.IMPLY && !((Node) operand).operand_parents.Contains(this))
            {
                operand.operand_parents.Add(this);
            }
        }


        public void AddOperands(Node[] newOperands)
        {
            foreach (var operand in newOperands)
                AddOperand(operand);
        }

        public override bool? Solve()
        {
            if (visited)
                return state;
            visited = true;
            if (Type == ConnectorType.IMPLY)
            {
                var ret = Operands[0].Solve();
                SetState(ret, Operands[0].StateFixed);
                visited = false;
                return ret;
            }

            bool? res = null;
            bool found_none = false;
            bool has_fixed_operands = false;

            foreach (var operand in Operands)
            {
                var op_res = operand.Solve();
                if (operand.StateFixed)
                    has_fixed_operands = true;
                if (op_res == null)
                {
                    found_none = true;
                    continue;
                }
                else if (res == null)
                    res = op_res;
                else if (Type == ConnectorType.AND)
                    res &= op_res;
                else if (Type == ConnectorType.OR)
                    res |= op_res;
                else if (Type == ConnectorType.XOR)
                    res ^= op_res;
            }

            visited = false;
            if (found_none && ((Type == ConnectorType.OR && res == false) ||
                               (Type == ConnectorType.AND && res == true) ||
                               (Type == ConnectorType.XOR)))
                return null;
            if (res != null)
            {
                SetState(res, has_fixed_operands);
                return res;
            }

            return base.Solve();
        }
    }
}