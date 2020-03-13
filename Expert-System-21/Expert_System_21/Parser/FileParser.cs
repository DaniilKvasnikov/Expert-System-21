using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Expert_System_21.ExpertSystem;
using Expert_System_21.MyExtensions;

namespace Expert_System_21.Parser
{
    public class FileParser
    {
        private const string PatternFact = @"(^=[A-Z]*(\s)*$)";
        private const string PatternQuerie = @"(^\?[A-Z]*(\s)*$)";

        private const string PatternRule =
            @"(^((\()*(\s)*(!){0,2})*(\s)*[A-Z](\s)*(\))*((\s*[+|^]\s*((\()*(\s)*(!){0,2})*(\s)*[A-Z](\s)*(\))*)*)?\s*(=>|<=>)\s*((\()*(\s)*(!){0,2})*[A-Z](\s)*(\))*((\s*[+]\s*((\()*(\s)*(!){0,2})*(\s)*[A-Z](\s)*(\))*)*)?\s*$)";

        private int _countFact;
        private int _countQuerie;
        private int _countRule;

        public FileParser(IEnumerable<string> lines)
        {
            if (lines == null) throw new Exception("lines must be not null!");
            foreach (var line in lines) AddExpectedResult(line.PreProcess());
            if (_countQuerie == 0) throw new Exception("Queries not found");
        }

        public List<char> Facts { get; } = new List<char>();
        public List<char> Queries { get; } = new List<char>();
        public ArrayList Rules { get; } = new ArrayList();

        private void AddExpectedResult(string line)
        {
            if (Regex.IsMatch(line, PatternRule))
                AddRule(line);
            else if (Regex.IsMatch(line, PatternFact))
                AddFact(line);
            else if (Regex.IsMatch(line, PatternQuerie))
                AddQuerie(line);
            else if (line.Length > 0) throw new Exception(line);
        }

        public void AddRule(string line)
        {
            if (_countQuerie > 0)
                throw new Exception("Queries must come after rules");
            if (_countFact > 0)
                throw new Exception("Facts must come after rules");
            Rules.Add(new ExpertSystemRule(line));
            _countRule++;
        }

        public void AddFact(string line)
        {
            if (_countRule == 0)
                throw new Exception("Rules not found before facts");
            line = line.Replace("=", "");
            Facts.AddRange(line);
            _countFact++;
        }

        public void AddQuerie(string line)
        {
            if (_countFact == 0)
                throw new Exception("Fact not found before queries");
            line = line.Replace("?", "");
            Queries.AddRange(line);
            _countQuerie++;
        }
    }
}