using System;
namespace Skybrud.Umbraco.MailChimp.Exceptions {

    public class MailChimpException : Exception {

        public MailChimpException(string message) : base(message) { }

        public MailChimpException(string message, Exception innerException) : base(message, innerException) { }

    }

}