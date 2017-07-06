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

The deafult class you enherit from contains theese properties:

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


## Umbraco Properties
`skyMailChimpApiKey` - API key from MailChimp (overwrites appsetting `mailchimpapi` in web.config)

`skyMailChimpUpdateMailSubject` - Overwrites default subject for update profile e-mail

`skyMailChimpUpdateMailBody` - Overwrites default body for update profile e-mail ({updateLink})



## Updates

#### 0.5.3
* Security added. You know need to have emailId from MailChimp to edit/update existing subscriber
* Now you can send the subscriber a update-link
