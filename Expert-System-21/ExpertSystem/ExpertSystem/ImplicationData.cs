using System;
using Expert_System_21.Exceptions;
using Expert_System_21.Nodes;

namespace Expert_System_21
{
    public class ImplicationData
    {
        private readonly Node _left;
        private readonly Node _right;

        public ImplicationData(Node left, Node right)
        {
            _left = left;
            _right = right;
        }

        public void Validate()
        {
            var leftValidate = _left.Solve();
            var rightValidate = _right.Solve();
            if (leftValidate == true && rightValidate == false)
                throw new NodeConflictException("[Conflict] " + ToString());
        }
    }
}