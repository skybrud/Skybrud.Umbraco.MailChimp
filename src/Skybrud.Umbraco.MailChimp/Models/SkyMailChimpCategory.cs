using System.Collections.Generic;
using System.Linq;
using MailChimp.Lists;
using Newtonsoft.Json;

namespace Skybrud.Umbraco.MailChimp.Models
{
    public class SkyMailChimpCategory
    {
        #region Properties
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("checked")]
        public bool Checked { get; set; }
        #endregion


        #region Constructors
        public SkyMailChimpCategory(string categoryname, bool selected)
        {
            Value = categoryname;
            Checked = selected;
        }

        [JsonConstructor]
        public SkyMailChimpCategory()
        {}

        /// <summary>
        /// Used when subscriber exists
        /// </summary>
        /// <param name="category"></param>
        public SkyMailChimpCategory(Grouping.GroupInterest category)
        {
            Value = category.Name;
            Checked = category.Interested;
        }

        /// <summary>
        /// Used when no subscriber exists
        /// </summary>
        /// <param name="category"></param>
        public SkyMailChimpCategory(InterestGrouping.InnerGroup category)
        {
            Value = category.Name;
            Checked = false;
        }
        #endregion


        #region Statics
        public static List<SkyMailChimpCategory> GetCategories(List<Grouping.GroupInterest> categories)
        {
            return categories.Select(x => new SkyMailChimpCategory(x)).ToList();
        }

        public static List<SkyMailChimpCategory> GetCategories(List<InterestGrouping.InnerGroup> categories)
        {
            return categories.Select(x => new SkyMailChimpCategory(x)).ToList();
        }
        #endregion




    }
}