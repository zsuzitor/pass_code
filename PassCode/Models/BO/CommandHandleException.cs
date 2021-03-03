using System;

namespace PassCode.Models.BO
{
    public class CommandHandleException : Exception
    {
        public CommandHandleException() : base()
        {

        }

        public CommandHandleException(string message) : base(message)
        {

        }

        public CommandHandleException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
