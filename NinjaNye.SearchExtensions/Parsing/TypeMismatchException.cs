using System;

namespace NinjaNye.SearchExtensions.Parsing
{
    public class TypeMismatchException : Exception
    {
        public TypeMismatchException(string message): base (message) { }
    }
}