using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpertSystemTests.Notation
{
	public enum CharType
	{
		Fact,
		Operation,
		PrefixOperation,
		OpeningBracket,
		ClosingBracket,
		StringEnd,
		Error,
	}
	
	public class ReversePolishNotation
	{
		private readonly Dictionary<CharType, Dictionary<char, int>> _dicts = new Dictionary<CharType, Dictionary<char, int>>();
		private readonly Stack<char> _stack = new Stack<char>();
		
		public ReversePolishNotation()
		{
			Dictionary<char, int> facts = Enumerable
				.Range('A', 'Z' - 'A' + 1)
				.ToDictionary((i) => (char) i, (i) => 1);
			_dicts.Add(CharType.Fact, facts);
			
			Dictionary<char, int> operations = new Dictionary<char, int>();
			operations.Add('+', 3);
			operations.Add('|', 2);
			operations.Add('^', 1);
			_dicts.Add(CharType.Operation, operations);
			
			Dictionary<char, int> prefixOperations = new Dictionary<char, int>();
			prefixOperations.Add('!', 4);
			_dicts.Add(CharType.PrefixOperation, prefixOperations);
			
			Dictionary<char, int> openingBracket = new Dictionary<char, int>();
			openingBracket.Add('(', 1);
			_dicts.Add(CharType.OpeningBracket, openingBracket);
			
			Dictionary<char, int> closingBracket = new Dictionary<char, int>();
			closingBracket.Add(')', 1);
			_dicts.Add(CharType.ClosingBracket, closingBracket);
		}
		
		public string Convert(string input)
		{
			char c;
			string result = "";
			if (input.Length == 0)
			{
				return input;
			}
			_stack.Clear();
			while (input.Length > 0)
			{
				c = get_next_char(input, out input, out CharType type);
				switch(type)
				{
					case CharType.Fact:
						result += c;
						break;
					case CharType.Operation:
						while (_stack.Count > 0 && check_operation(_stack.Peek(), c))
						{
							result += _stack.Pop();
						}
						_stack.Push(c);
						break;
					case CharType.PrefixOperation:
					case CharType.OpeningBracket:
						_stack.Push(c);
						break;
					case CharType.ClosingBracket:
						result += operation_closing(c, CharType.OpeningBracket);
						break;
					case CharType.StringEnd:
						while (_stack.Count > 0)
						{
							result += _stack.Pop();
						}
						return result;
					case CharType.Error:
						throw new Exception("Error converting string");
					default:
						throw new Exception("Unknown type");
				}
			}

			while (_stack.Count > 0)
			{
				result += _stack.Pop();
			}
			return result;
		}

		private bool check_operation(char peek, char curr)
		{
			try
			{
				return _dicts[CharType.PrefixOperation].ContainsKey(peek)
				       || _dicts[CharType.Operation][peek] > _dicts[CharType.Operation][curr];
			}
			catch (KeyNotFoundException e)
			{
				return false;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}

		private char get_next_char(string input, out string output, out CharType charType)
		{
			var c = (char) 0;
			output = input;
			charType = CharType.StringEnd;
			if (input.Length == 0) return c;
			while (input.Length > 0)
			{
				c = input[0];
				input = input.Remove(0, 1);
				if (check_char(c))
				{
					output = input;
					charType = get_type(c);
					return c;
				}
			}
			return c;
		}

		private string operation_closing(char c, CharType openingBracket)
		{
			string substring = "";
			while (_stack.Count > 0)
			{
				char elem = _stack.Pop();
				if (_dicts[openingBracket].Keys.Contains(elem))
					return substring;
				substring += elem;
			}
			throw new Exception("( - not found!");
		}

		private bool check_char(char c)
		{
			return get_type(c) != CharType.Error;
		}

		private CharType get_type(char c)
		{
			foreach (var dick in _dicts)
			{
				if (dick.Value.ContainsKey(c))
					return dick.Key;
			}
			return CharType.Error;
		}

	}
}