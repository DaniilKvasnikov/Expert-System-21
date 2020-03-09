using System.Collections.Generic;
using System.Text.RegularExpressions;
using Expert_System_21.MyExtensions;

namespace Expert_System_21.Parser
{
    public class FileParserWithAnswer : FileParser
    {
        private const string PatternTrueResults = @"(^#=)";
        private const string PatternFalseResults = @"(^#!=)";

        public FileParserWithAnswer(string[] lines) : base(lines)
        {
            foreach (var line in lines) AddExpectedResult(line);
        }

        public List<char> ExpectedTrueResults { get; } = new List<char>();
        public List<char> ExpectedFalseResults { get; } = new List<char>();


        private void AddExpectedResult(string line)
        {
            var isMatchExpectedTrueResults = Regex.IsMatch(line, PatternTrueResults);
            var isMatchExpectedFalseResults = Regex.IsMatch(line, PatternFalseResults);
            line = line.ReplaceOneOf("#!?= ", "");
            if (isMatchExpectedTrueResults)
                ExpectedTrueResults.AddRange(line);
            else if (isMatchExpectedFalseResults)
                ExpectedFalseResults.AddRange(line);
        }
    }
}