using System;

namespace ExpertSystemTests.ExpertSystem
{
    public class NegativeNode: Node
    {
        public NegativeNode(Node child) : base(null)
        {
            if (child == null)
                throw new Exception("NegativeNode must have one child!");

            state = null;
            AddChildren(child);
        }

        public override void AddChildren(Node child)
        {
            base.AddChildren(child);
            child.operand_parents.Add(this);
        }
        
        public override void SetState(bool? status, bool isFixed)
        {
            base.SetState(status, isFixed);
            var res = status;
            ((Node)children[0]).SetState(res != null ? !res : null, isFixed);
        }
    }
}