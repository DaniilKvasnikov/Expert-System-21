namespace Expert_System_21.Nodes
{
    public class AtomNode: Node
    {
        private char name;
        public AtomNode(char atom, ESTree tree): base(tree)
        {
            name = atom;
        }

        public static bool IsAtom(char ch)
        {
            //TODO: do better
            return 'A' <= ch && ch <= 'Z';
        }
    }
}