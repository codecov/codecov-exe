using System;

namespace Codecov.Exceptions
{
    /// <summary>
    /// Custom exception used for failures related to uploading.
    /// </summary>
    /// <seealso cref="System.Exception"/>
    [Serializable]
    public class UploadException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UploadException"/> class.
        /// </summary>
        public UploadException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public UploadException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadException"/> class.
        /// </summary>
        /// <param name="message">The message that descripbes the error.</param>
        /// <param name="inner">The inner exception.</param>
        public UploadException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadException"/> class.
        /// </summary>
        /// <param name="info">
        /// The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the
        /// serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains
        /// contextual information about the source or destination.
        /// </param>
        protected UploadException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
