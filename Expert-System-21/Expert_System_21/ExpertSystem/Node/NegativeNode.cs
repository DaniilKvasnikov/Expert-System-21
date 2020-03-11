using System;

namespace Expert_System_21.Nodes
{
    public class NegativeNode : Node
    {
        public NegativeNode(Node child)
        {
            if (child == null)
                throw new Exception("NegativeNode must have one child!");

            State = null;
            AddChildren(child);
        }

        public override void AddChildren(Node child)
        {
            base.AddChildren(child);
            child.OperandParents.Add(this);
        }

        public override void SetState(bool? status, bool isFixed)
        {
            base.SetState(status, isFixed);
            Children[0].SetState(!status, isFixed);
        }

        public override string ToString()
        {
            var str = "!";
            foreach (var operand in Children) str += operand.ToString();
            return str;
        }
    }
}