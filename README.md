# Skybrud.Umbraco.MailChimp
Small NuGet package to help your customers subscribe/update their MailChimp subscriptions.


## Setup C#
First of all, you need to install the NuGet Package via `Install-Package Skybrud.Umbraco.MailChimp`.

After that, you need to create at least 2 models and a controller.

### Models
If you have created or altered the default MailChimp merge-fields, you need to tell Skybrud.Umbraco.MailChimp what you have done.

```csharp
using MailChimp.Lists;

namespace Customer.Models.Website.MailChimp
{
    public class CustomMailChimpMergeModel : MergeVar
    {
        public string NAME { get; set; }
    }
}
```

You also need to tell Skybrud.Umbraco.MailChimp what properties your submit form will send. Please remember to enherit from `MailChimpSignup` as shown below.

```csharp
using System.Net.Http;
using Skybrud.Umbraco.MailChimp.Models;

namespace Customer.Models.Website.MailChimp
{
    public class CustomMailChimpFormModel : MailChimpSignup
    {
        #region Properties
        #endregion

        #region Statics
        public static CustomMailChimpFormModel GetModel(HttpRequestMessage request)
        {
            return SkyMailChimpHelper.ConvertToMailChimpSignupModel<CustomMailChimpFormModel>(request);
        }
        #endregion
    }
}
```

The deafult class you enherit from contains these properties:

```csharp
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
```


### Controller
You also need to create a controller to handle your api-call from the frontend. Here you can find an example:

```csharp
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using MailChimp.Helper;
using ServiceStack.Text;
using Skybrud.Umbraco.MailChimp.Models;
using Umbraco.Core.Logging;
using Umbraco.Web.WebApi;
using Customer.Models.Website.MailChimp;

namespace Customer.Controllers.Api.MailChimp
{
    public class MailChimpSignupController : UmbracoApiController
    {
        [HttpPost]
        public object SaveUpdate(HttpRequestMessage request)
        {
            try
            {
                // cast your custom mailchimp form model
                CustomMailChimpFormModel mco = CustomMailChimpFormModel.GetModel(request);


                // add your config model to the MailChimpFormModel
                var config = new MailChimpOptions(true, "html", new CustomMailChimpMergeModel
                {
                    NAME = mco.Name
                });
                mco.AddConfig(config);

                EmailParameter r = mco.SaveUpdateSubscriber(mco);

                return Request.CreateResponse(r);
            }
            catch (Exception ex)
            {
                LogHelper.Error<MailChimpSignupController>("mailchimp error", ex);

                return
                    Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
```

Now you can post to this api, and Skybrud.Umbraco.MailChimp will save or update your subscriber.


## Umbraco Properties
`skyMailChimpApiKey` - API key from MailChimp (overwrites appsetting `mailchimpapi` in web.config)

`skyMailChimpUpdateFromEmail` - Overwrites default from-mailaddress for update profile e-mail

`skyMailChimpUpdateFromDisplayName` - Overwrites default displayname for update profile e-mail

`skyMailChimpUpdateMailSubject` - Overwrites default subject for update profile e-mail

`skyMailChimpUpdateMailBody` - Overwrites default body for update profile e-mail ({updateLink})


## MailChimp template
If you want your subscribers to be able to ex. update their groups or name, you can add an update link in your MailChimp template.

`<a href="https://customerdomain.com/news/newslettersignup/?email=*|EMAIL|*&amp;id=*|EMAIL_UID|*" mc:edit="std_update">Update your profile</a>`

Remember to change domain + url, but leave the querystring.


## Updates

#### 0.5.3
* Security added. You know need to have emailId from MailChimp to edit/update existing subscriber
* Now you can send the subscriber a update-link

#### 0.5.6
* Added option for overwrite default from e-mail and display name (update mail)
