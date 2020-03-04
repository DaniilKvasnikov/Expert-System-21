using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ExpertSystemTests.MyExtensions;
using ExpertSystemTests.Parser;

namespace ExpertSystemTests.Parser
{
    public class FileParserWithAnswer : FileParser
    {
        protected List<char> ExpectedTrueResults;
        protected List<char> ExpectedFalseResults;
        protected List<string> ExpectedErrorResult;
        
        
        protected readonly string PatternTrueResults = @"(^(#)(=))";
        protected readonly string PatternFalseResults = @"(^#{1}!{1}={1})";
        protected readonly string PatternErrorResults = @"(^#)";
        
        public FileParserWithAnswer(string[] lines): base(lines)
        {
            ExpectedTrueResults = new List<char>();
            ExpectedFalseResults = new List<char>();
            ExpectedErrorResult = new List<string>();
            
            foreach (var line in lines)
                GetLineType(line);
        }


        protected override void GetLineType(string line)
        {
            base.GetLineType(line);
            bool resExpectedTrueResults = Regex.IsMatch(line, PatternTrueResults);
            bool resExpectedFalseResults = Regex.IsMatch(line, PatternFalseResults);
            bool resExpectedErrorResult = Regex.IsMatch(line, PatternErrorResults);
            if (resExpectedTrueResults)
                AddToCorrectResults(line, ExpectedTrueResults);
            else if (resExpectedFalseResults)
                AddToCorrectResults(line, ExpectedFalseResults);
            else if (resExpectedErrorResult)
                AddToErrorResults(line);
                
        }

        private void AddToErrorResults(string line)
        {
            line = line.ReplaceOneOf("#?=", "");
            ExpectedErrorResult.Add(line);
        }

        private void AddToCorrectResults(string line, List<char> results)
        {
            line = line.ReplaceOneOf("#!?= ", "");
            results.AddRange(line);
        }
    }
}