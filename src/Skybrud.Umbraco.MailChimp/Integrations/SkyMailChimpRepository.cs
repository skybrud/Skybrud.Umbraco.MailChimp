using System;
using System.Configuration;
using MailChimp;
using MailChimp.Helper;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Skybrud.Umbraco.MailChimp.Integrations
{
    public class SkyMailChimpRepository
    {
        #region Properties

        public string ApiKey { get; set; }
        public string ListId { get; set; }
        public string Email { get; set; }
        public MailChimpManager MC { get; set; }
        public EmailParameter EP { get; set; }

        #endregion

        #region Constructor

        public SkyMailChimpRepository(string listId, string email, string contextId)
        {
            try
            {
                ApiKey = GetApiKey(contextId);
                ListId = listId;
                Email = email;
                MC = new MailChimpManager(ApiKey);
                EP = new EmailParameter
                {
                    Email = email
                };
            }
            catch (Exception ex)
            {
                LogHelper.Error<SkyMailChimpRepository>("constructor error", ex);
            }
        }

        #endregion

        #region public methods

        #endregion

        #region static methods

        /// <summary>
        ///     Get MailChimp api-key (either from web.config or Umbraco)
        /// </summary>
        /// <param name="contextId"></param>
        /// <returns></returns>
        public static string GetApiKey(string contextId = "")
        {
            if (string.IsNullOrEmpty(contextId))
            {
                return ConfigurationManager.AppSettings["mailchimpapi"];
            }

            IPublishedContent context = UmbracoContext.Current.ContentCache.GetById(int.Parse(contextId));

            if (context == null) return null;

            return context.GetPropertyValue<string>("skyMailChimpApiKey", true);
        }

        #endregion
    }
}