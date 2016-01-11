using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using OrderEngine;

namespace deals.earlymoments.com.Models
{
    public class ShippingModels
    {
        public class ShippingAddress
        {
            [Required(ErrorMessage = "Shipping First Name is required.")]
            [StringLength(50, MinimumLength = 3, ErrorMessage = "Shipping First Name cannot be longer than 50 characters.")]
            [DisplayName("First Name")]
            public string ShippingFirstName { get; set; }

            [Required(ErrorMessage = "Shipping Last Name is required.")]
            [StringLength(50, MinimumLength = 3, ErrorMessage = "Shipping Last Name cannot be longer than 50 characters.")]
            [DisplayName("Last Name")]
            public string ShippingLastName { get; set; }

            [Required(ErrorMessage = "Shipping Address is required.")]
            [StringLength(30, MinimumLength = 3, ErrorMessage = "Shipping Address1 cannot be longer than 30 characters.")]
            [DisplayName("Street Address")]
            public string ShippingAddress1 { get; set; }

            [StringLength(30, MinimumLength = 3, ErrorMessage = "Shipping Address2 cannot be longer than 30 characters.")]
            public string ShippingAddress2 { get; set; }

            [Required(ErrorMessage = "Shipping City is required.")]
            [StringLength(50, MinimumLength = 3, ErrorMessage = "Last Name cannot be longer than 50 characters.")]
            public string ShippingCity { get; set; }

            [Required(ErrorMessage = "Shipping State is required.")]
            [StringLength(2, MinimumLength = 2, ErrorMessage = "Shipping State cannot be longer than 50 characters.")]
            public string ShippingState { get; set; }

            [Required(ErrorMessage = "Shipping Zip Code is required.")]
            [StringLength(10, MinimumLength = 5, ErrorMessage = "Shipping Zip Code cannot be longer than 10 characters.")]
            public string ShippingZipCode { get; set; }

            [StringLength(10, MinimumLength = 10, ErrorMessage = "Shipping Phone cannot be longer than 10 characters.")]
            public string ShippingPhone { get; set; }

            [Required(ErrorMessage = "Shipping Email is required.")]
            [StringLength(50, MinimumLength = 10, ErrorMessage = "Shipping Email cannot be longer than 50 characters.")]
            public string ShippingEmail { get; set; }

            [Required(ErrorMessage = "Shipping Confirm Email is required.")]
            [StringLength(50, MinimumLength = 6, ErrorMessage = "Shipping Confirm Email cannot be longer than 50 characters.")]
            public string ShippingConfirmEmail { get; set; }

            [StringLength(50, ErrorMessage = "Child Name cannot be longer than 50 characters.")]
            public string ChildName { get; set; }

            public DateTime? ChildDOB { get; set; }
            public string ChildGender { get; set; }

            public bool isBonusSelected { get; set; }

            public ShippingAddress()
            {
                ShippingFirstName = "";
                ShippingLastName = "";
                ShippingAddress1 = "";
                ShippingAddress2 = "";
                ShippingCity = "";
                ShippingState = "";
                ShippingZipCode = "";
                ShippingPhone = "";
                ShippingEmail = "";
                ShippingConfirmEmail = "";
                ChildDOB = null;
                ChildGender = "";
                ChildName = "";
                isBonusSelected = false;
            }

        }

        public class PaymentDetails
        {
            [Required(ErrorMessage = "Credit Card is required.")]
            [StringLength(17, MinimumLength = 15, ErrorMessage = "Credit Card Number cannot be longer than 17 characters.")]
            public string CreditCardNumber { get; set; }

            [Required(ErrorMessage = "Card Expiry Month is required.")]
            [StringLength(3, MinimumLength = 2, ErrorMessage = "Card Expiry Month cannot be longer than 3 characters.")]
            public string CardExpiryMonth { get; set; }

            [Required(ErrorMessage = "Card Expiry Year is required.")]
            [StringLength(3, MinimumLength = 2, ErrorMessage = "Card Expiry Year cannot be longer than 4 characters.")]
            public string CardExpiryYear { get; set; }

            [Required(ErrorMessage = "Security Code is required.")]
            [StringLength(30, MinimumLength = 3, ErrorMessage = "Security Code cannot be longer than 4 characters.")]
            public string SecurityCode { get; set; }

            [Required(ErrorMessage = "Billing Zip Code is required.")]
            [StringLength(10, MinimumLength = 5, ErrorMessage = "Billing Zip Code cannot be longer than 10 characters.")]
            public string BillingZipCode { get; set; }

            public PaymentDetails()
            {
                CreditCardNumber = "";
                CardExpiryMonth = "";
                CardExpiryYear = "";
                SecurityCode = "";
                BillingZipCode = "";
            }

        }

        public class BillingDetails
        {
            [Required(ErrorMessage = "Billing First Name is required.")]
            [DisplayName("First Name")]
            [StringLength(50, MinimumLength = 3, ErrorMessage = "Billing First Name cannot be longer than 50 characters.")]
            public string BillingFirstName { get; set; }

            [Required(ErrorMessage = "Billing Last Name is required.")]
            [StringLength(50, MinimumLength = 3, ErrorMessage = "Billing Last Name cannot be longer than 50 characters.")]
            public string BillingLastName { get; set; }

            [Required(ErrorMessage = "Billing Address is required.")]
            [StringLength(30, MinimumLength = 3, ErrorMessage = "Billing Address1 cannot be longer than 30 characters.")]
            public string BillingAddress1 { get; set; }

            [StringLength(30, MinimumLength = 3, ErrorMessage = "Billing Address2 cannot be longer than 30 characters.")]
            public string BillingAddress2 { get; set; }

            [Required(ErrorMessage = "Billing City is required.")]
            [StringLength(50, MinimumLength = 3, ErrorMessage = "Billing City cannot be longer than 50 characters.")]
            public string BillingCity { get; set; }

            [Required(ErrorMessage = "Billing State is required.")]
            [StringLength(2, MinimumLength = 2, ErrorMessage = "Billing State cannot be longer than 50 characters.")]
            public string BillingState { get; set; }

            [Required(ErrorMessage = "Billing Zip Code is required.")]
            [StringLength(10, MinimumLength = 5, ErrorMessage = "Billing Zip Code cannot be longer than 10 characters.")]
            public string BillingZipCode { get; set; }

            [DisplayName("Is your billing address the same as your shipping address?")]
            public bool isBillingSameToShipping { get; set; }

            [Required(ErrorMessage = "Credit Card is required.")]
            [DisplayName("Credit Card Number")]
            [StringLength(17, MinimumLength = 15, ErrorMessage = "Credit Card Number cannot be longer than 17 characters.")]
            public string CreditCardNumber { get; set; }

            [Required(ErrorMessage = "Card Expiry Month is required.")]
            [DisplayName("Expiry Month")]
            [StringLength(3, MinimumLength = 2, ErrorMessage = "Card Expiry Month cannot be longer than 3 characters.")]
            public string CardExpiryMonth { get; set; }

            [Required(ErrorMessage = "Card Expiry Year is required.")]
            [DisplayName("Expiry Year")]
            [StringLength(3, MinimumLength = 2, ErrorMessage = "Card Expiry Year cannot be longer than 4 characters.")]
            public string CardExpiryYear { get; set; }

            [Required(ErrorMessage = "Security Code is required.")]
            [DisplayName("Security Code")]
            [StringLength(30, MinimumLength = 3, ErrorMessage = "Security Code cannot be longer than 4 characters.")]
            public string SecurityCode { get; set; }

            [Required(ErrorMessage = "Billing Zip Code is required.")]
            [DisplayName("Billing Zip Code")]
            [StringLength(10, MinimumLength = 5, ErrorMessage = "Billing Zip Code cannot be longer than 10 characters.")]
            public string CCBillZipCode { get; set; }
            public string SecurityCaptch { get; set; }

            public BillingDetails()
            {
                isBillingSameToShipping = true;
                BillingFirstName = "";
                BillingLastName = "";
                BillingAddress1 = "";
                BillingAddress2 = "";
                BillingCity = "";
                BillingState = "";
                BillingZipCode = "";
                CreditCardNumber = "";
                CardExpiryMonth = "";
                CardExpiryYear = "";
                SecurityCode = "";
                CCBillZipCode = "";
                SecurityCaptch = "";
            }

        }

        public class BillingAndPaymentModels
        {
            public BillingDetails BillingAddress { get; set; }
            public PaymentDetails PaymentDetails { get; set; }
        }

        public static OrderVariables AssignShippingToOrderVariable(OrderVariables oVariables, ShippingAddress newShippingAddress)
        {
            CommonModels oComm = new CommonModels();
            //OrderVariables oVariables = new OrderVariables();
            oVariables.ShipVars[oVariables.default_shp_id].ship_to_fname = newShippingAddress.ShippingFirstName;
            oVariables.ShipVars[oVariables.default_shp_id].ship_to_lname = newShippingAddress.ShippingLastName;
            oVariables.ShipVars[oVariables.default_shp_id].ship_to_address1 = newShippingAddress.ShippingAddress1;
            oVariables.ShipVars[oVariables.default_shp_id].ship_to_apt = newShippingAddress.ShippingAddress2;
            oVariables.ShipVars[oVariables.default_shp_id].ship_to_city = newShippingAddress.ShippingCity;
            oVariables.ShipVars[oVariables.default_shp_id].ship_to_state = newShippingAddress.ShippingState;
            oVariables.ShipVars[oVariables.default_shp_id].ship_to_zipcode = newShippingAddress.ShippingZipCode;
            oVariables.referring_url = HttpContext.Current.Request.Url.ToString();

            oVariables.ip_address = oComm.GetIPAddress();
            oVariables.phone = newShippingAddress.ShippingPhone;
            oVariables.email = newShippingAddress.ShippingEmail;
            oVariables.bonus_option = false;

            if (newShippingAddress.isBonusSelected)
            {
                oVariables.bonus_option = true;
            }

            //Setting billing Address by default same as shipping address.
            // later on payment page it will be updated if shipping and biling not same
            oVariables.bill_to_fname = oVariables.ShipVars[oVariables.default_shp_id].ship_to_fname;
            oVariables.bill_to_lname = oVariables.ShipVars[oVariables.default_shp_id].ship_to_lname;
            oVariables.bill_to_address1 = oVariables.ShipVars[oVariables.default_shp_id].ship_to_address1;
            oVariables.bill_to_apt = oVariables.ShipVars[oVariables.default_shp_id].ship_to_apt;
            oVariables.bill_to_city = oVariables.ShipVars[oVariables.default_shp_id].ship_to_city;
            oVariables.bill_to_state = oVariables.ShipVars[oVariables.default_shp_id].ship_to_state;
            oVariables.bill_to_zipcode = oVariables.ShipVars[oVariables.default_shp_id].ship_to_zipcode;
            return oVariables;
        }

        public static OrderVariables AssignBillingToOrderVariable(OrderVariables oVariables, BillingDetails billingDetails)
        {
            //Assigning Billing Address details to OVariable
            //if (billingDetails.isBillingSameToShipping)
            //{
            //    oVariables.bill_to_fname = oVariables.ShipVars[oVariables.default_shp_id].ship_to_fname;
            //    oVariables.bill_to_lname = oVariables.ShipVars[oVariables.default_shp_id].ship_to_lname;
            //    oVariables.bill_to_address1 = oVariables.ShipVars[oVariables.default_shp_id].ship_to_address1;
            //    oVariables.bill_to_apt = oVariables.ShipVars[oVariables.default_shp_id].ship_to_apt;
            //    oVariables.bill_to_city = oVariables.ShipVars[oVariables.default_shp_id].ship_to_city;
            //    oVariables.bill_to_state = oVariables.ShipVars[oVariables.default_shp_id].ship_to_state;
            //    oVariables.bill_to_zipcode = oVariables.ShipVars[oVariables.default_shp_id].ship_to_zipcode;
            //}
            //else


            //As we are setting billing and shipping address same on landing page 
            if (billingDetails.isBillingSameToShipping == false)
            {
                oVariables.bill_to_fname = billingDetails.BillingFirstName.Trim();
                oVariables.bill_to_lname = billingDetails.BillingLastName.Trim();
                oVariables.bill_to_address1 = billingDetails.BillingAddress1.Trim();
                oVariables.bill_to_apt = billingDetails.BillingAddress2.Trim();
                oVariables.bill_to_city = billingDetails.BillingCity.Trim();
                oVariables.bill_to_state = billingDetails.BillingState.Trim();
                oVariables.bill_to_zipcode = billingDetails.BillingZipCode.Trim();
            }

            //Assigning Payment details to OVariable
            oVariables.credit_rule = "CCC";
            oVariables.err = "";

            if (oVariables.CCVars.Count > 0)
            {
                oVariables.CCVars[0].number = billingDetails.CreditCardNumber.Trim();
                oVariables.CCVars[0].type = "";
                oVariables.CCVars[0].expdate = billingDetails.CardExpiryMonth.Trim() + billingDetails.CardExpiryYear.Trim();
                oVariables.CCVars[0].cvv = billingDetails.SecurityCode.Trim();
                oVariables.CCVars[0].zipcode = billingDetails.BillingZipCode.Trim();
                oVariables.total_amt = 0.0;
                oVariables.total_sah = 0.0;
            }
            else
            {
                CCProperties oCCVars = new OrderEngine.CCProperties();
                oVariables.payment_type = "CC";
                oCCVars.number = billingDetails.CreditCardNumber.Trim();
                oCCVars.type = "";
                oCCVars.expdate = billingDetails.CardExpiryMonth.Trim() + billingDetails.CardExpiryYear.Trim();
                oCCVars.cvv = billingDetails.SecurityCode.Trim();
                oCCVars.zipcode = (billingDetails.BillingZipCode != null && billingDetails.BillingZipCode != "") ? billingDetails.BillingZipCode.Trim() : "";// billingDetails.BillingZipCode.Trim();
                oVariables.credit_rule = "CCC";
                oVariables.CCVars.Add(oCCVars);
                oVariables.total_amt = 0.0;
                oVariables.total_sah = 0.0;
            }

            return oVariables;
        }
    }

}