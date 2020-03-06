using System;

namespace Expert_System_21.Exceptions
{
    public class NodeConflictException : Exception
    {
        private string _message;
        public NodeConflictException(string message)
        {
            _message = message;
        }

        public override string ToString()
        {
            return _message;
        }
    }
}