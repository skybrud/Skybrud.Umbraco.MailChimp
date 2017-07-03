using System;
using System.Collections.Generic;
using MailChimp;
using MailChimp.Helper;
using Newtonsoft.Json;
using ServiceStack.Text;
using Skybrud.Umbraco.MailChimp.Extensions;
using Skybrud.Umbraco.MailChimp.Integrations;
using Umbraco.Core.Logging;

namespace Skybrud.Umbraco.MailChimp.Models
{
    public class MailChimpSignup
    {
        #region Properties
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("listid")]
        public string ListId { get; set; }

        [JsonProperty("listname")]
        public string ListName { get; set; }

        [JsonProperty("contextid")]
        public string ContextId { get; set; }

        [JsonProperty("existingUser")]
        public bool ExistingUser { get; set; }

        [JsonProperty("groups")]
        public List<SkyMailChimpGroup> Groups { get; set; }

        [JsonProperty("config")]
        public MailChimpOptions Config { get; set; }
        #endregion


        #region Public methods

        /// <summary>
        /// Add config-options to object
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public void AddConfig(MailChimpOptions config)
        {
            Config = config;

            LogHelper.Info<MailChimpSignup>("Object Initiatet: " + this.SerializeToString());
        }

        /// <summary>
        /// Saves or Updates MailChimp subscriber
        /// </summary>
        /// <returns>EmailParameter</returns>
        public EmailParameter SaveUpdateSubscriber(MailChimpSignup signUpModel)
        {
            try
            {
                if (signUpModel.Config == null)
                {
                    throw new Exception("Config-Options missing");
                }

                // add groupings if possible
                signUpModel.Config.CustomMergeVar.Groupings = signUpModel.GetGroupings();

                // initiate MailChimpManager
                MailChimpManager mcManager = new MailChimpManager(SkyMailChimpRepository.GetApiKey(signUpModel.ContextId));

                // add EmailParamteter
                EmailParameter EmailP = new EmailParameter()
                {
                    Email = signUpModel.Email
                };

                LogHelper.Info<MailChimpSignup>(signUpModel.Config.DoubleOptIn.ToString);

                // subscribe @MailChimp
                EmailParameter r = mcManager.Subscribe(signUpModel.ListId, EmailP, signUpModel.Config.CustomMergeVar,
                    signUpModel.Config.EmailType, signUpModel.Config.DoubleOptIn, signUpModel.ExistingUser);


                LogHelper.Info<MailChimpSignup>("Subscriber saved/updated: " + r.SerializeToString());

                return r;
            }
            catch (Exception ex)
            {
                LogHelper.Error<MailChimpSignup>("SaveUpdateSubscriberError", ex);

                return new EmailParameter();
            }

        }

        #endregion
    }
}