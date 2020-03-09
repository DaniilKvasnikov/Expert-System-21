using System;
using System.Collections.Generic;
using System.Linq;

namespace Expert_System_21.Notation
{
    public enum CharType
    {
        Fact,
        Operation,
        PrefixOperation,
        OpeningBracket,
        ClosingBracket,
        StringEnd,
        Error
    }

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
            var result = "";
            while (input.Length > 0)
            {
                var c = GetNextChar(input, out input, out var type);
                switch (type)
                {
                    case CharType.Fact:
                        result += c;
                        break;
                    case CharType.Operation:
                        while (_stack.Count > 0 && CheckOperation(_stack.Peek(), c))
                            result += _stack.Pop();
                        _stack.Push(c);
                        break;
                    case CharType.PrefixOperation:
                    case CharType.OpeningBracket:
                        _stack.Push(c);
                        break;
                    case CharType.ClosingBracket:
                        result += OperationClosing();
                        break;
                    case CharType.StringEnd:
                        while (_stack.Count > 0)
                        {
                            c = _stack.Pop();
                            if (c == '(')
                                throw new Exception("Error converting string (");
                            result += c;
                        }

                        return result;
                    case CharType.Error:
                        throw new Exception("Error converting string");
                    default:
                        throw new Exception("Unknown type");
                }
            }

            while (_stack.Count > 0)
                result += _stack.Pop();
            return result;
        }

        private bool CheckOperation(char peek, char curr)
        {
            try
            {
                var isPrefix = _dicts[CharType.PrefixOperation].ContainsKey(peek);
                var isMorePriorityOperation = _dicts[CharType.Operation].ContainsKey(peek) &&
                                              _dicts[CharType.Operation].ContainsKey(curr) &&
                                              _dicts[CharType.Operation][peek] > _dicts[CharType.Operation][curr];
                return isPrefix || isMorePriorityOperation;
            }
            catch (KeyNotFoundException e)
            {
                Console.WriteLine(e);
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private char GetNextChar(string input, out string output, out CharType charType)
        {
            var c = (char) 0;
            output = input;
            charType = CharType.StringEnd;
            while (input.Length > 0)
            {
                c = input[0];
                input = input.Remove(0, 1);
                if (!CheckChar(c)) continue;
                output = input;
                charType = GetType(c);
                return c;
            }

            return c;
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

        private bool CheckChar(char c)
        {
            return GetType(c) != CharType.Error;
        }

        public CharType GetType(char c)
        {
            var res = _dicts.FirstOrDefault(dick => dick.Value.ContainsKey(c));
            return res.Value != null ? res.Key : CharType.Error;
        }
    }
}