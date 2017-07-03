using MailChimp.Lists;

namespace Skybrud.Umbraco.MailChimp.Models
{
    public class MailChimpOptions
    {
        #region Properties

        /// <summary>
        /// Set to true if subscriber schould accept via e-mail
        /// </summary>
        public bool DoubleOptIn { get; set; }

        public string EmailType { get; set; }

        /// <summary>
        /// Add more custom fields from MailChimp
        /// </summary>
        public MergeVar CustomMergeVar { get; set; }

        #endregion


        #region Constructors

        public MailChimpOptions()
        {
            DoubleOptIn = false;
            EmailType = "";
            CustomMergeVar = new MergeVar();
        }

        public MailChimpOptions(bool doubleOptIn)
        {
            DoubleOptIn = doubleOptIn;
            EmailType = "";
            CustomMergeVar = new MergeVar();
        }

        public MailChimpOptions(MergeVar customMergeVar)
        {
            DoubleOptIn = false;
            EmailType = "";
            CustomMergeVar = customMergeVar;
        }

        public MailChimpOptions(string emailType)
        {
            DoubleOptIn = false;
            CustomMergeVar = new MergeVar();
            EmailType = emailType;
        }

        public MailChimpOptions(bool doubleOptIn, MergeVar customMergeVar)
        {
            DoubleOptIn = doubleOptIn;
            EmailType = "";
            CustomMergeVar = customMergeVar;
        }

        public MailChimpOptions(bool doubleOptIn, string emailType, MergeVar customMergeVar)
        {
            DoubleOptIn = doubleOptIn;
            EmailType = emailType;
            CustomMergeVar = customMergeVar;
        }

        #endregion


        

    }
}
