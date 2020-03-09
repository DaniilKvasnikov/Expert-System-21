namespace Expert_System_21.Nodes
{
    public class AtomNode: Node
    {
        private string _name;

        public AtomNode(string name)
        {
            _name = name;
        }
        
        public static bool IsAtom(char ch)
        {
            return 'A' <= ch && ch <= 'Z';
        }

        public override string ToString()
        {
            return _name;
        }
    }
}