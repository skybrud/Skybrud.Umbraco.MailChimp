using System;
using System.Collections.Generic;
using MailChimp;
using MailChimp.Helper;
using Newtonsoft.Json;
using ServiceStack.Text;
using Skybrud.Umbraco.MailChimp.Exceptions;
using Skybrud.Umbraco.MailChimp.Extensions;
using Skybrud.Umbraco.MailChimp.Integrations;

namespace Skybrud.Umbraco.MailChimp.Models
{
    public class MailChimpSignup
    {
        #region Properties
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
        }

        /// <summary>
        /// Saves or Updates MailChimp subscriber
        /// </summary>
        /// <returns>EmailParameter</returns>
        public EmailParameter SaveUpdateSubscriber(MailChimpSignup signUpModel) {

            if (signUpModel.Config == null) throw new Exception("Config-Options missing");

            try {

                // add groupings if possible
                signUpModel.Config.CustomMergeVar.Groupings = signUpModel.GetGroupings();

                // initiate MailChimpManager
                MailChimpManager mcManager = new MailChimpManager(SkyMailChimpRepository.GetApiKey(signUpModel.ContextId));

                // add EmailParamteter
                EmailParameter EmailP = new EmailParameter {
                    Email = signUpModel.Email
                };

                // subscribe @MailChimp
                EmailParameter r = mcManager.Subscribe(signUpModel.ListId, EmailP, signUpModel.Config.CustomMergeVar,
                    signUpModel.Config.EmailType, signUpModel.Config.DoubleOptIn, signUpModel.ExistingUser);

                return r;

            } catch (Exception ex) {
                
                throw new MailChimpSignupException(signUpModel, ex);

            }

        }

        #endregion
    }
}
