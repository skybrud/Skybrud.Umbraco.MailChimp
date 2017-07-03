using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using MailChimp.Lists;
using Skybrud.Umbraco.MailChimp.Integrations;
using Skybrud.Umbraco.MailChimp.Models;
using Skybrud.WebApi.Json;
using Umbraco.Core.Logging;
using Umbraco.Web.WebApi;

namespace Skybrud.Umbraco.MailChimp.Controllers.Api
{
    [JsonOnlyConfiguration]
    public class SkyMailChimpController : UmbracoApiController
    {
        #region Get Methods
        /// <summary>
        ///     Used to render Newsletter Signupform
        /// </summary>
        /// <param name="listid"></param>
        /// <param name="contextid"></param>
        /// <returns>SkyMailChimpSubscriber</returns>
        [HttpGet]
        public object GetTemplate(string listid, string contextid)
        {
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");

            try
            {
                var helper = new SkyMailChimpHelper(SkyMailChimpRepository.GetApiKey(contextid), listid, "");

                ListInfo listinfo = helper.MC.GetLists().Data.FirstOrDefault(x => x.Id == listid);
                List<InterestGrouping> listig = null;

                // surpressing error from MailChimp Api
                try
                {
                    listig = helper.MC.GetListInterestGroupings(listid);
                }
                catch (Exception ex)
                {
                }


                // find init data for MailChimp groups
                var noMcs = new SkyMailChimpSubscriber("", listinfo, listig);
                return Request.CreateResponse(noMcs);
            }
            catch (Exception ex)
            {
                LogHelper.Error<SkyMailChimpController>("mailchimp error", ex);

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        ///     Get data from MailChimp (if user exists)
        /// </summary>
        /// <param name="email"></param>
        /// <param name="contextid"></param>
        /// <param name="listid"></param>
        /// <returns>SkyMailChimpSubscriber</returns>
        [HttpGet]
        public object GetSubscriber(string email, string contextid, string listid)
        {
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");

            try
            {
                var helper = new SkyMailChimpHelper(SkyMailChimpRepository.GetApiKey(contextid), listid, email);

                ListInfo listinfo = helper.MC.GetLists().Data.FirstOrDefault(x => x.Id == listid);
                List<InterestGrouping> listig = null;

                // surpressing error from MailChimp Api
                try
                {
                    listig = helper.MC.GetListInterestGroupings(listid);
                }
                catch (Exception ex)
                {
                }

                MemberInfo subscriber = helper.GetSubscriber();

                if (subscriber != null)
                {
                    var mcs = new SkyMailChimpSubscriber(subscriber);

                    // return SkyMailChimpSubscriber object
                    return Request.CreateResponse(mcs);
                }

                // no subscriber found, send empty SkyMailChimpSubscriber
                var noMcs = new SkyMailChimpSubscriber(email, listinfo, listig);
                return Request.CreateResponse(HttpStatusCode.PartialContent, noMcs);
            }
            catch (Exception ex)
            {
                LogHelper.Error<SkyMailChimpController>("mailchimp error", ex);

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        #endregion
    }
}
