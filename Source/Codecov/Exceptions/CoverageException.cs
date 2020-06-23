using System;

namespace Codecov.Exceptions
{
    /// <summary>
    /// Custom exception used for failures related to coverage reports.
    /// </summary>
    /// <seealso cref="System.Exception"/>
    [Serializable]
    public class CoverageException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CoverageException"/> class.
        /// </summary>
        public CoverageException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoverageException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public CoverageException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoverageException"/> class.
        /// </summary>
        /// <param name="message">The message that descripbes the error.</param>
        /// <param name="inner">The inner exception.</param>
        public CoverageException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoverageException"/> class.
        /// </summary>
        /// <param name="info">
        /// The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the
        /// serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains
        /// contextual information about the source or destination.
        /// </param>
        protected CoverageException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
