using System;

namespace Expert_System_21.ExpertSystem
{
    public class ImplicationData
    {
        public ImplicationData(Node.Node left, Node.Node right)
        {
            Left = left;
            Right = right;
        }

        public Node.Node Left { get; }
        public Node.Node Right { get; }

        public void Validate()
        {
            var leftValidate = Left.Solve();
            var rightValidate = Right.Solve();
            if (leftValidate == true && rightValidate == false)
                throw new Exception("[Conflict] " + ToString());
        }
    }
}