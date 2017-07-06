using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Http;
using MailChimp.Lists;
using Skybrud.Umbraco.MailChimp.Integrations;
using Skybrud.Umbraco.MailChimp.Models;
using Skybrud.WebApi.Json;
using umbraco.cms.presentation;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Web;
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
        /// <param name="subscriberId"></param>
        /// <returns>SkyMailChimpSubscriber</returns>
        [HttpGet]
        public object GetSubscriber(string email, string contextid, string listid, string subscriberId = "")
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
                    if (subscriber.Id == subscriberId)
                    {
                        // all okay, load the user
                        var mcs = new SkyMailChimpSubscriber(subscriber);

                        // return SkyMailChimpSubscriber object
                        return Request.CreateResponse(mcs);
                    }
                    else
                    {
                        // subscriberId missing, throw error
                        return Request.CreateResponse(HttpStatusCode.Unauthorized);
                    }
                }

                // no subscriber found, send empty SkyMailChimpSubscriber
                var noMcs = new SkyMailChimpSubscriber(email, listinfo, listig);
                return Request.CreateResponse(HttpStatusCode.PartialContent, noMcs);
            }
            catch (Exception ex)
            {
                LogHelper.Error<SkyMailChimpController>("mailchimp error (GetSubscriber)", ex);

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Send updatelink to user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="contextid"></param>
        /// <param name="listid"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpGet]
        public object SendUpdateLink(string email, string contextid, string listid)
        {
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");

            try
            {
                SkyMailChimpHelper helper = new SkyMailChimpHelper(SkyMailChimpRepository.GetApiKey(contextid), listid, email);
                MemberInfo subscriber = helper.GetSubscriber();
                IPublishedContent currentPage = UmbracoContext.ContentCache.GetById(int.Parse(contextid));
                string host = HttpContext.Current.Request.Url.Host;
                var scheme = HttpContext.Current.Request.Url.Scheme;

                if (currentPage == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "contenxt not existing");
                }

                // build update url
                string updateUrl = string.Format("{0}://{1}{2}?email={3}&id={4}", scheme, host, currentPage.Url, subscriber.Email, subscriber.Id);

                // fetch settings from Umbraco
                string subject = currentPage.GetPropertyValue<string>("skyMailChimpUpdateMailSubject", true);
                string body = currentPage.GetPropertyValue<string>("skyMailChimpUpdateMailBody", true);

                // set default values if non found
                subject = string.IsNullOrWhiteSpace(subject) ? "Rediger din nyhedsbrevsprofil" : subject;
                body = string.IsNullOrWhiteSpace(body) ? "<a href=\"{updateLink}\">Opdater din profil</a>" : body;

                // replace body tags
                body = body.Replace("{updateLink}", updateUrl);

                // send mail
                MailMessage msg = new MailMessage();
                msg.To.Add(subscriber.Email);
                msg.Subject = subject;
                msg.BodyEncoding = Encoding.UTF8;
                msg.IsBodyHtml = true;

                msg.Body = body;

                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Send(msg);
                }

                LogHelper.Info<SkyMailChimpController>(string.Format("Mail send to: {0} ({1})", subscriber.Id, subscriber.Email));

                return Request.CreateResponse(true);
            }
            catch (Exception ex)
            {
                LogHelper.Error<SkyMailChimpController>("mailchimp error (SendUpdateLink)", ex);

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        #endregion

    }
}
