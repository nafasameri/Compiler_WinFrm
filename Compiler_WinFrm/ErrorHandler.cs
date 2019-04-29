using System;
using System.Runtime.Serialization;

namespace Compiler_WinFrm
{
    [Serializable]
    public class ErrorHandler : Exception
    {
        public ErrorHandler() { }

        public ErrorHandler(string message) : base(message) { }

        public ErrorHandler(string message, Exception inner) : base(message, inner) { }

        protected ErrorHandler(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
