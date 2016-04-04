using System.Web;
using deals.earlymoments.com.Models;
using deals.earlymoments.com.Utilities;

namespace deals.earlymoments.com.Services
{
    public class OfferService
    {

        public ShippingModels.ShippingAddress GetDefaultCustomerInfo()
        {
            var shippingAddress = new ShippingModels.ShippingAddress
            {
                ShippingFirstName =RegexLib.GetRegexResponse(RegexLib.PatternName, value: HttpContext.Current.Request.QueryString["sFirstName"])
                ,ShippingLastName =RegexLib.GetRegexResponse(RegexLib.PatternName, value: HttpContext.Current.Request.QueryString["sLastName"])
                ,ShippingAddress1 =RegexLib.GetRegexResponse(RegexLib.PatternAddress,value: HttpContext.Current.Request.QueryString["sAddress1"])
                ,ShippingAddress2 =RegexLib.GetRegexResponse(RegexLib.PatternAddress,value: HttpContext.Current.Request.QueryString["sAddress2"])
                ,ShippingCity =RegexLib.GetRegexResponse(RegexLib.PatternCity,value: HttpContext.Current.Request.QueryString["sCity"])
                ,ShippingState =RegexLib.GetRegexResponse(RegexLib.PatternState,value: HttpContext.Current.Request.QueryString["sState"])
                ,ShippingZipCode =RegexLib.GetRegexResponse(RegexLib.PatternZipcode,value: HttpContext.Current.Request.QueryString["sZipCode"])
                ,ShippingEmail =RegexLib.GetRegexResponse(RegexLib.PatternEMail,value: HttpContext.Current.Request.QueryString["email"])
                ,ShippingConfirmEmail =RegexLib.GetRegexResponse(RegexLib.PatternEMail,value: HttpContext.Current.Request.QueryString["confEmail"])
                ,ShippingPhone =RegexLib.GetRegexResponse(RegexLib.PatternPhone,value: HttpContext.Current.Request.QueryString["phone1"])
            };

            return shippingAddress;
        }
    }
}