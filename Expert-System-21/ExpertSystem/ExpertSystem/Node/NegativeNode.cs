﻿using System;

namespace Expert_System_21.Nodes
{
    public class NegativeNode: Node
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
            ((Node)Children[0]).SetState(!status, isFixed);
        }
    }
}