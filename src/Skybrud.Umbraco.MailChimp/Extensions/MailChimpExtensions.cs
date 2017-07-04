using System.Collections.Generic;
using System.Linq;
using MailChimp.Lists;
using Skybrud.Umbraco.MailChimp.Models;

namespace Skybrud.Umbraco.MailChimp.Extensions
{
    public static class MailChimpExtensions
    {
        /// <summary>
        /// Get groupings from MailChimpSignup object
        /// </summary>
        /// <param name="mcsu">MailChimpSignup</param>
        /// <returns>List<Grouping></returns>
        public static List<Grouping> GetGroupings(this MailChimpSignup mcsu)
        {
            if (mcsu.Groups == null) return new List<Grouping>();
            
            List<Grouping> groupings = new List<Grouping>();

            if (mcsu.Groups.Any())
            {
                groupings.AddRange(mcsu.Groups.Select(g => new Grouping
                {
                    Id = g.Id,
                    GroupNames = g.Items.Where(x => x.Checked).Select(x => x.Value).ToList()
                }).Where(gr => gr != null).ToList());
            }

            return groupings;
        }
    }
}