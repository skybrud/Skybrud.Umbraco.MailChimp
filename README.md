# Skybrud.Umbraco.MailChimp
Small NuGet package to help your customers subscribe/update their MailChimp subscriptions.


## Setup C#
First of all, you need to install the NuGet Package via `Install-Package Skybrud.Umbraco.MailChimp`.

After that, you need to create at least 2 models and a controller.

### Models
If you have created or altered the default MailChimp merge-fields, you need to tell Skybrud.Umbraco.MailChimp what you have done.

```csharp
using MailChimp.Lists;

namespace vejlekommune.Models.VejleKommuneWebsite.MailChimp
{
    public class CustomMailChimpMergeModel : MergeVar
    {
        public string NAME { get; set; }
    }
}
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
