using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ExpertSystemTests.ExpertSystem;
using ExpertSystemTests.MyExtensions;
using ExpertSystemTests.Notation;

namespace ExpertSystemTests.Parser
{
    public class FileParser
    {
        public List<char> Facts{ get; }
        public List<char> Queries { get; }
        public ArrayList Rules { get; }

        protected readonly string PatternFact = @"(^=[A-Z]*(\s)*$)";
        protected readonly string PatternQuerie = @"(^\?[A-Z]*(\s)*$)";
        protected readonly string PatternRule = @"(^((\()*(\s)*(!){0,2})*(\s)*[A-Z](\s)*(\))*((\s*[+|^]\s*((\()*(\s)*(!){0,2})*(\s)*[A-Z](\s)*(\))*)*)?\s*(=>|<=>)\s*((\()*(\s)*(!){0,2})*[A-Z](\s)*(\))*((\s*[+]\s*((\()*(\s)*(!){0,2})*(\s)*[A-Z](\s)*(\))*)*)?\s*$)";

        protected int CountFact;
        protected int CountQuerie;
        protected int CountRule;
        
        public FileParser(string[] lines)
        {
            
            Facts = new List<char>();
            Queries = new List<char>();
            Rules = new ArrayList();
            
            foreach (var line in lines)
                GetLineType(line);
        }

        private void GetLineType(string line)
        {
            line = line.PostProcess();
            bool resFact = Regex.IsMatch(line, PatternFact);
            bool resQuerie = Regex.IsMatch(line, PatternQuerie);
            bool resRule = Regex.IsMatch(line, PatternRule);
            if (resRule)
            {
                if (CountFact > 0)
                    throw new Exception("Facts must come after rules");
                if (CountQuerie > 0)
                    throw new Exception("Queries must come after rules");
                Rules.Add(new ESRule(line));
                CountRule++;
            }
            if (resFact)
            {
                if (CountRule == 0)
                    throw new Exception("Rules not found before facts");
                line = line.Replace("=", "").Replace(" ", "");
                foreach (var ch in line)
                    Facts.Add(ch);
                CountFact++;
            }

            if (resQuerie)
            {
                line = line.Replace("?", "").Replace(" ", "");
                foreach (var ch in line)
                    Queries.Add(ch);
                CountQuerie++;
            }
        }
    }
}