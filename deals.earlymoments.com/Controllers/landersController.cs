using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using deals.earlymoments.com.Models;
using deals.earlymoments.com.Utilities;
using OrderEngine;
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

                        orderSummary = oCom.ResponsiveConfirmation_GiftingProducts(oVariables);

                        ViewBag.Cart = orderSummary;
                        string email = oVariables.email.ToString();
                        ViewBag.email = email;

                        //if (!string.IsNullOrEmpty(email))
                        //{
                        //    ProcessConfirmationEmail(oVariables, shipping_address, billing_address, shipall);
                        //    BuildDynamicPixel(oVariables);
                        //    //internalPixel = "<img src='https://ping.earlymoments.com/conversion.ashx?o=" + oVariables.order_id + "&e=" + oVariables.email + "' width='1' height='1' border='0' /><iframe src='https://services.earlymoments.com/ping/p.ashx?source=7630&brand=e&data=" + oVariables.order_id.ToString() + "&type=ifr' height='1' width='1' frameborder='0'></iframe><br /><img src='https://services.earlymoments.com/ping/p.ashx?source=7630&brand=e&data=" + oVariables.order_id.ToString() + "&type=img' width='1' height='1' border='0' />";
                        //    //internalPixel = "<iframe src='https://services.earlymoments.com/ping/p.ashx?source=7630&brand=e&data=" + oVariables.order_id.ToString() + "&type=ifr' height='1' width='1' frameborder='0'></iframe><br /><img src='https://services.earlymoments.com/ping/p.ashx?source=7630&brand=e&data=" + oVariables.order_id.ToString() + "&type=img' width='1' height='1' border='0' />";
                        //    //Session["IsPostBack"] = true;

                        //    FirePostBackPixel(oVariables);
                        //}
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
    }
}