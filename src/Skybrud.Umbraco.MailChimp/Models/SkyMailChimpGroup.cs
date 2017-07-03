using System;
using System.Collections.Generic;
using System.Linq;
using MailChimp.Lists;
using Newtonsoft.Json;

namespace Skybrud.Umbraco.MailChimp.Models
{
    public class SkyMailChimpGroup
    {
        #region Properties
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [Obsolete("Use Items instead")]
        [JsonIgnore]
        public List<string> GroupNames { get; set; }

        [JsonProperty("items")]
        public List<SkyMailChimpCategory> Items { get; set; }

        #endregion


        #region Constructors
        public SkyMailChimpGroup(int id, List<string> groupNames)
        {
            Id = id;
            GroupNames = groupNames;
        }

        [JsonConstructor]
        public SkyMailChimpGroup()
        {
        }

        public SkyMailChimpGroup(Grouping g)
        {
            Id = g.Id != null ? g.Id.Value : -1;
            Name = g.Name;
            Items = SkyMailChimpCategory.GetCategories(g.GroupInterests);
        }

        public SkyMailChimpGroup(InterestGrouping ig)
        {
            Id = ig.Id;
            Name = ig.Name;
            Items = SkyMailChimpCategory.GetCategories(ig.GroupNames);
        }

        #endregion


        #region Statics

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupings"></param>
        /// <returns></returns>
        public static List<SkyMailChimpGroup> GetGroups(List<Grouping> groupings)
        {
            return groupings.Select(x => new SkyMailChimpGroup(x)).ToList();
        }

        public static List<SkyMailChimpGroup> GetGroups(List<InterestGrouping> groupings)
        {
            return groupings.Select(x => new SkyMailChimpGroup(x)).ToList();
        }

        #endregion

    }
}