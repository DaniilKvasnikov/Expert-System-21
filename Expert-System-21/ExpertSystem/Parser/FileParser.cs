using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ExpertSystemTests.ExpertSystem;
using ExpertSystemTests.Notation;
using ExpertSystemTests.Preprocessing;

namespace ExpertSystemTests.Parser
{
    public class FileParser
    {
        public List<char> Facts{ get; }
        public List<char> Queries { get; }
        public ArrayList Rules { get; }

        private string patternFact;
        private string patternQuerie;
        private string patternRule;

        private int countFact;
        private int countQuerie;
        private int countRule;
        
        public FileParser(string[] lines)
        {
            
            Facts = new List<char>();
            Queries = new List<char>();
            Rules = new ArrayList();
            
            patternFact = @"(^=[A-Z]*(\s)*$)";
            patternQuerie = @"(^\?[A-Z]*(\s)*$)";
            patternRule = @"(^((\()*(\s)*(!){0,2})*(\s)*[A-Z](\s)*(\))*((\s*[+|^]\s*((\()*(\s)*(!){0,2})*(\s)*[A-Z](\s)*(\))*)*)?\s*(=>|<=>)\s*((\()*(\s)*(!){0,2})*[A-Z](\s)*(\))*((\s*[+]\s*((\()*(\s)*(!){0,2})*(\s)*[A-Z](\s)*(\))*)*)?\s*$)";

            countFact = 0;
            countQuerie = 0;
            countRule = 0;
            
            foreach (var line in lines)
                GetLineType(StringPreprocessing.GetString(line));
        }

        private void GetLineType(string line)
        {
            bool resFact = Regex.IsMatch(line, patternFact);
            bool resQuerie = Regex.IsMatch(line, patternQuerie);
            bool resRule = Regex.IsMatch(line, patternRule);
            if (resRule)
            {
                if (countFact > 0)
                    throw new Exception("Facts must come after rules");
                if (countQuerie > 0)
                    throw new Exception("Queries must come after rules");
                Rules.Add(new ESRule(line));
                countRule++;
            }
            if (resFact)
            {
                if (countRule == 0)
                    throw new Exception("Rules not found before facts");
                line = line.Replace("=", "").Replace(" ", "");
                foreach (var ch in line)
                    Facts.Add(ch);
                countFact++;
            }

            if (resQuerie)
            {
                line = line.Replace("?", "").Replace(" ", "");
                foreach (var ch in line)
                    Queries.Add(ch);
                countQuerie++;
            }
        }
    }
}