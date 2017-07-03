using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using MailChimp;
using MailChimp.Helper;
using MailChimp.Lists;
using Skybrud.Essentials.Json;

namespace Skybrud.Umbraco.MailChimp.Models
{
    public class SkyMailChimpHelper
    {

        #region Properties
        private int newsCategoryId = 0;
        private int subscriberTypeId = 0;

        public string ApiKey { get; set; }
        public string ListId { get; set; }
        public string Email { get; set; }
        public MailChimpManager MC { get; set; }
        public EmailParameter EP { get; set; }
        #endregion


        #region Constructor
        public SkyMailChimpHelper(string apiKey, string listId, string email)
        {
            ApiKey = apiKey;
            ListId = listId;
            Email = email;
            MC = new MailChimpManager(apiKey);
            EP = new EmailParameter()
            {
                Email = email
            };
        }
        #endregion


        #region static methods

        /// <summary>
        /// Cast Json data to MailChimpSignup model
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T ConvertToMailChimpSignupModel<T>(HttpRequestMessage obj)
        {
            return (T) JsonUtils.ParseJsonObject<T>(obj.Content.ReadAsStringAsync().Result);
        }


        #endregion


        #region public methods

        public MemberInfo GetSubscriber()
        {
            return GetSubscriber(Email);
        }

        public MemberInfo GetSubscriber(string email)
        {
            var emailList = new List<EmailParameter> { EP };

            var data = MC.GetMemberInfo(ListId, emailList).Data.Where(x => x.Status == "subscribed").ToList();

            return data.Count > 0 ? data.FirstOrDefault() : null;
        }
        #endregion


        [DataContract]
        public class Variables : MergeVar
        {
            [DataMember(Name = "FNAME")]
            public string Name { get; set; }
        }
    }
}