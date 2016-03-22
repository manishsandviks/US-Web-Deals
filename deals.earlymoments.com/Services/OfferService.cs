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
            //@sFirstName=#fname
            //&sLastName=#lname
            //&sAddress1=#street
            //&sAddress2=
            //&sCity=#city
            //&sState=#state
            //&sZipCode=#zip_5
            //&email=#email
            //&confEmail=#email
            //&phone1=
            //&phone2=
            //&phone3= 

            ShippingModels.ShippingAddress shippingAddress = new ShippingModels.ShippingAddress
            {
                ShippingFirstName = HttpContext.Current.Request.QueryString["sFirstName"] as string
                , ShippingLastName = HttpContext.Current.Request.QueryString["sLastName"] as string
                , ShippingAddress1 = HttpContext.Current.Request.QueryString["sAddress1"] as string
                , ShippingAddress2 = HttpContext.Current.Request.QueryString["sAddress2"] as string
                , ShippingCity = HttpContext.Current.Request.QueryString["sCity"] as string
                , ShippingState = HttpContext.Current.Request.QueryString["sState"] as string
                , ShippingZipCode = HttpContext.Current.Request.QueryString["sZipCode"] as string
                , ShippingEmail = HttpContext.Current.Request.QueryString["email"] as string
                , ShippingConfirmEmail = HttpContext.Current.Request.QueryString["confEmail"] as string
                , ShippingPhone = HttpContext.Current.Request.QueryString["phone1"] as string
                //phone2
                //phone3
            };

            return shippingAddress;
        }
    }
}