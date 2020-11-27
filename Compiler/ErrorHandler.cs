using System;
using System.Runtime.Serialization;

namespace Compiler
{
    [Serializable]
    public class ErrorHandler : Exception
    {
        public ErrorHandler(string message, string type) : base(type + " error: " + message) { }
    }
}
