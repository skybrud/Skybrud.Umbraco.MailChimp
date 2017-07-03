using System.Collections.Generic;
using MailChimp.Lists;
using Newtonsoft.Json;

namespace Skybrud.Umbraco.MailChimp.Models
{
    public class SkyMailChimpSubscriber
    {
        #region Properties
        
        [JsonProperty("name")]
        public string Name { get;  set; }

        [JsonProperty("email")]
        public string Email { get;  set; }

        [JsonProperty("listId")]
        public string ListId { get; set; }

        [JsonProperty("listName")]
        public string ListName { get; set; }

        [JsonProperty("groups")]
        public List<SkyMailChimpGroup> Groups { get; set; }

        #endregion

        #region Constructors

        public SkyMailChimpSubscriber()
        {
            Name = "";
            Email = "";
            ListId = "";
            ListName = "";
            Groups = new List<SkyMailChimpGroup>();
        }

        public SkyMailChimpSubscriber(string email, ListInfo listinfo, List<InterestGrouping> groupings = null)
        {
            Name = "";
            Email = email;
            ListId = listinfo.Id;
            ListName = listinfo.Name;
            Groups = groupings != null ? SkyMailChimpGroup.GetGroups(groupings) : null;
        }

        public SkyMailChimpSubscriber(MemberInfo subscriber)
        {
            //fetching NAME merge from MailChimp
            object name;
            subscriber.MemberMergeInfo.TryGetValue("NAME", out name);

            Name = name != null ? name.ToString() : "";
            Email = subscriber.Email;
            ListId = subscriber.ListId;
            ListName = subscriber.ListName;
            Groups = SkyMailChimpGroup.GetGroups(subscriber.MemberMergeInfo.Groupings);
        }

        #endregion
    }
}