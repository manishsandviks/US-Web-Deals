using System;
using System.Web;
using deals.earlymoments.com.Models;
using deals.earlymoments.com.Utilities;
using OrderEngine;

namespace deals.earlymoments.com.Services
{
    public class OfferService
    {

        public ShippingModels.ShippingAddress GetDefaultCustomerInfo()
        {
            var shippingAddress = new ShippingModels.ShippingAddress
            {
                ShippingFirstName = RegexLib.GetRegexResponse(RegexLib.PatternName, value: HttpContext.Current.Request.QueryString["sFirstName"])
                ,
                ShippingLastName = RegexLib.GetRegexResponse(RegexLib.PatternName, value: HttpContext.Current.Request.QueryString["sLastName"])
                ,
                ShippingAddress1 = RegexLib.GetRegexResponse(RegexLib.PatternAddress, value: HttpContext.Current.Request.QueryString["sAddress1"])
                ,
                ShippingAddress2 = RegexLib.GetRegexResponse(RegexLib.PatternAddress, value: HttpContext.Current.Request.QueryString["sAddress2"])
                ,
                ShippingCity = RegexLib.GetRegexResponse(RegexLib.PatternCity, value: HttpContext.Current.Request.QueryString["sCity"])
                ,
                ShippingState = RegexLib.GetRegexResponse(RegexLib.PatternState, value: HttpContext.Current.Request.QueryString["sState"])
                ,
                ShippingZipCode = RegexLib.GetRegexResponse(RegexLib.PatternZipcode, value: HttpContext.Current.Request.QueryString["sZipCode"])
                ,
                ShippingEmail = RegexLib.GetRegexResponse(RegexLib.PatternEMail, value: HttpContext.Current.Request.QueryString["email"])
                ,
                ShippingConfirmEmail = RegexLib.GetRegexResponse(RegexLib.PatternEMail, value: HttpContext.Current.Request.QueryString["confEmail"])
                ,
                ShippingPhone = RegexLib.GetRegexResponse(RegexLib.PatternPhone, value: HttpContext.Current.Request.QueryString["phone1"])
            };

            return shippingAddress;
        }

        public ShippingModels.ShippingBillingOrder GetCustomerInfo()
        {
            var shippingAddress = new ShippingModels.ShippingBillingOrder
            {
                ShippingFirstName = RegexLib.GetRegexResponse(RegexLib.PatternName, value: HttpContext.Current.Request.QueryString["sFirstName"])
                ,
                ShippingLastName = RegexLib.GetRegexResponse(RegexLib.PatternName, value: HttpContext.Current.Request.QueryString["sLastName"])
                ,
                ShippingAddress1 = RegexLib.GetRegexResponse(RegexLib.PatternAddress, value: HttpContext.Current.Request.QueryString["sAddress1"])
                ,
                ShippingAddress2 = RegexLib.GetRegexResponse(RegexLib.PatternAddress, value: HttpContext.Current.Request.QueryString["sAddress2"])
                ,
                ShippingCity = RegexLib.GetRegexResponse(RegexLib.PatternCity, value: HttpContext.Current.Request.QueryString["sCity"])
                ,
                ShippingState = RegexLib.GetRegexResponse(RegexLib.PatternState, value: HttpContext.Current.Request.QueryString["sState"])
                ,
                ShippingZipCode = RegexLib.GetRegexResponse(RegexLib.PatternZipcode, value: HttpContext.Current.Request.QueryString["sZipCode"])
                ,
                ShippingEmail = RegexLib.GetRegexResponse(RegexLib.PatternEMail, value: HttpContext.Current.Request.QueryString["email"])
                ,
                ShippingConfirmEmail = RegexLib.GetRegexResponse(RegexLib.PatternEMail, value: HttpContext.Current.Request.QueryString["confEmail"])
                ,
                ShippingPhone = RegexLib.GetRegexResponse(RegexLib.PatternPhone, value: HttpContext.Current.Request.QueryString["phone1"])
            };

            return shippingAddress;
        }

        public OrderVariables MapQueryStringToOrderVariables(OrderVariables oVariables)
        {
            if (HttpContext.Current.Request.UrlReferrer != null)
            {
                if (oVariables == null) return null;
                oVariables.referring_url = HttpContext.Current.Request.UrlReferrer.ToString();
                oVariables.vendor_id = GetQueryStringFromUri("vendorcode", "");
                oVariables.vendor_data2 = GetQueryStringFromUri("key", "");
                oVariables.vendor_id = GetQueryStringFromUri("vc", "");
                oVariables.promotion_code = GetQueryStringFromUri("pc", "");
                oVariables.vendor_data1 = GetQueryStringFromUri("aff_id", "");
                oVariables.vendor_cust_ref_id = GetQueryStringFromUri("tracking", "");
                oVariables.pcode_pos_8 = GetQueryStringFromUri("src", "");
                oVariables.pcode_segment = GetQueryStringFromUri("seg", "");
                oVariables.vendor_data2 = GetQueryStringFromUri("aff_id2", "");
                oVariables.transaction_id = GetQueryStringFromUri("tracking_id", "");
            }
            return oVariables;
        }

        private string GetQueryStringFromUri(string param, string defaultValue)
        {
            try
            {
                Uri myUri = new Uri(uriString: HttpContext.Current.Request.UrlReferrer.ToString());
                string paramValue = System.Web.HttpUtility.ParseQueryString(myUri.Query).Get(param);
                return string.IsNullOrEmpty(paramValue) ? defaultValue : paramValue.ToString();
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}