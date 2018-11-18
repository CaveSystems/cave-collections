#if NETSTANDARD13
namespace System.Data
{
    /// <summary>
    /// Represents the exception that is thrown when you try to change the value of a read-only column.
    /// </summary>
    public class ReadOnlyException : Exception
    {
        public ReadOnlyException()
        {
        }

        public ReadOnlyException(string message) : base(message)
        {
        }

        public ReadOnlyException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
#endif
