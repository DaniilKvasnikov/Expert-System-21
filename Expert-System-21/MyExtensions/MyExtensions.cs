namespace ExpertSystemTests.MyExtensions
{
    public static class MyExtensions
    {
        public static string DeleteComment(this string input)
        {
            int sharp = input.IndexOf('#');
            if (sharp != -1)
            {
                input = input.Substring(0, input.IndexOf('#'));
            }
            return input;
        }
        
        public static string DeleteTabs(this string input)
        {
            return input.Replace(" ", "").Replace("\t", "");
        }
        
        public static string PostProcess(this string input)
        {
            return input.DeleteTabs().DeleteComment();
        }

        public static string ReplaceOneOf(this string input, string oldValue, string newValue)
        {
            foreach (var ch in oldValue)
            {
                input = input.Replace(ch.ToString(), newValue);
            }

            return input;
        }
    }
}