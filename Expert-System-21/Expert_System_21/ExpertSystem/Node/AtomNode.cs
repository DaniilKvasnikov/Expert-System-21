namespace Expert_System_21.Nodes
{
    public class AtomNode: Node
    {
        public char Name { get; }

        public AtomNode(char name)
        {
            Name = name;
        }

        public static bool IsAtom(char ch)
        {
            return 'A' <= ch && ch <= 'Z';
        }

        public override string ToString()
        {
            return Name.ToString();
        }
    }
}