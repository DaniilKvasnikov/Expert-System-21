using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ExpertSystemTests.MyExtensions;
using ExpertSystemTests.Parser;

namespace ExpertSystemTests.Parser
{
    public class FileParserWithAnswer : FileParser
    {
        public List<char> ExpectedTrueResults { get; }
        public List<char> ExpectedFalseResults { get; }
        public List<string> ExpectedErrorResult { get; }
        
        
        protected readonly string PatternTrueResults = @"(^#=)";
        protected readonly string PatternFalseResults = @"(^#!=)";
        protected readonly string PatternErrorResults = @"(^#\?=)";
        
        public FileParserWithAnswer(string[] lines): base(lines)
        {
            ExpectedTrueResults = new List<char>();
            ExpectedFalseResults = new List<char>();
            ExpectedErrorResult = new List<string>();
            
            foreach (var line in lines)
                GetLineType(line);
        }


        private void GetLineType(string line)
        {
            bool resExpectedTrueResults = Regex.IsMatch(line, PatternTrueResults);
            bool resExpectedFalseResults = Regex.IsMatch(line, PatternFalseResults);
            bool resExpectedErrorResult = Regex.IsMatch(line, PatternErrorResults);
            if (resExpectedTrueResults)
                ExpectedTrueResults.AddRange(line.ReplaceOneOf("#!?= ", ""));
            else if (resExpectedFalseResults)
                ExpectedFalseResults.AddRange(line.ReplaceOneOf("#!?= ", ""));
            else if (resExpectedErrorResult)
                ExpectedErrorResult.Add(line.ReplaceOneOf("#?=", ""));
        }
    }
}