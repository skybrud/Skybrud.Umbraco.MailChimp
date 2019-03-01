using System;
using Skybrud.Umbraco.MailChimp.Models;

namespace Skybrud.Umbraco.MailChimp.Exceptions {

    public class MailChimpSignupException : MailChimpException {

        public MailChimpSignup Details { get; }

        public MailChimpSignupException(MailChimpSignup details, Exception innerException) : base(innerException.Message, innerException) {
            Details = details;
        }

    }

}