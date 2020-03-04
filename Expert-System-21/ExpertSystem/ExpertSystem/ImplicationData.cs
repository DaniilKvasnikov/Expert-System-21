using System;

namespace ExpertSystemTests.ExpertSystem
{
    public class ImplicationData
    {
        private Node left;
        private Node right;

        public ImplicationData(Node left, Node right)
        {
            this.left = left;
            this.right = right;
        }

        public void validate()
        {
            var leftValidate = left.Solve();
            var rightValidate = right.Solve();
            if (leftValidate == true && rightValidate == false)
                throw new Exception("[Conflict] " + this.ToString());
            //TODO: Update exception
        }
    }
}