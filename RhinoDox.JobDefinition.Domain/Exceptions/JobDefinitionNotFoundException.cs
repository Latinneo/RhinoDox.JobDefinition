using System;

namespace RhinoDox.JobDefinition.Domain.Exceptions
{
    // <summary>
    /// Thrown when a job definition not found exception occurs.
    /// </summary>
    public class JobDefinitionNotFoundException : Exception
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="JobDefinitionNotFoundException"/> class
        /// with the given message.
        /// </summary>
        public JobDefinitionNotFoundException(string message) : base(message)
        {
            // This space intentionally left blank.
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="JobDefinitionNotFoundException"/> class
        /// with the given message and inner exception.
        /// </summary>
        public JobDefinitionNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
            // This space intentionally left blank.
        }
    }
}
