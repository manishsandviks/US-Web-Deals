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
            [StringLength(50, MinimumLength = 3, ErrorMessage = "Shipping First Name cannot be longer than 15 char and less than 3 char.")]
            [DisplayName("First Name")]
            public string ShippingFirstName { get; set; }

            [Required(ErrorMessage = "Shipping Last Name is required.")]
            [StringLength(50, MinimumLength = 3, ErrorMessage = "Shipping Last Name  cannot be longer than 15 char and less than 3 char.")]
            [DisplayName("Last Name")]
            public string ShippingLastName { get; set; }

            [Required(ErrorMessage = "Shipping Address is required.")]
            [StringLength(30, MinimumLength = 5, ErrorMessage = "Shipping Address1 cannot be longer than 30 char and less than 3 char.")]
            [DisplayName("Street Address")]
            public string ShippingAddress1 { get; set; }

            [StringLength(30, ErrorMessage = "Shipping Address2 cannot be longer than 30 characters.")]
            public string ShippingAddress2 { get; set; }

            [Required(ErrorMessage = "Shipping City is required.")]
            [StringLength(50, ErrorMessage = "Shipping City cannot be longer than 16 characters.")]
            public string ShippingCity { get; set; }

            [Required(ErrorMessage = "Shipping State is required.")]
            [StringLength(2, MinimumLength = 2, ErrorMessage = "Shipping State code cannot be longer than 2 characters and less than 2 char.")]
            public string ShippingState { get; set; }

            [Required(ErrorMessage = "Shipping Zip Code is required.")]
            [StringLength(10, MinimumLength = 5, ErrorMessage = "Shipping Zip Code cannot be longer than 10 char and less than 5 char..")]
            public string ShippingZipCode { get; set; }

            [StringLength(10, ErrorMessage = "Shipping Phone cannot be longer than 10 char.")]
            public string ShippingPhone { get; set; }

            [Required(ErrorMessage = "Shipping Email is required.")]
            [StringLength(50, ErrorMessage = "Shipping Email cannot be longer than 50 characters.")]
            public string ShippingEmail { get; set; }

            [Required(ErrorMessage = "Shipping Confirm Email is required.")]
            [StringLength(50, ErrorMessage = "Shipping Confirm Email cannot be longer than 50 characters.")]
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
                isBonusSelected = true;
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
            [StringLength(50, MinimumLength = 3, ErrorMessage = "Billing First Name cannot be longer than 15 char and less than 3 char.")]
            public string BillingFirstName { get; set; }

            [Required(ErrorMessage = "Billing Last Name is required.")]
            [StringLength(50, MinimumLength = 3, ErrorMessage = "Billing Last Name cannot be longer than 15 char and less than 3 char.")]
            public string BillingLastName { get; set; }

            [Required(ErrorMessage = "Billing Address is required.")]
            [StringLength(30, MinimumLength = 5, ErrorMessage = "Billing Address1 cannot be longer than 30 char and less than 5 char.")]
            public string BillingAddress1 { get; set; }

            [StringLength(30, ErrorMessage = "Billing Address2 cannot be longer than 30 characters.")]
            public string BillingAddress2 { get; set; }

            [Required(ErrorMessage = "Billing City is required.")]
            [StringLength(50, ErrorMessage = "Billing City cannot be longer than 16 characters.")]
            public string BillingCity { get; set; }

            [Required(ErrorMessage = "Billing State is required.")]
            [StringLength(2, MinimumLength = 2, ErrorMessage = "Billing State code can not be longer than 2 characters and less than 2 char")]
            public string BillingState { get; set; }

            [Required(ErrorMessage = "Billing Zip Code is required.")]
            [StringLength(10, MinimumLength = 5, ErrorMessage = "Billing Zip Code cannot be longer than 10 char and less than 5 char.")]
            public string BillingZipCode { get; set; }

            [DisplayName("Is your billing address the same as your shipping address?")]
            public bool isBillingSameToShipping { get; set; }

            [Required(ErrorMessage = "Credit Card is required.")]
            [DisplayName("Credit Card Number")]
            [StringLength(16, MinimumLength = 15, ErrorMessage = "Credit Card Number cannot be longer than 17 digits and less than 15 digits")]
            public string CreditCardNumber { get; set; }

            [Required(ErrorMessage = "Card Expiry Month is required.")]
            [DisplayName("Expiry Month")]
            [StringLength(3, ErrorMessage = "Card Expiry Month cannot be longer than 3 characters.")]
            public string CardExpiryMonth { get; set; }

            [Required(ErrorMessage = "Card Expiry Year is required.")]
            [DisplayName("Expiry Year")]
            [StringLength(4, MinimumLength = 2, ErrorMessage = "Card Expiry Year cannot be longer than 4 characters.")]
            public string CardExpiryYear { get; set; }

            [Required(ErrorMessage = "Security Code is required.")]
            [DisplayName("Security Code")]
            [StringLength(30, MinimumLength = 3, ErrorMessage = "Security Code cannot be longer than 4 char and less tha three char.")]
            public string SecurityCode { get; set; }

            [Required(ErrorMessage = "Billing Zip Code is required.")]
            [DisplayName("Billing Zip Code")]
            [StringLength(10, MinimumLength = 5, ErrorMessage = "Billing Zip Code cannot be longer than 10 char and less than 5 char.")]
            public string CCBillZipCode { get; set; }

            //[Required(ErrorMessage = "Adult DOB Day is required.")]
            //[DisplayName("Adult DOB Day")]
            //[StringLength(2, ErrorMessage = "Adult DOB Day can not be longer than 2 characters.")]
            //public string ADOB_Day { get; set; }

            //[Required(ErrorMessage = "Adult DOB Month is required.")]
            //[DisplayName("Adult DOB Day")]
            //[StringLength(3, ErrorMessage = "Adult DOB Month can not be longer than 2 characters.")]
            //public string ADOB_Month { get; set; }

            //[Required(ErrorMessage = "Adult DOB Year is required.")]
            //[DisplayName("Adult DOB Year")]
            //[StringLength(4, MinimumLength = 2, ErrorMessage = "Adult DOB Year cannot be longer than 4 characters.")]
            //public string ADOB_Year { get; set; }

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
                //ADOB_Day = "";
                //ADOB_Month = "";
                //ADOB_Year = "";
            }

        }

        public class BillingAndPaymentModels
        {
            public BillingDetails BillingAddress { get; set; }
            public PaymentDetails PaymentDetails { get; set; }
        }

        public class ShoppingOrder
        {

            [Required(ErrorMessage = "Shipping First Name is required.")]
            [StringLength(50, MinimumLength = 3, ErrorMessage = "Shipping First Name cannot be longer than 15 char and less than 3 char.")]
            [DisplayName("First Name")]
            public string ShippingFirstName { get; set; }

            [Required(ErrorMessage = "Shipping Last Name is required.")]
            [StringLength(50, MinimumLength = 3, ErrorMessage = "Shipping Last Name  cannot be longer than 15 char and less than 3 char.")]
            [DisplayName("Last Name")]
            public string ShippingLastName { get; set; }

            [Required(ErrorMessage = "Shipping Address is required.")]
            [StringLength(30, MinimumLength = 5, ErrorMessage = "Shipping Address1 cannot be longer than 30 char and less than 3 char.")]
            [DisplayName("Street Address")]
            public string ShippingAddress1 { get; set; }

            [StringLength(30, ErrorMessage = "Shipping Address2 cannot be longer than 30 characters.")]
            public string ShippingAddress2 { get; set; }

            [Required(ErrorMessage = "Shipping City is required.")]
            [StringLength(50, ErrorMessage = "Shipping City cannot be longer than 16 characters.")]
            public string ShippingCity { get; set; }

            [Required(ErrorMessage = "Shipping State is required.")]
            [StringLength(2, MinimumLength = 2, ErrorMessage = "Shipping State code cannot be longer than 2 characters and less than 2 char.")]
            public string ShippingState { get; set; }

            [Required(ErrorMessage = "Shipping Zip Code is required.")]
            [StringLength(10, MinimumLength = 5, ErrorMessage = "Shipping Zip Code cannot be longer than 10 char and less than 5 char..")]
            public string ShippingZipCode { get; set; }

            [StringLength(10, ErrorMessage = "Shipping Phone cannot be longer than 10 char.")]
            public string ShippingPhone { get; set; }

            [Required(ErrorMessage = "Shipping Email is required.")]
            [StringLength(50, ErrorMessage = "Shipping Email cannot be longer than 50 characters.")]
            public string ShippingEmail { get; set; }

            [Required(ErrorMessage = "Shipping Confirm Email is required.")]
            [StringLength(50, ErrorMessage = "Shipping Confirm Email cannot be longer than 50 characters.")]
            public string ShippingConfirmEmail { get; set; }

            [StringLength(50, ErrorMessage = "Child Name cannot be longer than 50 characters.")]
            public string ChildName { get; set; }

            public DateTime? ChildDOB { get; set; }
            public string ChildGender { get; set; }

            public bool isBonusSelected { get; set; }


            [DisplayName("First Name")]
            [StringLength(50, MinimumLength = 3, ErrorMessage = "Billing First Name cannot be longer than 15 char and less than 3 char.")]
            public string BillingFirstName { get; set; }


            [StringLength(50, MinimumLength = 3, ErrorMessage = "Billing Last Name cannot be longer than 15 char and less than 3 char.")]
            public string BillingLastName { get; set; }


            [StringLength(30, MinimumLength = 5, ErrorMessage = "Billing Address1 cannot be longer than 30 char and less than 5 char.")]
            public string BillingAddress1 { get; set; }

            [StringLength(30, ErrorMessage = "Billing Address2 cannot be longer than 30 characters.")]
            public string BillingAddress2 { get; set; }


            [StringLength(50, ErrorMessage = "Billing City cannot be longer than 16 characters.")]
            public string BillingCity { get; set; }


            [StringLength(2, MinimumLength = 2, ErrorMessage = "Billing State code can not be longer than 2 characters and less than 2 char")]
            public string BillingState { get; set; }


            [StringLength(10, ErrorMessage = "Billing Zip Code cannot be longer than 10 char and less than 5 char.")]
            public string BillingZipCode { get; set; }

            [DisplayName("Is your billing address the same as your shipping address?")]
            public bool isBillingSameToShipping { get; set; }

            [Required(ErrorMessage = "Credit Card is required.")]
            [DisplayName("Credit Card Number")]
            [StringLength(16, MinimumLength = 15, ErrorMessage = "Credit Card Number cannot be longer than 17 digits and less than 15 digits")]
            public string CreditCardNumber { get; set; }

            [Required(ErrorMessage = "Card Expiry Month is required.")]
            [DisplayName("Expiry Month")]
            [StringLength(3, ErrorMessage = "Card Expiry Month cannot be longer than 3 characters.")]
            public string CardExpiryMonth { get; set; }

            [Required(ErrorMessage = "Card Expiry Year is required.")]
            [DisplayName("Expiry Year")]
            [StringLength(4, MinimumLength = 2, ErrorMessage = "Card Expiry Year cannot be longer than 4 characters.")]
            public string CardExpiryYear { get; set; }

            [Required(ErrorMessage = "Security Code is required.")]
            [DisplayName("Security Code")]
            [StringLength(30, MinimumLength = 3, ErrorMessage = "Security Code cannot be longer than 4 char and less tha three char.")]
            public string SecurityCode { get; set; }


            [DisplayName("Billing Zip Code")]
            [StringLength(10, ErrorMessage = "CC Billing Zip Code cannot be longer than 10 char and less than 5 char.")]
            public string CCBillZipCode { get; set; }

            public string SecurityCaptch { get; set; }

            public ShoppingOrder()
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
                isBonusSelected = true;
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

        public class ShippingBillingOrder
        {

            [Required(ErrorMessage = "Shipping First Name is required.")]
            [StringLength(50, MinimumLength = 3, ErrorMessage = "Shipping First Name cannot be longer than 15 char and less than 3 char.")]
            [DisplayName("First Name")]
            public string ShippingFirstName { get; set; }

            [Required(ErrorMessage = "Shipping Last Name is required.")]
            [StringLength(50, MinimumLength = 3, ErrorMessage = "Shipping Last Name  cannot be longer than 15 char and less than 3 char.")]
            [DisplayName("Last Name")]
            public string ShippingLastName { get; set; }

            [Required(ErrorMessage = "Shipping Address is required.")]
            [StringLength(30, MinimumLength = 5, ErrorMessage = "Shipping Address1 cannot be longer than 30 char and less than 3 char.")]
            [DisplayName("Street Address")]
            public string ShippingAddress1 { get; set; }

            [StringLength(30, ErrorMessage = "Shipping Address2 cannot be longer than 30 characters.")]
            public string ShippingAddress2 { get; set; }

            [Required(ErrorMessage = "Shipping City is required.")]
            [StringLength(50, ErrorMessage = "Shipping City cannot be longer than 16 characters.")]
            public string ShippingCity { get; set; }

            [Required(ErrorMessage = "Shipping State is required.")]
            [StringLength(2, MinimumLength = 2, ErrorMessage = "Shipping State code cannot be longer than 2 characters and less than 2 char.")]
            public string ShippingState { get; set; }

            [Required(ErrorMessage = "Shipping Zip Code is required.")]
            [StringLength(10, MinimumLength = 5, ErrorMessage = "Shipping Zip Code cannot be longer than 10 char and less than 5 char..")]
            public string ShippingZipCode { get; set; }

            [StringLength(10, ErrorMessage = "Shipping Phone cannot be longer than 10 char.")]
            public string ShippingPhone { get; set; }

            [Required(ErrorMessage = "Shipping Email is required.")]
            [StringLength(50, ErrorMessage = "Shipping Email cannot be longer than 50 characters.")]
            public string ShippingEmail { get; set; }

            [Required(ErrorMessage = "Shipping Confirm Email is required.")]
            [StringLength(50, ErrorMessage = "Shipping Confirm Email cannot be longer than 50 characters.")]
            public string ShippingConfirmEmail { get; set; }

            [StringLength(50, ErrorMessage = "Child Name cannot be longer than 50 characters.")]
            public string ChildName { get; set; }

            public DateTime? ChildDOB { get; set; }
            public string ChildGender { get; set; }

            public bool isBonusSelected { get; set; }


            [DisplayName("First Name")]
            [StringLength(50, MinimumLength = 3, ErrorMessage = "Billing First Name cannot be longer than 15 char and less than 3 char.")]
            public string BillingFirstName { get; set; }


            [StringLength(50, MinimumLength = 3, ErrorMessage = "Billing Last Name cannot be longer than 15 char and less than 3 char.")]
            public string BillingLastName { get; set; }


            [StringLength(30, MinimumLength = 5, ErrorMessage = "Billing Address1 cannot be longer than 30 char and less than 5 char.")]
            public string BillingAddress1 { get; set; }

            [StringLength(30, ErrorMessage = "Billing Address2 cannot be longer than 30 characters.")]
            public string BillingAddress2 { get; set; }


            [StringLength(50, ErrorMessage = "Billing City cannot be longer than 16 characters.")]
            public string BillingCity { get; set; }


            [StringLength(2, MinimumLength = 2, ErrorMessage = "Billing State code can not be longer than 2 characters and less than 2 char")]
            public string BillingState { get; set; }


            [StringLength(10, ErrorMessage = "Billing Zip Code cannot be longer than 10 char and less than 5 char.")]
            public string BillingZipCode { get; set; }

            [DisplayName("Is your billing address the same as your shipping address?")]
            public bool isBillingSameToShipping { get; set; }

            [DisplayName("Credit Card Number")]
            [StringLength(16, MinimumLength = 15, ErrorMessage = "Credit Card Number cannot be longer than 17 digits and less than 15 digits")]
            public string CreditCardNumber { get; set; }

            [DisplayName("Expiry Month")]
            [StringLength(3, ErrorMessage = "Card Expiry Month cannot be longer than 3 characters.")]
            public string CardExpiryMonth { get; set; }

            [DisplayName("Expiry Year")]
            [StringLength(4, MinimumLength = 2, ErrorMessage = "Card Expiry Year cannot be longer than 4 characters.")]
            public string CardExpiryYear { get; set; }

            [DisplayName("Security Code")]
            [StringLength(30, MinimumLength = 3, ErrorMessage = "Security Code cannot be longer than 4 char and less tha three char.")]
            public string SecurityCode { get; set; }

            [DisplayName("Billing Zip Code")]
            [StringLength(10, ErrorMessage = "CC Billing Zip Code cannot be longer than 10 char and less than 5 char.")]
            public string CCBillZipCode { get; set; }
            public string SecurityCaptch { get; set; }
            public int stepNumber { get; set; }


            public ShippingBillingOrder()
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
                isBonusSelected = true;
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
                stepNumber = 0;
            }

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
            oVariables.ShipVars[oVariables.default_shp_id].child_dob = newShippingAddress.ChildDOB != null ? newShippingAddress.ChildDOB.ToString() : "";
            oVariables.ShipVars[oVariables.default_shp_id].child_fname = newShippingAddress.ChildName;

            if (string.IsNullOrEmpty(newShippingAddress.ChildGender) || newShippingAddress.ChildGender == "")
                oVariables.ShipVars[oVariables.default_shp_id].child_gender = "0";
            else
                oVariables.ShipVars[oVariables.default_shp_id].child_gender = newShippingAddress.ChildGender;
            //oVariables.referring_url = HttpContext.Current.Request.Url.ToString();

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
                oVariables.bill_to_apt = (!string.IsNullOrEmpty(billingDetails.BillingAddress2)) ? billingDetails.BillingAddress2.Trim() : "";
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
                oVariables.CCVars[0].zipcode = oVariables.bill_to_zipcode;// billingDetails.BillingZipCode.Trim();
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
                oCCVars.zipcode = oVariables.bill_to_zipcode;// (billingDetails.BillingZipCode != null && billingDetails.BillingZipCode != "") ? billingDetails.BillingZipCode.Trim() : oVariables.bill_to_zipcode; // billingDetails.BillingZipCode.Trim();
                oVariables.credit_rule = "CCC";
                oVariables.CCVars.Add(oCCVars);
                oVariables.total_amt = 0.0;
                oVariables.total_sah = 0.0;
            }

            return oVariables;
        }

        public static OrderVariables AssignShoppingDetailsToOrderVariable(OrderVariables oVariables, ShoppingOrder billingDetails)
        {
            CommonModels oComm = new CommonModels();
            //OrderVariables oVariables = new OrderVariables();
            oVariables.ShipVars[oVariables.default_shp_id].ship_to_fname = billingDetails.ShippingFirstName;
            oVariables.ShipVars[oVariables.default_shp_id].ship_to_lname = billingDetails.ShippingLastName;
            oVariables.ShipVars[oVariables.default_shp_id].ship_to_address1 = billingDetails.ShippingAddress1;
            oVariables.ShipVars[oVariables.default_shp_id].ship_to_apt = billingDetails.ShippingAddress2;
            oVariables.ShipVars[oVariables.default_shp_id].ship_to_city = billingDetails.ShippingCity;
            oVariables.ShipVars[oVariables.default_shp_id].ship_to_state = billingDetails.ShippingState;
            oVariables.ShipVars[oVariables.default_shp_id].ship_to_zipcode = billingDetails.ShippingZipCode;
            oVariables.ShipVars[oVariables.default_shp_id].child_dob = billingDetails.ChildDOB != null ? billingDetails.ChildDOB.ToString() : "";
            oVariables.ShipVars[oVariables.default_shp_id].child_fname = billingDetails.ChildName;

            if (string.IsNullOrEmpty(billingDetails.ChildGender) || billingDetails.ChildGender == "")
                oVariables.ShipVars[oVariables.default_shp_id].child_gender = "0";
            else
                oVariables.ShipVars[oVariables.default_shp_id].child_gender = billingDetails.ChildGender;

            //oVariables.referring_url = HttpContext.Current.Request.Url.ToString();

            oVariables.ip_address = oComm.GetIPAddress();
            oVariables.phone = billingDetails.ShippingPhone;
            oVariables.email = billingDetails.ShippingEmail;
            oVariables.bonus_option = false;

            if (billingDetails.isBonusSelected)
            {
                oVariables.bonus_option = true;
            }

            //Assigning Billing Address details to OVariable
            if (billingDetails.isBillingSameToShipping)
            {
                oVariables.bill_to_fname = oVariables.ShipVars[oVariables.default_shp_id].ship_to_fname;
                oVariables.bill_to_lname = oVariables.ShipVars[oVariables.default_shp_id].ship_to_lname;
                oVariables.bill_to_address1 = oVariables.ShipVars[oVariables.default_shp_id].ship_to_address1;
                oVariables.bill_to_apt = oVariables.ShipVars[oVariables.default_shp_id].ship_to_apt;
                oVariables.bill_to_city = oVariables.ShipVars[oVariables.default_shp_id].ship_to_city;
                oVariables.bill_to_state = oVariables.ShipVars[oVariables.default_shp_id].ship_to_state;
                oVariables.bill_to_zipcode = oVariables.ShipVars[oVariables.default_shp_id].ship_to_zipcode;
            }
            else
            {
                oVariables.bill_to_fname = billingDetails.BillingFirstName.Trim();
                oVariables.bill_to_lname = billingDetails.BillingLastName.Trim();
                oVariables.bill_to_address1 = billingDetails.BillingAddress1.Trim();
                oVariables.bill_to_apt = (!string.IsNullOrEmpty(billingDetails.BillingAddress2)) ? billingDetails.BillingAddress2.Trim() : "";
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

        public static OrderVariables AssignShippingBillingToOrderVariables(OrderVariables oVariables, ShippingBillingOrder newShippingAddress)
        {
            if (newShippingAddress != null && newShippingAddress.stepNumber == 1)
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
                //oVariables.referring_url = HttpContext.Current.Request.Url.ToString();

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
            else if (newShippingAddress != null && newShippingAddress.stepNumber == 1)
            {
                //As we are setting billing and shipping address same on landing page 
                if (newShippingAddress.isBillingSameToShipping == false)
                {
                    oVariables.bill_to_fname = newShippingAddress.BillingFirstName.Trim();
                    oVariables.bill_to_lname = newShippingAddress.BillingLastName.Trim();
                    oVariables.bill_to_address1 = newShippingAddress.BillingAddress1.Trim();
                    oVariables.bill_to_apt = (!string.IsNullOrEmpty(newShippingAddress.BillingAddress2)) ? newShippingAddress.BillingAddress2.Trim() : "";
                    oVariables.bill_to_city = newShippingAddress.BillingCity.Trim();
                    oVariables.bill_to_state = newShippingAddress.BillingState.Trim();
                    oVariables.bill_to_zipcode = newShippingAddress.BillingZipCode.Trim();
                }

                //Assigning Payment details to OVariable
                oVariables.credit_rule = "CCC";
                oVariables.err = "";

                if (oVariables.CCVars.Count > 0)
                {
                    oVariables.CCVars[0].number = newShippingAddress.CreditCardNumber.Trim();
                    oVariables.CCVars[0].type = "";
                    oVariables.CCVars[0].expdate = newShippingAddress.CardExpiryMonth.Trim() + newShippingAddress.CardExpiryYear.Trim();
                    oVariables.CCVars[0].cvv = newShippingAddress.SecurityCode.Trim();
                    oVariables.CCVars[0].zipcode = oVariables.bill_to_zipcode;// newShippingAddress.BillingZipCode.Trim();
                    oVariables.total_amt = 0.0;
                    oVariables.total_sah = 0.0;
                }
                else
                {
                    CCProperties oCCVars = new OrderEngine.CCProperties();
                    oVariables.payment_type = "CC";
                    oCCVars.number = newShippingAddress.CreditCardNumber.Trim();
                    oCCVars.type = "";
                    oCCVars.expdate = newShippingAddress.CardExpiryMonth.Trim() + newShippingAddress.CardExpiryYear.Trim();
                    oCCVars.cvv = newShippingAddress.SecurityCode.Trim();
                    oCCVars.zipcode = oVariables.bill_to_zipcode;// (billingDetails.BillingZipCode != null && billingDetails.BillingZipCode != "") ? billingDetails.BillingZipCode.Trim() : oVariables.bill_to_zipcode; // billingDetails.BillingZipCode.Trim();
                    oVariables.credit_rule = "CCC";
                    oVariables.CCVars.Add(oCCVars);
                    oVariables.total_amt = 0.0;
                    oVariables.total_sah = 0.0;
                }


                return oVariables;
            }
            else
                return null;
        }
    }
}