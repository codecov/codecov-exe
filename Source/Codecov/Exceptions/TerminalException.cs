using System;

namespace Codecov.Exceptions
{
    /// <summary>
    /// Custom exception used for errors related to the terminal/running programs.
    /// </summary>
    /// <seealso cref="System.Exception"/>
    [Serializable]
    public class TerminalException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TerminalException"/> class.
        /// </summary>
        public TerminalException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TerminalException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public TerminalException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TerminalException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The inner exception.</param>
        public TerminalException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TerminalException"/> class.
        /// </summary>
        /// <param name="info">
        /// The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the
        /// serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains
        /// contextual information about the source or destination.
        /// </param>
        protected TerminalException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
