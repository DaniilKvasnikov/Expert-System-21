using System;
using Expert_System_21.Nodes;

namespace Expert_System_21
{
    public class ImplicationData
    {
        public ImplicationData(Node left, Node right)
        {
            Left = left;
            Right = right;
        }

        public Node Left { get; }
        public Node Right { get; }

        public void Validate()
        {
            var leftValidate = Left.Solve();
            var rightValidate = Right.Solve();
            if (leftValidate == true && rightValidate == false)
                throw new Exception("[Conflict] " + ToString());
        }
    }
}