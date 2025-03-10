﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using datntdev.Abp.Logging;

namespace datntdev.Abp.Runtime.Validation
{
    /// <summary>
    /// This exception type is used to throws validation exceptions.
    /// </summary>
    [Serializable]
    public class AbpValidationException : AbpException, IHasLogSeverity
    {
        /// <summary>
        /// Default log severity
        /// </summary>
        public static LogSeverity DefaultLogSeverity = LogSeverity.Warn;
        
        /// <summary>
        /// Detailed list of validation errors for this exception.
        /// </summary>
        public IList<ValidationResult> ValidationErrors { get; set; }

        /// <summary>
        /// Exception severity.
        /// Default: Warn.
        /// </summary>
        public LogSeverity Severity { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public AbpValidationException()
        {
            ValidationErrors = new List<ValidationResult>();
            Severity = DefaultLogSeverity;
        }

        /// <summary>
        /// Constructor for serializing.
        /// </summary>
        public AbpValidationException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
            ValidationErrors = new List<ValidationResult>();
            Severity = DefaultLogSeverity;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Exception message</param>
        public AbpValidationException(string message)
            : base(message)
        {
            ValidationErrors = new List<ValidationResult>();
            Severity = DefaultLogSeverity;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="validationErrors">Validation errors</param>
        public AbpValidationException(string message, IList<ValidationResult> validationErrors)
            : base(message)
        {
            ValidationErrors = validationErrors;
            Severity = DefaultLogSeverity;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public AbpValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
            ValidationErrors = new List<ValidationResult>();
            Severity = DefaultLogSeverity;
        }
    }
}
