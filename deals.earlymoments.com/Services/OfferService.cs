using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using deals.earlymoments.com.Models;

namespace deals.earlymoments.com.Services
{
    public class OfferService
    {
        public ShippingModels.ShippingAddress GetDefaultCustomerInfo()
        {
            var shippingAddress = new ShippingModels.ShippingAddress
            {
                ShippingFirstName = System.Web.HttpContext.Current.Request.QueryString["sFirstName"] as string
                ,ShippingLastName = System.Web.HttpContext.Current.Request.QueryString["sLastName"] as string
                ,ShippingAddress1 = System.Web.HttpContext.Current.Request.QueryString["sAddress1"] as string
                ,ShippingAddress2 = System.Web.HttpContext.Current.Request.QueryString["sAddress2"] as string
                ,ShippingCity = System.Web.HttpContext.Current.Request.QueryString["sCity"] as string
                ,ShippingState = System.Web.HttpContext.Current.Request.QueryString["sState"] as string
                ,ShippingZipCode = System.Web.HttpContext.Current.Request.QueryString["sZipCode"] as string
                ,ShippingEmail = System.Web.HttpContext.Current.Request.QueryString["email"] as string
                ,ShippingConfirmEmail = System.Web.HttpContext.Current.Request.QueryString["confEmail"] as string
                ,ShippingPhone = System.Web.HttpContext.Current.Request.QueryString["phone1"] as string
                //phone2
                //phone3
            };

            return shippingAddress;
        }
    }
}