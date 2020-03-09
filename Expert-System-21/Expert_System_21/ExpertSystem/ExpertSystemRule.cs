using System;
using System.Collections.Generic;
using System.Linq;
using Expert_System_21.Nodes;
using Expert_System_21.Notation;
using Expert_System_21.Type;

namespace Expert_System_21.ExpertSystem
{
    public class ExpertSystemRule
    {
        public ExpertSystemRule(string line)
        {
            Type = line.Contains("<=>") ? ImplicationType.EQUAL : ImplicationType.IMPLY;
            var notation = new ReversePolishNotation();
            string[] separator = {"=>", "<=>"};
            var lines = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length != 2)
                throw new Exception("ESRule error convert! " + line);
            NpiLeft = notation.Convert(lines[0]);
            NpiRight = notation.Convert(lines[1]);
            if (NpiRight.Contains("+!"))
                throw new Exception("Format error!(right +!) " + line);
            if (Type == ImplicationType.EQUAL && (NpiLeft.Contains('|') || NpiRight.Contains('|')))
                throw new Exception("Format error!(| and <=>) " + line);
            if (Type == ImplicationType.EQUAL && (NpiLeft.Contains("+!") || NpiRight.Contains("+!")))
                throw new Exception("Format error!(+! and <=>) " + line);
        }

        public ImplicationType Type { get; }
        public string NpiLeft { get; }
        public string NpiRight { get; }

        public List<char> GetAtomsPart(string part)
        {
            var atoms = new List<char>();
            foreach (var atom in part.Where(atom => AtomNode.IsAtom(atom) && !atoms.Contains(atom)))
                atoms.Add(atom);
            return atoms;
        }
    }
}