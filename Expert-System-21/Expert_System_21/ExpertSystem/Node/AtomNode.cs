namespace Expert_System_21.ExpertSystem.Node
{
    public class AtomNode : Node
    {
        public AtomNode(char name)
        {
            Name = name;
        }

        public char Name { get; }

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