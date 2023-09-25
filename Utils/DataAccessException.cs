using System.Runtime.Serialization;

namespace Lab2.Utils
{
    [Serializable]
    internal class DataAccessException : Exception
    {
        public DataAccessException()
        {
        }

        public DataAccessException(string? message) : base(message)
        {
        }

        public DataAccessException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DataAccessException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}