using System;
using System.Collections.Generic;
using System.Linq;
using ExpertSystemTests.Notation;

namespace ExpertSystemTests.ExpertSystem
{
    public class ESRule
    {
        public ImplicationType Type { get; }
        public string NpiLeft { get; private set; }
        public string NpiRight { get; private set; }

        ReversePolishNotation notation;
        public ESRule(string line)
        {
            Type = line.Contains("<=>") ? ImplicationType.EQUAL : ImplicationType.IMPLY;
            notation = new ReversePolishNotation();
            string[] separator = new[] {"=>", "<=>"};
            var lines = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length != 2)
                throw new Exception("ESRule error convert! " + line);
            NpiLeft = notation.Convert(lines[0]);
            NpiRight = notation.Convert(lines[1]);
            if (NpiRight.Contains("+!"))
                throw new Exception("Format error!(right +!) " + line);
            //TODO: check->
            if (Type == ImplicationType.EQUAL && (NpiLeft.Contains('|') || NpiRight.Contains('|')))
                throw new Exception("Format error!(| and <=>) " + line);
            if (Type == ImplicationType.EQUAL && (NpiLeft.Contains("+!") || NpiRight.Contains("+!")))
                throw new Exception("Format error!(+! and <=>) " + line);
                
        }

        public List<char> GetAtomsPart(string part)
        {
            //TODO: LINQ
            var atoms = new List<char>();
            foreach (var ch in part)
                if (AtomNode.IsAtom(ch) && !atoms.Contains(ch))
                    atoms.Add(ch);
            return atoms;
        }
        
    }
}