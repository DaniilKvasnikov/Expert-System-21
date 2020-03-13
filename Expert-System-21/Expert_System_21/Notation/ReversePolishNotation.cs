using System;
using System.Collections.Generic;
using System.Linq;

namespace Expert_System_21.Notation
{
    public class ReversePolishNotation
    {
        private readonly Dictionary<CharType, Dictionary<char, int>> _dicts =
            new Dictionary<CharType, Dictionary<char, int>>();

        private readonly Stack<char> _stack = new Stack<char>();

        public ReversePolishNotation()
        {
            var facts = Enumerable
                .Range('A', 'Z' - 'A' + 1)
                .ToDictionary(i => (char) i, i => 1);
            _dicts.Add(CharType.Fact, facts);

            var operations = new Dictionary<char, int>
            {
                {'+', 3},
                {'|', 2},
                {'^', 1}
            };
            _dicts.Add(CharType.Operation, operations);

            var prefixOperations = new Dictionary<char, int>
            {
                {'!', 4}
            };
            _dicts.Add(CharType.PrefixOperation, prefixOperations);

            var openingBracket = new Dictionary<char, int>
            {
                {'(', 1}
            };
            _dicts.Add(CharType.OpeningBracket, openingBracket);

            var closingBracket = new Dictionary<char, int>
            {
                {')', 1}
            };
            _dicts.Add(CharType.ClosingBracket, closingBracket);
        }

        public string Convert(string input)
        {
            if (input.Length == 0)
                return input;
            _stack.Clear();
            return GetConvertingString(input);
        }

        private string GetConvertingString(string input)
        {
            var convertingResult = "";
            while (input.Length > 0)
            {
                var c = GetNextChar(ref input, out var type);
                switch (type)
                {
                    case CharType.Fact:
                        convertingResult += c;
                        break;
                    case CharType.Operation:
                        while (_stack.Count > 0 && CheckOperation(_stack.Peek(), c)) convertingResult += _stack.Pop();
                        _stack.Push(c);
                        break;
                    case CharType.PrefixOperation:
                    case CharType.OpeningBracket:
                        _stack.Push(c);
                        break;
                    case CharType.ClosingBracket:
                        convertingResult += OperationClosing();
                        break;
                }
            }
            convertingResult += string.Join("", _stack);
            return convertingResult;
        }

        private bool CheckOperation(char peek, char curr)
        {
            var isPrefix = _dicts[CharType.PrefixOperation].ContainsKey(peek);
            var isMorePriorityOperation = _dicts[CharType.Operation].ContainsKey(peek) &&
                                          _dicts[CharType.Operation].ContainsKey(curr) &&
                                          _dicts[CharType.Operation][peek] > _dicts[CharType.Operation][curr];
            return isPrefix || isMorePriorityOperation;
        }

        public char GetNextChar(ref string input, out CharType charType)
        {
            if (input == null)
                throw new Exception("ошибка при чтении null");
            while (input.Length > 0)
            {
                var c = input[0];
                input = input.Remove(0, 1);
                charType = GetType(c);
                return c;
            }

            throw new Exception("ошибка при чтении");
        }

        private string OperationClosing()
        {
            var substring = "";
            while (_stack.Count > 0)
            {
                var elem = _stack.Pop();
                if (_dicts[CharType.OpeningBracket].Keys.Contains(elem))
                    return substring;
                substring += elem;
            }

            throw new Exception("( - not found!");
        }

        public CharType GetType(char c)
        {
            var res = _dicts.FirstOrDefault(dick => dick.Value.ContainsKey(c));
            if (res.Value == null)
                throw new Exception("неверный символ " + c);
            return res.Key;
        }
    }
}