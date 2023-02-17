using System;


namespace СalculateLibrary.Exceptions
{
    public class UnknownTypeException : Exception
    {
        public UnknownTypeException() : base("Unknown Class Type")
        {
            
        }
    }
}