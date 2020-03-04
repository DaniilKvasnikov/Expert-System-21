namespace ExpertSystemTests.Preprocessing
{
    public static class StringPreprocessing
    {
        public static string GetString(string input)
        {
            int sharp = input.IndexOf('#');
            if (sharp != -1)
            {
                input = input.Substring(0, input.IndexOf('#'));
            }
            return input.Replace(" ", "").Replace("\t", "");
        }
    }
}