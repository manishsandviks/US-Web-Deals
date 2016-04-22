﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using deals.earlymoments.com.Models;
using OrderEngine;
using System.IO;
using System.Configuration;
using deals.earlymoments.com.Utilities;
using System.Net;
using System.Data;
using webConn;
using System.Web.Script.Serialization;

namespace deals.earlymoments.com.Controllers
{
    public class landersController : Controller
    {
        OrderProcess oProcess = new OrderProcess();
        CommonMethods oComm = new CommonMethods();
        OrderVariables oVariables = new OrderVariables();
        CommonModels oCom = new CommonModels();

        const string genericCustomerServiceMsg =
            "We're sorry we were unable to process your request, please try again or call Customer Care toll-free at 866-984-0188 Monday-Friday between 8am and 4pm east coast time.";

        [PreserveQueryString]
        public ActionResult home()
        {
            if (Session["NewOrderDetails"] != null)
            {
                ViewBag.ButtonText = "My FREE Frozen Essential Guide is already on its way.  Continue with my order.";
                ViewBag.LastButtonText = "Continue with my Order";
            }
            return View();
        }

        [PreserveQueryString]
        [HttpPost]
        public ActionResult home(FormCollection form)
        {
            if (Session["NewOrderDetails"] != null)
            {
                oVariables = Session["NewOrderDetails"] as OrderVariables;
                return RedirectToAction(oVariables.lastPageClientOn, "landers");
            }
            return RedirectToAction("shipping", "landers");
        }

        [PreserveQueryString]
        public ActionResult shipping()
        {
            if (Session["NewOrderDetails"] != null)
            {
                return RedirectToAction("home", "landers");
            }
            // ViewBag.Email = !string.IsNullOrEmpty(email) ? email : "";
            ViewData["StatesList"] = UtilitiesModels.GetStateNameList();
            ViewData["GenderList"] = UtilitiesModels.GetChildGenderNameList();
            ViewData["MonthList"] = UtilitiesModels.GetMonthNameList();
            ViewData["YearList"] = UtilitiesModels.GetCardExpiryYearList();
            return View();
        }

        [PreserveQueryString]
        [HttpPost]
        public ActionResult shipping(FormCollection form, ShippingModels.ShoppingOrder shipping)
        {
            ViewData["StatesList"] = UtilitiesModels.GetStateNameList();
            ViewData["GenderList"] = UtilitiesModels.GetChildGenderNameList();
            ViewData["MonthList"] = UtilitiesModels.GetMonthNameList();
            ViewData["YearList"] = UtilitiesModels.GetCardExpiryYearList();
            try
            {
                if (shipping.isBillingSameToShipping)
                {
                    shipping.BillingFirstName = shipping.ShippingFirstName;
                    shipping.BillingLastName = shipping.ShippingLastName;
                    shipping.BillingAddress1 = shipping.ShippingAddress1;
                    shipping.BillingCity = shipping.ShippingCity;
                    shipping.BillingZipCode = shipping.ShippingZipCode;
                    shipping.CCBillZipCode = shipping.BillingZipCode;
                }

                if (ModelState.IsValid == false)
                {
                    var message = string.Join("<br/>", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                    ViewBag.ErrorMsg = message.ToString();
                    return View();
                }

                oVariables = oProcess.GetOfferAndPageDetails("disney_tripwire");

                if ((string)Request.QueryString["vendorcode"] != null) { oVariables.vendor_id = (string)Request.QueryString["vendorcode"]; }
                if ((string)Request.QueryString["key"] != null) { oVariables.vendor_data2 = (string)Request.QueryString["key"]; }
                if ((string)Request.QueryString["vc"] != null) { oVariables.vendor_id = (string)Request.QueryString["vc"]; }
                if ((string)Request.QueryString["pc"] != null) { oVariables.promotion_code = (string)Request.QueryString["pc"]; }
                if ((string)Request.QueryString["aff_id"] != null) { oVariables.vendor_data1 = (string)Request.QueryString["aff_id"]; } if ((string)Request.QueryString["tracking"] != null) { oVariables.vendor_cust_ref_id = (string)Request.QueryString["tracking"]; }
                if ((string)Request.QueryString["src"] != null) { oVariables.pcode_pos_8 = (string)Request.QueryString["src"]; }
                if ((string)Request.QueryString["seg"] != null) { oVariables.pcode_segment = (string)Request.QueryString["seg"]; } if ((string)Request.QueryString["aff_id2"] != null) { oVariables.vendor_data2 = (string)Request.QueryString["aff_id2"]; }

                // oVariables.referring_url = System.Web.HttpContext.Current.Request.Url.ToString();

                //string queryStr = "";
                //if (System.Web.HttpContext.Current.Request.UrlReferrer != null)
                //{
                //    queryStr = System.Web.HttpContext.Current.Request.UrlReferrer.Query;
                //    string ref_url = System.Web.HttpContext.Current.Request.Url.ToString();
                //    oVariables.referring_url = ref_url + queryStr;
                //}
                string ref_url = System.Web.HttpContext.Current.Request.Url.ToString();
                oVariables.referring_url = ref_url;

                oVariables = ShippingModels.AssignShoppingDetailsToOrderVariable(oVariables, shipping);

                oVariables.has_shopping_Cart = true;

                //Submitting shipping details to order engin for order process
                //Code commented for passing to payment page. 
                oVariables = oProcess.OrderSubmit(oVariables);
                if (oVariables != null)
                {
                    if (oVariables.order_id > 0)
                    {
                        oVariables.lastPageClientOn = "upsell_offer1";
                        Session.Add("NewOrderDetails", oVariables);
                        return RedirectToAction("upsell_offer1", "landers");
                    }
                    else
                    {
                        if (oVariables.err.Length >= 0)
                        {
                            if ((oVariables.order_status == "X") || (oVariables.order_status == "F"))
                            {
                                return RedirectToAction("orderstatus", "Home");
                            }
                            else if (oVariables.err.Length > 0)
                            {
                                ViewBag.ErrorMsg = oVariables.err;
                                oVariables.err = oVariables.err.Replace("<br>", "\\r\\n");
                            }
                            else if ((oVariables.order_status == "N") || (oVariables.redirect_page.Length > 0))
                            {
                                Session.Add("NewSBMDetails", oVariables);
                                return View();
                                //return RedirectToAction("upsell_offer1", "landers");
                            }
                            else
                            {
                                ViewBag.ErrorMsg = oVariables.err;
                                oVariables.err = oVariables.err.Replace("<br>", "\\r\\n");
                            }
                        }
                    }
                }
                else
                {
                    Session["NewSBMDetails"] = null;
                    return RedirectToAction("orderstatus", "Home");
                }
            }
            catch (Exception ex)
            {
                string page_log = "Exception raised. Exception: " + ex.Message.ToString() + "<br>";

                ViewBag.ErrorMsg = ex.Message.ToString();
                string s = "";
                oCom.SendEmail(HttpContext.Request.Url.ToString() + "<br>ex.message = " + ex.Message.ToString() + "<br> Additional Information - " + page_log + ".<br> Browser Details....<br>" + s);
                //return RedirectToAction("shipping", "landers", new { uniqueUri = Request.RequestContext.RouteData.Values["uniqueUri"] });
                return View();
            }
            finally
            {
                oVariables = null;
                oComm = null;
                oProcess = null;
            }
            // return RedirectToAction("shipping", "landers", new { uniqueUri = Request.RequestContext.RouteData.Values["uniqueUri"] });
            return View();
        }

        [PreserveQueryString]
        public ActionResult upsell_offer1()
        {
            if (Session["NewOrderDetails"] != null)
            {
                oVariables = Session["NewOrderDetails"] as OrderVariables;
                if (oVariables.lastPageClientOn != "upsell_offer1")
                    return RedirectToAction(oVariables.lastPageClientOn, "landers");
            }
            if (Session["NewOrderDetails"] == null)
            {
                return RedirectToAction("home", "landers");
            }

            return View();
        }

        [PreserveQueryString]
        [HttpPost]
        public ActionResult upsell_offer1(string SubmitButton, FormCollection form)
        {
            try
            {
                if (Session["NewOrderDetails"] != null)
                {
                    oVariables = Session["NewOrderDetails"] as OrderVariables;
                    if (oVariables != null)
                    {
                        if (!string.IsNullOrEmpty(SubmitButton) && SubmitButton.Contains("Yes"))
                        {
                            oVariables.applyDupeCheck = false;
                            oVariables.applyEnhancedCTI = false;
                            oVariables.has_shopping_Cart = true;
                            // oVariables.shopping_cart_items_remove = oVariables.shopping_cart_items;
                            oVariables.shopping_cart_items = "19632368";

                            oVariables.err = "";
                            oVariables.payment_type = "CC";
                            oVariables.total_amt = 0.00;
                            oVariables.total_sah = 0.00;
                            oVariables.order_status = "N";

                            oVariables.isOrderUpgraded = true;
                            oVariables.cancelOriginalOrder = true;
                            oVariables.originalOrderId = oVariables.order_id;

                            oProcess.OrderSubmit(oVariables);

                            //code for adding upsell by api call commented
                            //isUpsellAdded = addUpsellToCart(order_id, campaign_id, offer_id, upsell_id);
                            //if (isUpsellAdded)
                            //{
                            //    Session["NewOrderDetails"] = oVariables;
                            //}
                            if (oVariables != null)
                            {
                                if (oVariables.order_id > 0)
                                {
                                    oVariables.lastPageClientOn = "upsell_offer2";
                                    Session.Add("NewOrderDetails", oVariables);
                                    return RedirectToAction("upsell_offer2", "landers");
                                }
                                else
                                {
                                    if (oVariables.err.Length >= 0)
                                    {
                                        if ((oVariables.order_status == "X") || (oVariables.order_status == "F"))
                                        {
                                            return RedirectToAction("orderstatus", "Home");
                                        }
                                        else if (oVariables.err.Length > 0)
                                        {
                                            ViewBag.ErrorMsg = oVariables.err;
                                            oVariables.err = oVariables.err.Replace("<br>", "\\r\\n");
                                        }
                                        else if ((oVariables.order_status == "N") || (oVariables.redirect_page.Length > 0))
                                        {
                                            Session.Add("NewSBMDetails", oVariables);
                                            return View();
                                            // return RedirectToAction("upsell_offer1", "landers", new { uniqueUri = Request.RequestContext.RouteData.Values["uniqueUri"] });
                                        }
                                        else
                                        {
                                            ViewBag.ErrorMsg = oVariables.err;
                                            oVariables.err = oVariables.err.Replace("<br>", "\\r\\n");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Session["NewSBMDetails"] = null;
                                return RedirectToAction("orderstatus", "Home", new { uniqueUri = Request.RequestContext.RouteData.Values["uniqueUri"] });
                            }

                        }
                        return RedirectToAction("upsell_offer2", "landers");
                    }
                    else
                    {
                        ViewBag.ErrorMsg = genericCustomerServiceMsg;
                    }
                }
                else
                {
                    ViewBag.ErrorMsg = genericCustomerServiceMsg;
                }
                // return RedirectToAction("upsell_offer1", "landers", new { uniqueUri = Request.RequestContext.RouteData.Values["uniqueUri"] });
                return View();

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message.ToString();
                if (oVariables != null)
                {
                    oCom.SendEmail("Page_Load() <br />Exception Raised from Confirmaiton Page in EM Landers. Exception: " + ex.Message.ToString() + ".<br />More Data (Offer URL): "
                      + oVariables.referring_url + ". <br />More Data (Order Id):" + oVariables.order_id);
                }
                //return RedirectToAction("upsell_offer1", "landers", new { uniqueUri = Request.RequestContext.RouteData.Values["uniqueUri"] });
                return View();
            }
            finally
            {
                //  Session["NewOrderDetails"] = null;
                oVariables = null;
                oComm = null;
                oProcess = null;
            }

            //return RedirectToAction("upsell_offer1", "landers", new { uniqueUri = Request.RequestContext.RouteData.Values["uniqueUri"] });
            // return View();
        }

        [PreserveQueryString]
        public ActionResult upsell_offer2()
        {
            if (Session["NewOrderDetails"] == null)
            {
                return RedirectToAction("home", "landers");
            }
            return View();
        }

        [PreserveQueryString]
        [HttpPost]
        public ActionResult upsell_offer2(string SubmitButton, FormCollection form)
        {
            try
            {
                if (Session["NewOrderDetails"] != null)
                {
                    oVariables = Session["NewOrderDetails"] as OrderVariables;
                    if (oVariables != null)
                    {
                        if (!string.IsNullOrEmpty(SubmitButton) && SubmitButton.IndexOf("Yes", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            oVariables.applyDupeCheck = false;
                            oVariables.applyEnhancedCTI = false;
                            oVariables.has_shopping_Cart = true;
                            //oVariables.shopping_cart_items_remove = "19632366,19632367";
                            oVariables.shopping_cart_items = "19632370,19632371";

                            oVariables.err = "";
                            oVariables.payment_type = "CC";
                            oVariables.total_amt = 0.00;
                            oVariables.total_sah = 0.00;
                            oVariables.order_status = "N";

                            oVariables.isOrderUpgraded = true;
                            oVariables.cancelOriginalOrder = true;
                            oVariables.originalOrderId = oVariables.order_id;

                            oProcess.OrderSubmit(oVariables);

                            //code for adding upsell by api call commented
                            //isUpsellAdded = addUpsellToCart(order_id, campaign_id, offer_id, upsell_id);
                            //if (isUpsellAdded)
                            //{
                            //    Session["NewOrderDetails"] = oVariables;
                            //}
                            if (oVariables != null)
                            {
                                if (oVariables.order_id > 0)
                                {
                                    oVariables.lastPageClientOn = "thankyou";
                                    Session.Add("NewOrderDetails", oVariables);
                                    return RedirectToAction("thankyou", "landers");
                                }
                                else
                                {
                                    if (oVariables.err.Length >= 0)
                                    {
                                        if ((oVariables.order_status == "X") || (oVariables.order_status == "F"))
                                        {
                                            return RedirectToAction("orderstatus", "Home");
                                        }
                                        else if (oVariables.err.Length > 0)
                                        {
                                            ViewBag.ErrorMsg = oVariables.err;
                                            oVariables.err = oVariables.err.Replace("<br>", "\\r\\n");
                                        }
                                        else if ((oVariables.order_status == "N") || (oVariables.redirect_page.Length > 0))
                                        {
                                            Session.Add("NewSBMDetails", oVariables);
                                            return View();
                                            //return RedirectToAction("upsell_offer2", "landers", new { uniqueUri = Request.RequestContext.RouteData.Values["uniqueUri"] });
                                        }
                                        else
                                        {
                                            ViewBag.ErrorMsg = oVariables.err;
                                            oVariables.err = oVariables.err.Replace("<br>", "\\r\\n");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Session["NewSBMDetails"] = null;
                                return RedirectToAction("orderstatus", "Home", new { uniqueUri = Request.RequestContext.RouteData.Values["uniqueUri"] });
                            }

                        }
                        return RedirectToAction("thankyou", "landers");
                    }
                    else
                    {
                        ViewBag.ErrorMsg = genericCustomerServiceMsg;
                    }
                }
                else
                {
                    ViewBag.ErrorMsg = genericCustomerServiceMsg;
                }
                //return RedirectToAction("upsell_offer2", "landers", new { uniqueUri = Request.RequestContext.RouteData.Values["uniqueUri"] });
                return View();

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message.ToString();
                if (oVariables != null)
                {
                    oCom.SendEmail("Page_Load() <br />Exception Raised from Confirmaiton Page in EM Landers. Exception: " + ex.Message.ToString() + ".<br />More Data (Offer URL): "
                      + oVariables.referring_url + ". <br />More Data (Order Id):" + oVariables.order_id);
                }
                //return RedirectToAction("upsell_offer2", "landers", new { uniqueUri = Request.RequestContext.RouteData.Values["uniqueUri"] });
                return View();
            }
            finally
            {
                //  Session["NewOrderDetails"] = null;
                oVariables = null;
                oComm = null;
                oProcess = null;
            }
        }

        [PreserveQueryString]
        public ActionResult thankyou()
        {
            string template = "";
            string shipall = "";
            string orderSummary = "";
            string shipping_address = "";
            string billing_address = "";
            try
            {
                if (Session["NewOrderDetails"] != null)
                {
                    oVariables = new OrderVariables();
                    oVariables = Session["NewOrderDetails"] as OrderVariables;
                    if (oVariables != null)
                    {
                        shipall = ((string)Request.QueryString["shipall"] != null) ? (string)Request.QueryString["shipall"] : "false";
                        ViewBag.product = oVariables.proj_desc;
                        ViewBag.special_text = oVariables.special_text;
                        ViewBag.order_id = oVariables.order_id.ToString();

                        ViewBag.amt = oVariables.total_amt.ToString();
                        ViewBag.Project = oVariables.project;
                        string CCNumber = oVariables.CCVars[0] != null ? oVariables.CCVars[0].number : "";

                        ViewBag.CreditCardNumber = EncryptCreditCard(CCNumber);
                        string ccExpiryDate = oVariables.CCVars[0] != null ? oVariables.CCVars[0].expdate : "";
                        ViewBag.CardExpiry = FormatCCExpiryDate(ccExpiryDate);
                        shipping_address = oCom.Confirmation_ShippingAddress(oVariables);
                        ViewBag.ShippingAddress = shipping_address;
                        billing_address = oCom.Confirmation_BillingAddress(oVariables, template);
                        ViewBag.BillingAddress = billing_address;

                        orderSummary = oCom.ResponsiveConfirmation_GiftingProducts(oVariables, false, "", false);

                        ViewBag.Cart = orderSummary;
                        string email = oVariables.email.ToString();
                        ViewBag.email = email;

                        if (!string.IsNullOrEmpty(email))
                        {
                            ProcessConfirmationEmail(oVariables, shipping_address, billing_address, shipall);
                            BuildDynamicPixel(oVariables);
                            //internalPixel = "<img src='https://ping.earlymoments.com/conversion.ashx?o=" + oVariables.order_id + "&e=" + oVariables.email + "' width='1' height='1' border='0' /><iframe src='https://services.earlymoments.com/ping/p.ashx?source=7630&brand=e&data=" + oVariables.order_id.ToString() + "&type=ifr' height='1' width='1' frameborder='0'></iframe><br /><img src='https://services.earlymoments.com/ping/p.ashx?source=7630&brand=e&data=" + oVariables.order_id.ToString() + "&type=img' width='1' height='1' border='0' />";
                            //internalPixel = "<iframe src='https://services.earlymoments.com/ping/p.ashx?source=7630&brand=e&data=" + oVariables.order_id.ToString() + "&type=ifr' height='1' width='1' frameborder='0'></iframe><br /><img src='https://services.earlymoments.com/ping/p.ashx?source=7630&brand=e&data=" + oVariables.order_id.ToString() + "&type=img' width='1' height='1' border='0' />";
                            //Session["IsPostBack"] = true;

                            FirePostBackPixel(oVariables);
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMsg = genericCustomerServiceMsg;
                    }
                }
                else
                {
                    ViewBag.ErrorMsg = genericCustomerServiceMsg;
                }
                return View();
            }
            catch (Exception ex)
            {
                oCom.SendEmail("Page_Load() <br />Exception Raised from Confirmaiton Page in EM Landers. Exception: " + ex.Message.ToString() + ".<br />More Data (Offer URL): "
                  + oVariables.referring_url + ". <br />More Data (Order Id):" + oVariables.order_id);
                return View();
            }
            finally
            {
                Session["NewOrderDetails"] = null;
                oVariables = null;
                oComm = null;
                oProcess = null;
            }
        }

        private static bool addUpsellToCart(string orderId, string campaignId, string offerId, string upsellId)
        {
            string serviceUrl = System.Configuration.ConfigurationManager.AppSettings["iservice"].ToString();
            serviceUrl += "addItemToOrder/?token=741889E3-4565-40A1-982A-F15F7A923D72&PromotionId=" + campaignId + "&OrderId=" + orderId + "&UpsellId=" + upsellId + "&OfferCodes=" + offerId + "&format=json";
            connectIServices connectIServices = new connectIServices();
            try
            {
                string obj = connectIServices.returnJson(serviceUrl);
                var yourOjbect = new JavaScriptSerializer().DeserializeObject(obj);
                List<object> offerList = new List<object>();

                List<object> resultList = new List<object>();
                Dictionary<string, object> dictStr = new Dictionary<string, object>();
                dictStr = (Dictionary<string, object>)yourOjbect;
                var result = dictStr.Where(b => b.Key == "response").Select(b => b.Value).ToList();

                System.Collections.Generic.Dictionary<string, object> book = new Dictionary<string, object>();
                book = (System.Collections.Generic.Dictionary<string, object>)result.FirstOrDefault();
                var O_bookDesc = book.Where(b => b.Key == "processed").Select(b => b.Value).FirstOrDefault();
                if (yourOjbect != null && result != null)
                {
                    if (O_bookDesc != null)
                        return Convert.ToBoolean(O_bookDesc);
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        private string EncryptCreditCard(string CCNumber)
        {
            string encryptedCC = "";
            encryptedCC = "XXXX-XXXX-XXXX-XXXX";
            if (!string.IsNullOrEmpty(CCNumber) && CCNumber.Length > 0)
            {
                int clen = CCNumber.Length;
                if (clen > 0 && (clen == 15 || clen == 16))
                    encryptedCC = CCNumber.Insert(4, "-").Insert(9, "-").Insert(14, "-");
                else if (clen > 0 && clen == 17)
                    encryptedCC = CCNumber.Insert(4, "-").Insert(8, "-").Insert(12, "-").Insert(16, "-");

                switch (clen)
                {
                    case 15:
                        encryptedCC = "XXXX-XXXX-XXX-" + CCNumber.Substring(clen - 4, 4);
                        break;
                    case 16:
                        encryptedCC = "XXXX-XXXX-XXXX-" + CCNumber.Substring(clen - 4, 4);
                        break;
                    case 17:
                        encryptedCC = "XXXX-XXX-XXX-XXX-" + CCNumber.Substring(clen - 4, 4);
                        break;
                    default:
                        encryptedCC = "XXXX-XXXX-XXXX-" + CCNumber.Substring(clen - 4, 4);
                        break;
                }
            }
            return encryptedCC;
        }

        private string FormatCCExpiryDate(string ExpDate)
        {
            string formatedDate = "";
            formatedDate = ExpDate.Insert(2, "-");
            return formatedDate;
        }

        private void ProcessConfirmationEmail(OrderVariables oVariables, string ship_address, string bill_address, string shipall)
        {
            var oComm = new CommonModels();
            try
            {
                if (oVariables.order_id <= 0) return;
                var oProcess = new OrderEngine.OrderProcess();


                var tempBillFname = oVariables.bill_to_fname.Length > 0
                    ? oVariables.bill_to_fname.ToUpper().Substring(0, 1) +
                      oVariables.bill_to_fname.ToLower().Substring(1)
                    : "";
                string tempBillLname = oVariables.bill_to_lname.Length > 0
                    ? oVariables.bill_to_lname.ToUpper().Substring(0, 1) +
                      oVariables.bill_to_lname.ToLower().Substring(1)
                    : "";

                const string optoutText = "If you no longer want to receive this order, please click on the following link within 24 hours: <a href='http://www.earlymoments.com/optout'"
                                           +
                                           "target='_blank'>Cancel Order</a><br /><br />If you are unable to click on the above link, please copy and paste the below link"
                                           + " in the browser: http://www.earlymoments.com/optout<br /><br />";

                string projectSpecificText = "";

                if (oVariables.project.In("DBU"))
                {
                    projectSpecificText =
                        "<br><div style='background-color: Yellow; padding: 15px;'><strong>Your companion ebooks are waiting for you!</strong><br />Your Disney ebooks are available now through the official Early Moments " +
                        "<a href='https://services.earlymoments.com/ping/redirect.ashx?newUrl=https://itunes.apple.com/us/app/early-moments/id661335739?mt=8&source=7635&brand=e&data=!_OrderNumber_!&type=red'" +
                        " target='_blank'>iPad app</a>. Download the FREE Early Moments app from <a href='https://services.earlymoments.com/ping/redirect.ashx?newUrl=https://itunes.apple.com/us/app/early-moments/id661335739?mt=8&source=7635&brand=e&data=!_OrderNumber_!&type=red' target='_blank'>iTunes</a>, and follow the instructions to login for the first time.<br />" +
                        "<a href='https://services.earlymoments.com/ping/redirect.ashx?newUrl=https://itunes.apple.com/us/app/early-moments/id661335739?mt=8&source=7635&brand=e&data=!_OrderNumber_!&type=red' target='_blank'><img src='https://enrollments.earlymoments.com/assets/images/available_on_the_app_store.png' alt='Earlymoments.com' width='130' height='35' border='0' /></a>" +
                        "<br /><br />If you are not using an iPad, <a href='https://services.earlymoments.com/ping/redirect.ashx?newUrl=https://ebooks.earlymoments.com/default.aspx?src=cemail&source=7636&brand=e&data=!_OrderNumber_!&type=red' target='_blank'>click here</a> and follow the instructions to use our eReader.<br /></div>";

                    projectSpecificText = projectSpecificText.Replace("!_OrderNumber_!",
                        Convert.ToString(oVariables.order_id));
                }

                const string nonOptputText = "Visit us online at <a href='http://www.earlymoments.com' target='_blank'>www.EarlyMoments.com</a><br />";

                string tmpStr =
                    "<tr bgcolor='#eeeeee'><td style='width: 293px;'><div style='color: #333333; float: left;'><strong>Item Description</strong></div></td><td width='60px' valign='middle' align='right'><div style='color: #333333; float: left; display:table-cell; vertical-align:middle;'><strong>Item Price</strong></div></td></tr>";
                string space_column = "<tr height='5px''><td colspan='2'></td></tr>";

                foreach (OrderEngine.ShippingVariables oShipVars in oVariables.ShipVars)
                {
                    if (oShipVars.selected)
                    {
                        if ((oComm.OfferItemsDisplayType(oShipVars.OfferVars) > 1) ||
                            (oShipVars.OfferVars.Count == 1))
                        {
                            tmpStr = oShipVars.OfferVars.Aggregate(tmpStr,
                                (current, oOffers) =>
                                    current +
                                    ("<tr><td style='width: 293px;' valign='top' align='left'>" +
                                     oOffers.item_desc +
                                     "</td><td style='width: 60px;' valign='top' align='right'><div style='float: right; display:table-cell; vertical-align:middle;'>" +
                                     ((oOffers.item_cost == 0)
                                         ? ((oComm.GetPriceDisplayType(oOffers.oeprop, oOffers.item_cost) ==
                                             "")
                                             ? String.Format("{0:c}", oOffers.item_cost * oShipVars.quantity)
                                             : "<strong>" +
                                               oComm.GetPriceDisplayType(oOffers.oeprop, oOffers.item_cost) +
                                               "</strong>")
                                         : String.Format("{0:c}", oOffers.item_cost * oShipVars.quantity)) +
                                     "</div></td></tr>"));
                            tmpStr += space_column;
                        }
                        else
                        {
                            string tmpProdlist = oShipVars.OfferVars.Aggregate("",
                                (current, oOffers) => current + (oOffers.item_desc + "<br />"));
                            tmpStr += "<tr><td style='width: 293px;' valign='top' align='left'>" +
                                       tmpProdlist +
                                       "</td><td style='width: 60px;' valign='top' align='right'><div style='float: right; display:table-cell; vertical-align:middle;'>" +
                                       ((oShipVars.unit_price == 0)
                                           ? "<strong>FREE</strong>"
                                           : String.Format("{0:c}", oShipVars.unit_price * oShipVars.quantity)) +
                                       "</div></td></tr>";
                            tmpStr += space_column;
                        }
                    }
                }

                const string shipping = "<tr><td style='width: 293px;' valign='top' align='left'>Shipping and Handling</td><td style='width: 60px;' valign='top' align='right'><div style='float: right; display:table-cell; vertical-align:middle;'>!_ship_!</div></td></tr>";

                if (!oVariables.ship_item_listed)
                {
                    tmpStr += shipping.Replace("!_ship_!",
                        ((oVariables.total_sah == 0)
                            ? "<strong>FREE</strong>"
                            : String.Format("{0:c}", oVariables.total_sah)));
                }

                var pixel =
                    "<iframe src='https://services.earlymoments.com/ping/p.ashx?source=7629&brand=e&data=" +
                    oVariables.email.Trim() +
                    "&type=ifr' height='1' width='1' frameborder='0'></iframe><br /><img src='https://services.earlymoments.com/ping/p.ashx?source=7629&brand=e&data=" +
                    oVariables.email.Trim() + "&type=img' width='1' height='1' border='0' />";

                try
                {
                    oComm.ProcessConfirmationEmails(oVariables.email
                        , tempBillFname
                        , tempBillLname
                        , oVariables.proj_desc
                        , tmpStr
                        , oVariables.special_text
                        , (oVariables.opt_out == "Y" ? optoutText : nonOptputText)
                        , bill_address
                        , ship_address
                        , oVariables.order_id.ToString()
                        , pixel
                        , String.Format("{0:c}", oVariables.tax_amt)
                        ,
                        String.Format("{0:c}", oVariables.total_amt + oVariables.total_sah + oVariables.tax_amt)
                        , projectSpecificText);
                }
                catch (Exception ex)
                {
                    oComm.SendEmail("", "", "EM Confirmation Page: Important",
                        "Error while logging Confirmation email to Database - error: " + ex.Message.ToString(),
                        "");
                }
            }
            catch (Exception ex)
            {
                oComm.SendEmail("ProcessConfirmationEmail() <br />Exception Raised from Confirmaiton Page in EM Landers. Exception: " + ex.Message.ToString() + ".<br />More Data (Offer URL): "
                    + oVariables.referring_url + ". <br />More Data (Order Id):" + oVariables.order_id);
            }
        }
        private void BuildDynamicPixel(OrderVariables oVariables)
        {
            CommonModels oComm = new CommonModels();
            string vendor_tracking = "";
            string aff_id = "";
            string aff_id2 = "";

            string finalpixel = string.Empty;
            DataTable pixeltb = new DataTable();
            DataTable vendparamstb = new DataTable();
            string pixelstring = string.Empty;

            if (oVariables.vendor_id.ToString().Trim().Length == 4)
            {
                try
                {

                    pixeltb = Dynamic_Pixel(oVariables, "get_vndr_details", oVariables.vendor_id.ToString().Trim().ToUpper());
                    //Building Pixel
                    if (pixeltb != null && pixeltb.Rows.Count > 0 && pixeltb.Rows[0]["pixel_hardcoded"].ToString().Trim() == "N")
                    {
                        vendparamstb = Dynamic_Pixel(oVariables, "GetVendorPixelData", oVariables.vendor_id.ToString().Trim().ToUpper());
                        //Raw Pixel
                        pixelstring = pixeltb.Rows[0]["pixel"].ToString().Trim();
                        if (vendparamstb != null && vendparamstb.Rows.Count >= 1)
                        {
                            string[] vendor_var = new string[vendparamstb.Rows.Count];
                            string[] sys_var = new string[vendparamstb.Rows.Count];

                            int i = 0;
                            foreach (DataRow dr in vendparamstb.Rows)
                            {
                                try
                                {
                                    vendor_var[i] = dr["vendor_var"].ToString().Trim();
                                    sys_var[i] = dr["system_var"].ToString().Trim();
                                    i = i + 1;
                                }
                                catch (Exception ex)
                                {
                                    oComm.SendEmail("BuildDynamicPixel() <br />Exception Raised from Confirmaiton Page in EM Landers. Exception: " + ex.Message.ToString() + ".<br />More Data (Offer URL): "
                                        + oVariables.referring_url + ". <br />More Data (Order Id):" + oVariables.order_id);
                                }
                            }

                            //pixelstring = pixeltb.Rows[0]["pixel"].ToString().Trim();
                            if (((string)Request.QueryString["tracking"]) != null) { vendor_tracking = ((string)Request.QueryString["tracking"]).ToString().Trim(); }
                            if (((string)Request.QueryString["aff_id"]) != null) { aff_id = ((string)Request.QueryString["aff_id"]).ToString().Trim(); }
                            if (((string)Request.QueryString["aff_id2"]) != null) { aff_id2 = ((string)Request.QueryString["aff_id2"]).ToString().Trim(); }

                            double base_amt = 0.00;

                            foreach (OrderEngine.ShippingVariables oShipVars in oVariables.ShipVars)
                                if (oShipVars.selected && oShipVars.offer_type.In("B", "O"))
                                    foreach (OrderEngine.OfferProperties oOffers in oShipVars.OfferVars)
                                        base_amt += oOffers.item_cost;


                            for (int j = 0; j < sys_var.Length; j++)
                            {

                                switch (sys_var[j].Trim())
                                {
                                    //[vendor_tracking]
                                    case "VDT":
                                        pixelstring = pixelstring.Replace(vendor_var[j], vendor_tracking);
                                        break;
                                    //[order_id]
                                    case "OID":
                                        pixelstring = pixelstring.Replace(vendor_var[j], oVariables.order_id.ToString());
                                        break;
                                    //[aff_id]
                                    case "AFI1":
                                        pixelstring = pixelstring.Replace(vendor_var[j], aff_id);
                                        break;
                                    //[aff_id2]
                                    case "AFI2":
                                        pixelstring = pixelstring.Replace(vendor_var[j], aff_id2);
                                        break;
                                    //[email]
                                    case "EMA":
                                        pixelstring = pixelstring.Replace(vendor_var[j], oVariables.email.ToString());
                                        break;
                                    //[promotion_code]
                                    case "PRC":
                                        pixelstring = pixelstring.Replace(vendor_var[j], oVariables.promotion_code.ToString());
                                        break;
                                    //[SubTotal]
                                    case "SUBTO":
                                        pixelstring = pixelstring.Replace(vendor_var[j], (oVariables.total_amt - oVariables.tax_amt).ToString());
                                        break;
                                    //[total_amt]
                                    case "TOA":
                                        pixelstring = pixelstring.Replace(vendor_var[j], oVariables.total_amt.ToString());
                                        break;
                                    case "BAMT":
                                        pixelstring = pixelstring.Replace(vendor_var[j], base_amt.ToString());
                                        break;
                                    case "PRJ":
                                        pixelstring = pixelstring.Replace(vendor_var[j], oVariables.project);
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                    else if (pixeltb.Rows[0]["isPostBack"].ToString().Trim() == "N")
                    {
                        vendparamstb = null;
                        //Raw Pixel
                        pixelstring = pixeltb.Rows[0]["pixel"].ToString().Trim();
                    }


                }
                catch (Exception ex)
                {
                    oComm.SendEmail("BuildDynamicPixel() <br />Exception Raised from Confirmaiton Page in EM Landers. Exception: " + ex.Message.ToString() + ".<br />More Data (Offer URL): "
                        + oVariables.referring_url + ". <br />More Data (Order Id):" + oVariables.order_id);
                }
            }
            else
            {
                pixelstring = "";
            }
        }

        public DataTable Dynamic_Pixel(OrderVariables oVariables, string SPname, string pixcd)
        {
            CommonModels oComm = new CommonModels();
            dbConn.dbConnector.dbConn oDBConn;
            DataTable pixeltb = new DataTable();
            DataSet VpixData = new DataSet();
            try
            {
                //DBConn and SP execution
                oDBConn = new dbConn.dbConnector.dbConn();
                oDBConn.regKey = ConfigurationManager.AppSettings["SQLConn"].ToString();
                VpixData = oDBConn.ExecSPReturnSingleDS(SPname, pixcd);
                pixeltb = VpixData.Tables[0];
                return pixeltb;
            }
            catch (Exception ex)
            {
                oComm.SendEmail("Dynamic_Pixel() <br />Exception Raised from Confirmaiton Page in EM Landers. Exception: " + ex.Message.ToString() + ".<br />More Data (Offer URL): " + oVariables.referring_url + ". <br />More Data (Order Id):" + oVariables.order_id);
                return null;
            }
            finally
            {
                oDBConn = null;
                pixeltb = null;
                VpixData = null;

            }
        }

        private void FirePostBackPixel(OrderVariables oVariables)
        {
            string SQLString = System.Configuration.ConfigurationManager.AppSettings["SQLConn"].ToString();
            try
            {
                if (oVariables != null && (!string.IsNullOrEmpty(oVariables.transaction_id)) && (oVariables.transaction_id.Trim().Length > 0))
                {
                    string trans_id = oVariables.transaction_id.Trim();
                    string pixelstring = oVariables.pixel.Trim();
                    string conversionPixel = "https://sandvikpublishing.go2cloud.org/aff_lsr?transaction_id=" + trans_id;
                    dbConn.dbConnector.dbConn oDBConn = new dbConn.dbConnector.dbConn();
                    oDBConn.regKey = SQLString;
                    int rowId = 0;
                    rowId = oDBConn.ExecSPReturnInteger("InsertVendorPxlPostBackFeedback", oVariables.order_id, conversionPixel, rowId);
                    oDBConn = null;

                    var client = new WebClient();
                    var content = client.DownloadString(conversionPixel);

                    oDBConn = new dbConn.dbConnector.dbConn();
                    oDBConn.regKey = SQLString;
                    oDBConn.ExecSP("UpdateVendorPxlPostBackFeedback", rowId, string.IsNullOrEmpty(content) ? content.ToString().Trim() : "EMPTY");
                    oDBConn = null;

                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                // dSet = null;
            }
        }

    }
}