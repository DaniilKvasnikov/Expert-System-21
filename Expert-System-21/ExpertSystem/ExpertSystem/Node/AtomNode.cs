namespace Expert_System_21.Nodes
{
    public class AtomNode: Node
    {
        public static bool IsAtom(char ch)
        {
            return 'A' <= ch && ch <= 'Z';
        }
    }
}