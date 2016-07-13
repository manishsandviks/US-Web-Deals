using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using deals.earlymoments.com.Models;
using OrderEngine;
using deals.earlymoments.com.Utilities;
using deals.earlymoments.com.Services;

namespace deals.earlymoments.com.Controllers
{
    public class DisneyController : Controller
    {
        //
        // GET: /Diney/
        [PreserveQueryString]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection form, ShippingModels.ShippingAddress shipping)
        {

            return RedirectToAction("Four_for_99", "Disney", new { email = shipping.ShippingEmail });
            //return View();
        }

        #region Four_for_99

        [PreserveQueryString]
        public ActionResult Four_for_99(string email = "")
        {

            ViewBag.Email = !string.IsNullOrEmpty(email) ? email : "";
            ViewData["StatesList"] = UtilitiesModels.GetStateNameList();
            ViewData["GenderList"] = UtilitiesModels.GetChildGenderNameList();
            ViewData["MonthList"] = UtilitiesModels.GetMonthNameList();
            ViewData["YearList"] = UtilitiesModels.GetCardExpiryYearList();
            return View();
        }

        [HttpPost]
        public ActionResult Four_for_99(FormCollection form, ShippingModels.ShoppingOrder shipping, string[] SelectedBooks)
        {
            ViewData["StatesList"] = UtilitiesModels.GetStateNameList();
            ViewData["GenderList"] = UtilitiesModels.GetChildGenderNameList();
            ViewData["MonthList"] = UtilitiesModels.GetMonthNameList();
            ViewData["YearList"] = UtilitiesModels.GetCardExpiryYearList();
            OrderProcess oProcess = new OrderProcess();
            CommonMethods oComm = new CommonMethods();
            OrderVariables oVariables = new OrderVariables();
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

                oVariables = oProcess.GetOfferAndPageDetails("fosina-disney-4for99-secure");

                if ((string)Request.QueryString["vendorcode"] != null) { oVariables.vendor_id = (string)Request.QueryString["vendorcode"]; }
                if ((string)Request.QueryString["key"] != null) { oVariables.vendor_data2 = (string)Request.QueryString["key"]; }
                if ((string)Request.QueryString["vc"] != null) { oVariables.vendor_id = (string)Request.QueryString["vc"]; }
                if ((string)Request.QueryString["pc"] != null) { oVariables.promotion_code = (string)Request.QueryString["pc"]; }
                if ((string)Request.QueryString["aff_id"] != null) { oVariables.vendor_data1 = (string)Request.QueryString["aff_id"]; } if ((string)Request.QueryString["tracking"] != null) { oVariables.vendor_cust_ref_id = (string)Request.QueryString["tracking"]; }
                if ((string)Request.QueryString["src"] != null) { oVariables.pcode_pos_8 = (string)Request.QueryString["src"]; }
                if ((string)Request.QueryString["seg"] != null) { oVariables.pcode_segment = (string)Request.QueryString["seg"]; } if ((string)Request.QueryString["aff_id2"] != null) { oVariables.vendor_data2 = (string)Request.QueryString["aff_id2"]; }
                oVariables.referring_url = System.Web.HttpContext.Current.Request.Url.ToString();

                oVariables = ShippingModels.AssignShoppingDetailsToOrderVariable(oVariables, shipping);
                string choiceBooks = string.Join(",", SelectedBooks);

                if (!string.IsNullOrEmpty(choiceBooks))
                {
                    oVariables.has_shopping_Cart = true;
                    oVariables.shopping_cart_items = choiceBooks;
                }

                //Submitting shipping details to order engin for order process
                //Code commented for passing to payment page. 
                oVariables = oProcess.OrderSubmit(oVariables);
                if (oVariables != null)
                {
                    if (oVariables.order_id > 0)
                    {
                        Session.Add("NewOrderDetails", oVariables);
                        return RedirectToAction("Confirmation", "Home");
                    }
                    else
                    {
                        if (oVariables.err.Length >= 0)
                        {
                            if ((oVariables.order_status == "X") || (oVariables.order_status == "F"))
                            {
                                //   page_log += "Order is NOT processed with Order Status X or F. Error: " + oVariables.err + " | Status: " + oVariables.order_status + "<br>";
                                //  Response.Redirect("../orderstatus.aspx" + oComm.GetURIString(), false);
                                // HttpContext.Current.ApplicationInstance.CompleteRequest();
                                return RedirectToAction("orderstatus", "Home");
                            }
                            else if (oVariables.err.Length > 0)
                            {
                                //  page_log += "Order is NOT processed with an ERROR. Error: " + oVariables.err + " | Status: " + oVariables.order_status + "<br>";
                                //  lblErrorMsg.Text = oVariables.err;
                                //  oVariables.err = oVariables.err.Replace("<br>", "\\r\\n");
                                //  string error_msg = string.Empty;
                                //  error_msg = "alert('" + oVariables.err + "')";
                                //  ScriptManager.RegisterStartupScript(this, this.GetType(), "client_error", error_msg, true);
                                ViewBag.ErrorMsg = oVariables.err;
                                oVariables.err = oVariables.err.Replace("<br>", "\\r\\n");
                            }
                            else if ((oVariables.order_status == "N") || (oVariables.redirect_page.Length > 0))
                            {
                                //  page_log += "Order is NOT processed with NO ERROR. Error: " + oVariables.err + " | Status: " + oVariables.order_status + "<br>";
                                Session.Add("NewSBMDetails", oVariables);
                                //   Response.Redirect("../" + oVariables.redirect_page + oComm.GetURIString() + "&template=club", false);
                                //   HttpContext.Current.ApplicationInstance.CompleteRequest();
                                return RedirectToAction("Payment4_for_99", "Disney");
                            }
                            else
                            {
                                /* page_log += "Order is NOT processed with YES ERROR. Error: " + oVariables.err + " | Status: " + oVariables.order_status + "<br>";
                                 lblErrorMsg.Text = oVariables.err;
                                 oVariables.err = oVariables.err.Replace("<br>", "\\r\\n");
                                 string error_msg = string.Empty;
                                 error_msg = "alert('" + oVariables.err + "')";
                                 ScriptManager.RegisterStartupScript(this, this.GetType(), "client_error", error_msg, true); */

                                ViewBag.ErrorMsg = oVariables.err;
                                oVariables.err = oVariables.err.Replace("<br>", "\\r\\n");
                            }
                        }
                    }
                }
                else
                {
                    //page_log += "Order is NOT processed, Order Process returned NULL.<br>";
                    Session["NewSBMDetails"] = null;
                    return RedirectToAction("orderstatus", "Home");
                }
                //ends Submitting shipping details to order engin for order process
                //Session["NewSBMDetails"] = oVariables;
                //Session["ShippingDetails"] = oVariables;
                //return RedirectToAction("Payment4_for_1", "Seuss");
            }
            catch (Exception ex)
            {
                string page_log = "Exception raised. Exception: " + ex.Message.ToString() + "<br>";
                CommonModels oCom = new CommonModels();
                string s = "";// oCom.LogBrowserCapabilities(Request.Browser);
                oCom.SendEmail(HttpContext.Request.Url.ToString() + "<br>ex.message = " + ex.Message.ToString() + "<br> Additional Information - " + page_log + ".<br> Browser Details....<br>" + s);
                return View();
            }
            finally
            {
                oVariables = null;
                oComm = null;
                oProcess = null;
            }
            return View();
        }

        [PreserveQueryString]
        public ActionResult Payment4_for_99()
        {
            ViewData["StatesList"] = UtilitiesModels.GetStateNameList();
            ViewData["MonthList"] = UtilitiesModels.GetMonthNameList();
            ViewData["YearList"] = UtilitiesModels.GetCardExpiryYearList();

            OrderVariables oVariables = new OrderVariables();
            OrderProcess oProcess = new OrderProcess();
            CommonModels oComm = new CommonModels();

            string cart_details, conf_pg_tac, total = "";
            string shipall = "false";
            string template = "";
            string pixel = "";
            try
            {
                if (Session["NewSBMDetails"] != null)
                {
                    oVariables = (OrderVariables)Session["NewSBMDetails"];
                    Dictionary<string, string> d = oComm.GetOfferCreatives(oVariables);
                    ViewBag.HeaderImageSrc = oComm.GetDictionaryValue("payment_header", d);
                    if ((string)Request.QueryString["shipall"] != null) { shipall = (string)Request.QueryString["shipall"]; }
                    if ((string)Request.QueryString["template"] != null) { template = (string)Request.QueryString["template"]; }
                    string cartId = oVariables.cart_id.ToString();
                    conf_pg_tac = oVariables.PageVars[0].conf_pg_tac;
                    cart_details = oComm.ResponsivePayment_GiftingProducts(oVariables, Convert.ToBoolean(shipall), template);
                    total = String.Format("{0:c}", oVariables.total_amt + oVariables.tax_amt + oVariables.total_sah);
                    ViewBag.IsBonusSelected = false;
                    if (oVariables.bonus_option == true)
                    {
                        ViewBag.IsBonusSelected = true;
                    }
                    ViewBag.CartSummary = cart_details;
                    ViewBag.Cart = cartId;
                    ViewBag.Total = total;
                    ViewBag.ConfPgTAC = conf_pg_tac;
                    return View();
                }
                else
                {
                    return RedirectToAction("Four_for_99", "Disney");
                }
            }
            catch (Exception ex)
            {
                oComm.SendEmail("Exception Raised in EM Landers Payment Page - " + ex.Message.ToString());
                return View();
            }
            finally
            {
                oComm = null;
                oVariables = null;
            }
        }

        [HttpPost]
        public ActionResult Payment4_for_99(ShippingModels.BillingDetails billing)
        {
            ViewData["StatesList"] = UtilitiesModels.GetStateNameList();
            ViewData["StatesList"] = UtilitiesModels.GetStateNameList();
            ViewData["MonthList"] = UtilitiesModels.GetMonthNameList();
            ViewData["YearList"] = UtilitiesModels.GetCardExpiryYearList();

            OrderVariables oVariables = new OrderVariables();
            OrderProcess oProcess = new OrderProcess();
            CommonModels oComm = new CommonModels();

            string cart_details, conf_pg_tac, total = "";
            string shipall = "false";
            string template = "";
            string pixel = "";

            if (!string.IsNullOrEmpty(billing.SecurityCaptch) && Session["rndtext"] != null)
            {
                string strCaptch = Session["rndtext"] as string;
                if (!strCaptch.Equals(billing.SecurityCaptch))
                {
                    ViewBag.ErrorMsg = "Invalid Security Captch.";
                    // return View();
                }
            }
            else
            {
                // if (string.IsNullOrEmpty(billing.SecurityCaptch))
                //  {
                ViewBag.ErrorMsg = "Security Captch is required.";
                return View();
                //  }
            }

            if (Session["ShippingDetails"] != null)
                oVariables = Session["ShippingDetails"] as OrderVariables;

            try
            {
                if (Session["NewSBMDetails"] != null)
                {
                    oVariables = (OrderVariables)Session["NewSBMDetails"];

                    if (oVariables != null)
                    {
                        if (ViewBag.ErrorMsg != null && ViewBag.ErrorMsg != "")
                        {
                            //Setting values if page is getting back to the payment view only.
                            Dictionary<string, string> d = oComm.GetOfferCreatives(oVariables);
                            ViewBag.HeaderImageSrc = oComm.GetDictionaryValue("payment_header", d);
                            if ((string)Request.QueryString["shipall"] != null) { shipall = (string)Request.QueryString["shipall"]; }
                            if ((string)Request.QueryString["template"] != null) { template = (string)Request.QueryString["template"]; }
                            string cartId = oVariables.cart_id.ToString();
                            conf_pg_tac = oVariables.PageVars[0].conf_pg_tac;
                            cart_details = oComm.ResponsivePayment_GiftingProducts(oVariables, Convert.ToBoolean(shipall), template);
                            total = String.Format("{0:c}", oVariables.total_amt + oVariables.tax_amt + oVariables.total_sah);
                            ViewBag.IsBonusSelected = false;
                            if (oVariables.bonus_option == true)
                            {
                                ViewBag.IsBonusSelected = true;
                            }
                            ViewBag.CartSummary = cart_details;
                            ViewBag.Cart = cartId;
                            ViewBag.Total = total;
                            ViewBag.ConfPgTAC = conf_pg_tac;

                            return View();
                        }


                        oVariables = ShippingModels.AssignBillingToOrderVariable(oVariables, billing);
                        oVariables = oProcess.OrderSubmit(oVariables);
                        if (oVariables != null)
                        {
                            if (oVariables.order_id > 0)
                            {
                                Session["NewSBMDetails"] = null;
                                Session.Add("NewOrderDetails", oVariables);
                                return RedirectToAction("Confirmation", "Home");
                            }
                            else
                            {
                                if (oVariables.err.Length > 0)
                                {
                                    if ((oVariables.order_status == "X") || (oVariables.order_status == "F"))
                                    {
                                        Session["NewSBMDetails"] = null;
                                        return RedirectToAction("orderstatus", "Home");
                                    }
                                    else
                                    {
                                        Session.Add("NewSBMDetails", oVariables);
                                        ViewBag.ErrorMsg = oVariables.err;
                                        oVariables.err = oVariables.err.Replace("<br>", "\\r\\n");
                                    }
                                }
                                else if (oVariables.isSoftDeclined)
                                {
                                    Session["NewSBMDetails"] = null;
                                    return RedirectToAction("ThankYou", "home");
                                }
                                //Setting values if page is getting back to the payment view only.
                                Dictionary<string, string> d = oComm.GetOfferCreatives(oVariables);
                                ViewBag.HeaderImageSrc = oComm.GetDictionaryValue("payment_header", d);
                                if ((string)Request.QueryString["shipall"] != null) { shipall = (string)Request.QueryString["shipall"]; }
                                if ((string)Request.QueryString["template"] != null) { template = (string)Request.QueryString["template"]; }
                                string cartId = oVariables.cart_id.ToString();
                                conf_pg_tac = oVariables.PageVars[0].conf_pg_tac;
                                cart_details = oComm.ResponsivePayment_GiftingProducts(oVariables, Convert.ToBoolean(shipall), template);
                                total = String.Format("{0:c}", oVariables.total_amt + oVariables.tax_amt + oVariables.total_sah);
                                ViewBag.IsBonusSelected = false;
                                if (oVariables.bonus_option == true)
                                {
                                    ViewBag.IsBonusSelected = true;
                                }
                                ViewBag.CartSummary = cart_details;
                                ViewBag.Cart = cartId;
                                ViewBag.Total = total;
                                ViewBag.ConfPgTAC = conf_pg_tac;
                            }
                        }
                        else
                        {
                            return RedirectToAction("orderstatus", "Home");
                        }
                    }
                    return View();
                }
                else
                {
                    return RedirectToAction("Four_For_99", "Disney");
                }

            }
            catch (Exception ex)
            {
                oComm.SendEmail("Exception Raised in EM Landers Payment Page (Submit) - " + ex.Message.ToString());
                return View();
            }
            finally
            {
                oComm = null;
                oVariables = null;
                oProcess = null;
            }

        }
        #endregion //Four_for_99

        #region Four_for_99_Calendar
        [PreserveQueryString]
        public ActionResult Four_for_99_Calendar()
        {
            ViewData["StatesList"] = UtilitiesModels.GetStateNameList();
            ViewData["GenderList"] = UtilitiesModels.GetChildGenderNameList();
            return View();
        }

        [HttpPost]
        public ActionResult Four_for_99_Calendar(FormCollection form, ShippingModels.ShippingAddress shipping, string[] SelectedBooks)
        {
            ViewData["StatesList"] = UtilitiesModels.GetStateNameList();
            ViewData["GenderList"] = UtilitiesModels.GetChildGenderNameList();
            OrderProcess oProcess = new OrderProcess();
            CommonMethods oComm = new CommonMethods();
            OrderVariables oVariables = new OrderVariables();
            try
            {
                if (ModelState.IsValid == false)
                {
                    var message = string.Join("<br/>", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                    ViewBag.ErrorMsg = message.ToString();
                    return View();
                }

                oVariables = oProcess.GetOfferAndPageDetails("fosina-disney-4for99-secure-calendar");

                if ((string)Request.QueryString["vendorcode"] != null) { oVariables.vendor_id = (string)Request.QueryString["vendorcode"]; }
                if ((string)Request.QueryString["key"] != null) { oVariables.vendor_data2 = (string)Request.QueryString["key"]; }
                if ((string)Request.QueryString["vc"] != null) { oVariables.vendor_id = (string)Request.QueryString["vc"]; }
                if ((string)Request.QueryString["pc"] != null) { oVariables.promotion_code = (string)Request.QueryString["pc"]; }
                if ((string)Request.QueryString["aff_id"] != null) { oVariables.vendor_data1 = (string)Request.QueryString["aff_id"]; } if ((string)Request.QueryString["tracking"] != null) { oVariables.vendor_cust_ref_id = (string)Request.QueryString["tracking"]; }
                if ((string)Request.QueryString["src"] != null) { oVariables.pcode_pos_8 = (string)Request.QueryString["src"]; }
                if ((string)Request.QueryString["seg"] != null) { oVariables.pcode_segment = (string)Request.QueryString["seg"]; } if ((string)Request.QueryString["aff_id2"] != null) { oVariables.vendor_data2 = (string)Request.QueryString["aff_id2"]; }
                oVariables.referring_url = System.Web.HttpContext.Current.Request.Url.ToString();

                oVariables = ShippingModels.AssignShippingToOrderVariable(oVariables, shipping);

                string choiceBooks = string.Join(",", SelectedBooks);

                if (!string.IsNullOrEmpty(choiceBooks))
                {
                    oVariables.has_shopping_Cart = true;
                    oVariables.shopping_cart_items = choiceBooks;
                }

                //Submitting shipping details to order engin for order process
                //Code commented for passing to payment page. 
                oVariables = oProcess.OrderSubmit(oVariables);
                if (oVariables != null)
                {
                    if (oVariables.order_id > 0)
                    {
                        Session.Add("NewOrderDetails", oVariables);
                        return RedirectToAction("Confirmation", "Home");
                    }
                    else
                    {
                        if (oVariables.err.Length >= 0)
                        {
                            if ((oVariables.order_status == "X") || (oVariables.order_status == "F"))
                            {
                                //   page_log += "Order is NOT processed with Order Status X or F. Error: " + oVariables.err + " | Status: " + oVariables.order_status + "<br>";
                                //  Response.Redirect("../orderstatus.aspx" + oComm.GetURIString(), false);
                                // HttpContext.Current.ApplicationInstance.CompleteRequest();
                                return RedirectToAction("orderstatus", "Home");
                            }
                            else if (oVariables.err.Length > 0)
                            {
                                //  page_log += "Order is NOT processed with an ERROR. Error: " + oVariables.err + " | Status: " + oVariables.order_status + "<br>";
                                //  lblErrorMsg.Text = oVariables.err;
                                //  oVariables.err = oVariables.err.Replace("<br>", "\\r\\n");
                                //  string error_msg = string.Empty;
                                //  error_msg = "alert('" + oVariables.err + "')";
                                //  ScriptManager.RegisterStartupScript(this, this.GetType(), "client_error", error_msg, true);
                                ViewBag.ErrorMsg = oVariables.err;
                                oVariables.err = oVariables.err.Replace("<br>", "\\r\\n");
                            }
                            else if ((oVariables.order_status == "N") || (oVariables.redirect_page.Length > 0))
                            {
                                //  page_log += "Order is NOT processed with NO ERROR. Error: " + oVariables.err + " | Status: " + oVariables.order_status + "<br>";
                                Session.Add("NewSBMDetails", oVariables);
                                //   Response.Redirect("../" + oVariables.redirect_page + oComm.GetURIString() + "&template=club", false);
                                //   HttpContext.Current.ApplicationInstance.CompleteRequest();
                                return RedirectToAction("Payment4_for_99Calendar", "Disney");
                            }
                            else
                            {
                                /* page_log += "Order is NOT processed with YES ERROR. Error: " + oVariables.err + " | Status: " + oVariables.order_status + "<br>";
                                 lblErrorMsg.Text = oVariables.err;
                                 oVariables.err = oVariables.err.Replace("<br>", "\\r\\n");
                                 string error_msg = string.Empty;
                                 error_msg = "alert('" + oVariables.err + "')";
                                 ScriptManager.RegisterStartupScript(this, this.GetType(), "client_error", error_msg, true); */

                                ViewBag.ErrorMsg = oVariables.err;
                                oVariables.err = oVariables.err.Replace("<br>", "\\r\\n");
                            }
                        }
                    }
                }
                else
                {
                    //page_log += "Order is NOT processed, Order Process returned NULL.<br>";
                    Session["NewSBMDetails"] = null;
                    return RedirectToAction("orderstatus", "Home");
                }
                //ends Submitting shipping details to order engin for order process
                //Session["NewSBMDetails"] = oVariables;
                //Session["ShippingDetails"] = oVariables;
                //return RedirectToAction("Payment4_for_1", "Seuss");
            }
            catch (Exception ex)
            {
                string page_log = "Exception raised. Exception: " + ex.Message.ToString() + "<br>";
                CommonModels oCom = new CommonModels();
                string s = "";// oCom.LogBrowserCapabilities(Request.Browser);
                oCom.SendEmail(HttpContext.Request.Url.ToString() + "<br>ex.message = " + ex.Message.ToString() + "<br> Additional Information - " + page_log + ".<br> Browser Details....<br>" + s);
                return View();
            }
            finally
            {
                oVariables = null;
                oComm = null;
                oProcess = null;
            }
            return View();
        }

        [PreserveQueryString]
        public ActionResult Payment4_for_99Calendar()
        {
            ViewData["StatesList"] = UtilitiesModels.GetStateNameList();
            ViewData["MonthList"] = UtilitiesModels.GetMonthNameList();
            ViewData["YearList"] = UtilitiesModels.GetCardExpiryYearList();

            OrderVariables oVariables = new OrderVariables();
            OrderProcess oProcess = new OrderProcess();
            CommonModels oComm = new CommonModels();

            string cart_details, conf_pg_tac, total = "";
            string shipall = "false";
            string template = "";
            string pixel = "";
            try
            {
                if (Session["NewSBMDetails"] != null)
                {
                    oVariables = (OrderVariables)Session["NewSBMDetails"];
                    Dictionary<string, string> d = oComm.GetOfferCreatives(oVariables);
                    ViewBag.HeaderImageSrc = oComm.GetDictionaryValue("payment_header", d);
                    if ((string)Request.QueryString["shipall"] != null) { shipall = (string)Request.QueryString["shipall"]; }
                    if ((string)Request.QueryString["template"] != null) { template = (string)Request.QueryString["template"]; }
                    string cartId = oVariables.cart_id.ToString();

                    if (oVariables != null)
                    {
                        conf_pg_tac = oVariables.PageVars[0].conf_pg_tac;
                        cart_details = oComm.ResponsivePayment_GiftingProducts(oVariables, Convert.ToBoolean(shipall), template);
                        total = String.Format("{0:c}", oVariables.total_amt + oVariables.tax_amt + oVariables.total_sah);
                        ViewBag.IsBonusSelected = false;
                        if (oVariables.bonus_option == true)
                        {
                            ViewBag.IsBonusSelected = true;
                        }
                        ViewBag.CartSummary = cart_details;
                        ViewBag.Cart = cartId;
                        ViewBag.Total = total;
                        ViewBag.ConfPgTAC = conf_pg_tac;
                        ViewBag.ChoiceBook = oVariables.shopping_cart_items;
                    }
                    return View();
                }
                else
                {
                    return RedirectToAction("Four_for_99_Calendar", "Disney");
                }

            }
            catch (Exception ex)
            {
                oComm.SendEmail("Exception Raised in EM Landers Payment Page - " + ex.Message.ToString());
                return View();
            }
            finally
            {
                oComm = null;
                oVariables = null;
            }
        }

        [HttpPost]
        public ActionResult Payment4_for_99Calendar(ShippingModels.BillingDetails billing)
        {
            ViewData["StatesList"] = UtilitiesModels.GetStateNameList();
            ViewData["StatesList"] = UtilitiesModels.GetStateNameList();
            ViewData["MonthList"] = UtilitiesModels.GetMonthNameList();
            ViewData["YearList"] = UtilitiesModels.GetCardExpiryYearList();

            OrderVariables oVariables = new OrderVariables();
            OrderProcess oProcess = new OrderProcess();
            CommonModels oComm = new CommonModels();

            string cart_details, conf_pg_tac, total = "";
            string shipall = "false";
            string template = "";
            string pixel = "";

            if (!string.IsNullOrEmpty(billing.SecurityCaptch) && Session["rndtext"] != null)
            {
                string strCaptch = Session["rndtext"] as string;
                if (!strCaptch.Equals(billing.SecurityCaptch))
                {
                    ViewBag.ErrorMsg = "Invalid Security Captch.";
                    // return View();
                }
            }
            else
            {
                //  if (string.IsNullOrEmpty(billing.SecurityCaptch))
                //  {
                ViewBag.ErrorMsg = "Security Captch is required.";
                //return View();
                //  }
            }

            if (Session["ShippingDetails"] != null)
                oVariables = Session["ShippingDetails"] as OrderVariables;

            try
            {
                if (Session["NewSBMDetails"] != null)
                {
                    oVariables = (OrderVariables)Session["NewSBMDetails"];

                    if (oVariables != null)
                    {
                        if (ViewBag.ErrorMsg != null && ViewBag.ErrorMsg != "")
                        {
                            //Setting values if page is getting back to the payment view only.
                            Dictionary<string, string> d = oComm.GetOfferCreatives(oVariables);
                            ViewBag.HeaderImageSrc = oComm.GetDictionaryValue("payment_header", d);
                            if ((string)Request.QueryString["shipall"] != null) { shipall = (string)Request.QueryString["shipall"]; }
                            if ((string)Request.QueryString["template"] != null) { template = (string)Request.QueryString["template"]; }
                            string cartId = oVariables.cart_id.ToString();
                            conf_pg_tac = oVariables.PageVars[0].conf_pg_tac;
                            cart_details = oComm.ResponsivePayment_GiftingProducts(oVariables, Convert.ToBoolean(shipall), template);
                            total = String.Format("{0:c}", oVariables.total_amt + oVariables.tax_amt + oVariables.total_sah);
                            ViewBag.IsBonusSelected = false;
                            if (oVariables.bonus_option == true)
                            {
                                ViewBag.IsBonusSelected = true;
                            }
                            ViewBag.CartSummary = cart_details;
                            ViewBag.Cart = cartId;
                            ViewBag.Total = total;
                            ViewBag.ConfPgTAC = conf_pg_tac;
                            ViewBag.ChoiceBook = oVariables.shopping_cart_items;
                            return View();
                        }

                        oVariables = ShippingModels.AssignBillingToOrderVariable(oVariables, billing);
                        oVariables = oProcess.OrderSubmit(oVariables);
                        if (oVariables != null)
                        {
                            if (oVariables.order_id > 0)
                            {
                                Session["NewSBMDetails"] = null;
                                Session.Add("NewOrderDetails", oVariables);
                                return RedirectToAction("Confirmation", "Home");
                            }
                            else
                            {
                                if (oVariables.err.Length > 0)
                                {
                                    if ((oVariables.order_status == "X") || (oVariables.order_status == "F"))
                                    {
                                        Session["NewSBMDetails"] = null;
                                        return RedirectToAction("orderstatus", "Home");
                                    }
                                    else
                                    {
                                        Session.Add("NewSBMDetails", oVariables);
                                        ViewBag.ErrorMsg = oVariables.err;
                                        oVariables.err = oVariables.err.Replace("<br>", "\\r\\n");
                                    }
                                }
                                else if (oVariables.isSoftDeclined)
                                {
                                    Session["NewSBMDetails"] = null;
                                    return RedirectToAction("ThankYou", "home");
                                }
                                //Setting values if page is getting back to the payment view only.
                                Dictionary<string, string> d = oComm.GetOfferCreatives(oVariables);
                                ViewBag.HeaderImageSrc = oComm.GetDictionaryValue("payment_header", d);
                                if ((string)Request.QueryString["shipall"] != null) { shipall = (string)Request.QueryString["shipall"]; }
                                if ((string)Request.QueryString["template"] != null) { template = (string)Request.QueryString["template"]; }
                                string cartId = oVariables.cart_id.ToString();
                                conf_pg_tac = oVariables.PageVars[0].conf_pg_tac;
                                cart_details = oComm.ResponsivePayment_GiftingProducts(oVariables, Convert.ToBoolean(shipall), template);
                                total = String.Format("{0:c}", oVariables.total_amt + oVariables.tax_amt + oVariables.total_sah);
                                ViewBag.IsBonusSelected = false;
                                if (oVariables.bonus_option == true)
                                {
                                    ViewBag.IsBonusSelected = true;
                                }
                                ViewBag.CartSummary = cart_details;
                                ViewBag.Cart = cartId;
                                ViewBag.Total = total;
                                ViewBag.ConfPgTAC = conf_pg_tac;
                                ViewBag.ChoiceBook = oVariables.shopping_cart_items;
                            }
                        }
                        else
                        {
                            return RedirectToAction("orderstatus", "Home");
                        }
                    }
                    return View();
                }
                else
                {
                    return RedirectToAction("Four_for_99_Calendar", "Disney");
                }

            }
            catch (Exception ex)
            {
                oComm.SendEmail("Exception Raised in EM Landers Payment Page (Submit) - " + ex.Message.ToString());
                return View();
            }
            finally
            {
                oComm = null;
                oVariables = null;
                oProcess = null;
            }
        }

        #endregion //Four_for_99_Calendar

        #region Four_for_99_Bonus
        [PreserveQueryString]
        public ActionResult four_for_99_bonus()
        {
            ViewData["StatesList"] = UtilitiesModels.GetStateNameList();
            ViewData["GenderList"] = UtilitiesModels.GetChildGenderNameList();
            OfferService offerService = new OfferService();
            return View(offerService.GetDefaultCustomerInfo());
        }

        [HttpPost]
        [PreserveQueryString]
        public ActionResult four_for_99_bonus(FormCollection form, ShippingModels.ShippingAddress shipping, string[] SelectedBooks)
        {
            ViewData["StatesList"] = UtilitiesModels.GetStateNameList();
            ViewData["GenderList"] = UtilitiesModels.GetChildGenderNameList();
            OrderProcess oProcess = new OrderProcess();
            CommonMethods oComm = new CommonMethods();
            OrderVariables oVariables = new OrderVariables();
            try
            {
                if (ModelState.IsValid == false)
                {
                    var message = string.Join("<br/>", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                    ViewBag.ErrorMsg = message.ToString();
                    return View();
                }

                oVariables = oProcess.GetOfferAndPageDetails("fosina-disney-4for99-secure");

                #region "Commented by Manish @ 07.11.2016"
                //if ((string)Request.QueryString["vendorcode"] != null) { oVariables.vendor_id = (string)Request.QueryString["vendorcode"]; }
                //if ((string)Request.QueryString["key"] != null) { oVariables.vendor_data2 = (string)Request.QueryString["key"]; }
                //if ((string)Request.QueryString["vc"] != null) { oVariables.vendor_id = (string)Request.QueryString["vc"]; }
                //if ((string)Request.QueryString["pc"] != null) { oVariables.promotion_code = (string)Request.QueryString["pc"]; }
                //if ((string)Request.QueryString["aff_id"] != null) { oVariables.vendor_data1 = (string)Request.QueryString["aff_id"]; } if ((string)Request.QueryString["tracking"] != null) { oVariables.vendor_cust_ref_id = (string)Request.QueryString["tracking"]; }
                //if ((string)Request.QueryString["src"] != null) { oVariables.pcode_pos_8 = (string)Request.QueryString["src"]; }
                //if ((string)Request.QueryString["seg"] != null) { oVariables.pcode_segment = (string)Request.QueryString["seg"]; } if ((string)Request.QueryString["aff_id2"] != null) { oVariables.vendor_data2 = (string)Request.QueryString["aff_id2"]; }
                //oVariables.referring_url = System.Web.HttpContext.Current.Request.Url.ToString();
                #endregion

                var offerService = new OfferService();
                oVariables = offerService.MapQueryStringToOrderVariables(oVariables: oVariables);
                oVariables = ShippingModels.AssignShippingToOrderVariable(oVariables, shipping);

                string choiceBooks = string.Join(",", SelectedBooks);

                if (!string.IsNullOrEmpty(choiceBooks))
                {
                    oVariables.has_shopping_Cart = true;
                    oVariables.shopping_cart_items = choiceBooks;
                }

                //Submitting shipping details to order engin for order process
                //Code commented for passing to payment page. 
                oVariables = oProcess.OrderSubmit(oVariables);
                if (oVariables != null)
                {
                    if (oVariables.order_id > 0)
                    {
                        Session.Add("NewOrderDetails", oVariables);
                        return RedirectToAction("Confirmation", "Home");
                    }
                    else
                    {
                        if (oVariables.err.Length >= 0)
                        {
                            if ((oVariables.order_status == "X") || (oVariables.order_status == "F"))
                            {
                                Session.Add("NewOrderDetails", oVariables);
                                return RedirectToAction("orderstatus", "Home");
                            }
                            else if (oVariables.err.Length > 0)
                            {
                                //  page_log += "Order is NOT processed with an ERROR. Error: " + oVariables.err + " | Status: " + oVariables.order_status + "<br>";
                                //  lblErrorMsg.Text = oVariables.err;
                                //  oVariables.err = oVariables.err.Replace("<br>", "\\r\\n");
                                //  string error_msg = string.Empty;
                                //  error_msg = "alert('" + oVariables.err + "')";
                                //  ScriptManager.RegisterStartupScript(this, this.GetType(), "client_error", error_msg, true);
                                ViewBag.ErrorMsg = oVariables.err;
                                oVariables.err = oVariables.err.Replace("<br>", "\\r\\n");
                            }
                            else if ((oVariables.order_status == "N") || (oVariables.redirect_page.Length > 0))
                            {
                                //  page_log += "Order is NOT processed with NO ERROR. Error: " + oVariables.err + " | Status: " + oVariables.order_status + "<br>";
                                Session.Add("NewSBMDetails", oVariables);
                                //   Response.Redirect("../" + oVariables.redirect_page + oComm.GetURIString() + "&template=club", false);
                                //   HttpContext.Current.ApplicationInstance.CompleteRequest();
                                return RedirectToAction("payment4_for_99_bonus", "Disney");
                            }
                            else
                            {
                                /* page_log += "Order is NOT processed with YES ERROR. Error: " + oVariables.err + " | Status: " + oVariables.order_status + "<br>";
                                 lblErrorMsg.Text = oVariables.err;
                                 oVariables.err = oVariables.err.Replace("<br>", "\\r\\n");
                                 string error_msg = string.Empty;
                                 error_msg = "alert('" + oVariables.err + "')";
                                 ScriptManager.RegisterStartupScript(this, this.GetType(), "client_error", error_msg, true); */

                                ViewBag.ErrorMsg = oVariables.err;
                                oVariables.err = oVariables.err.Replace("<br>", "\\r\\n");
                            }
                        }
                    }
                }
                else
                {
                    //page_log += "Order is NOT processed, Order Process returned NULL.<br>";
                    Session["NewSBMDetails"] = null;
                    return RedirectToAction("orderstatus", "Home");
                }
                //ends Submitting shipping details to order engin for order process
                //Session["NewSBMDetails"] = oVariables;
                //Session["ShippingDetails"] = oVariables;
                //return RedirectToAction("Payment4_for_1", "Seuss");
            }
            catch (Exception ex)
            {
                string page_log = "Exception raised. Exception: " + ex.Message.ToString() + "<br>";
                CommonModels oCom = new CommonModels();
                string s = "";// oCom.LogBrowserCapabilities(Request.Browser);
                oCom.SendEmail(HttpContext.Request.Url.ToString() + "<br>ex.message = " + ex.Message.ToString() + "<br> Additional Information - " + page_log + ".<br> Browser Details....<br>" + s);
                return View();
            }
            finally
            {
                oVariables = null;
                oComm = null;
                oProcess = null;
            }
            return View();
        }

        [PreserveQueryString]
        public ActionResult payment4_for_99_bonus()
        {
            ViewData["StatesList"] = UtilitiesModels.GetStateNameList();
            ViewData["MonthList"] = UtilitiesModels.GetMonthNameList();
            ViewData["YearList"] = UtilitiesModels.GetCardExpiryYearList();

            OrderVariables oVariables = new OrderVariables();
            OrderProcess oProcess = new OrderProcess();
            CommonModels oComm = new CommonModels();

            string cart_details, conf_pg_tac, total = "";
            string shipall = "false";
            string template = "";
            string pixel = "";
            try
            {
                if (Session["NewSBMDetails"] != null)
                {
                    oVariables = (OrderVariables)Session["NewSBMDetails"];
                    Dictionary<string, string> d = oComm.GetOfferCreatives(oVariables);
                    ViewBag.HeaderImageSrc = oComm.GetDictionaryValue("payment_header", d);
                    if ((string)Request.QueryString["shipall"] != null) { shipall = (string)Request.QueryString["shipall"]; }
                    if ((string)Request.QueryString["template"] != null) { template = (string)Request.QueryString["template"]; }
                    string cartId = oVariables.cart_id.ToString();

                    if (oVariables != null)
                    {
                        conf_pg_tac = oVariables.PageVars[0].conf_pg_tac;
                        cart_details = oComm.ResponsivePayment_GiftingProducts(oVariables, Convert.ToBoolean(shipall), template);
                        total = String.Format("{0:c}", oVariables.total_amt + oVariables.tax_amt + oVariables.total_sah);
                        ViewBag.IsBonusSelected = false;
                        if (oVariables.bonus_option == true)
                        {
                            ViewBag.IsBonusSelected = true;
                        }
                        ViewBag.CartSummary = cart_details;
                        ViewBag.Cart = cartId;
                        ViewBag.Total = total;
                        ViewBag.ConfPgTAC = conf_pg_tac;
                        ViewBag.ChoiceBook = oVariables.shopping_cart_items;
                    }
                    return View();
                }
                else
                {
                    return RedirectToAction("four_for_99_bonus", "Disney");
                }

            }
            catch (Exception ex)
            {
                oComm.SendEmail("Exception Raised in EM Landers Payment Page - " + ex.Message.ToString());
                return View();
            }
            finally
            {
                oComm = null;
                oVariables = null;
            }
        }

        [HttpPost]
        [PreserveQueryString]
        public ActionResult payment4_for_99_bonus(ShippingModels.BillingDetails billing)
        {
           
            ViewData["StatesList"] = UtilitiesModels.GetStateNameList();
            ViewData["MonthList"] = UtilitiesModels.GetMonthNameList();
            ViewData["YearList"] = UtilitiesModels.GetCardExpiryYearList();

            OrderVariables oVariables = new OrderVariables();
            OrderProcess oProcess = new OrderProcess();
            CommonModels oComm = new CommonModels();

            string cart_details, conf_pg_tac, total = "";
            string shipall = "false";
            string template = "";
            string pixel = "";

            if (!string.IsNullOrEmpty(billing.SecurityCaptch) && Session["rndtext"] != null)
            {
                string strCaptch = Session["rndtext"] as string;
                if (!strCaptch.Equals(billing.SecurityCaptch))
                {
                    ViewBag.ErrorMsg = "Invalid Security Captcha.";
                    // return View();
                }
            }
            else
            {
                //  if (string.IsNullOrEmpty(billing.SecurityCaptch))
                //  {
                ViewBag.ErrorMsg = "Security Captcha is required.";
                //return View();
                //  }
            }

            if (Session["ShippingDetails"] != null)
                oVariables = Session["ShippingDetails"] as OrderVariables;

            try
            {
                if (Session["NewSBMDetails"] != null)
                {
                    oVariables = (OrderVariables)Session["NewSBMDetails"];

                    if (oVariables != null)
                    {
                        if (ViewBag.ErrorMsg != null && ViewBag.ErrorMsg != "")
                        {
                            //Setting values if page is getting back to the payment view only.
                            Dictionary<string, string> d = oComm.GetOfferCreatives(oVariables);
                            ViewBag.HeaderImageSrc = oComm.GetDictionaryValue("payment_header", d);
                            if ((string)Request.QueryString["shipall"] != null) { shipall = (string)Request.QueryString["shipall"]; }
                            if ((string)Request.QueryString["template"] != null) { template = (string)Request.QueryString["template"]; }
                            string cartId = oVariables.cart_id.ToString();
                            conf_pg_tac = oVariables.PageVars[0].conf_pg_tac;
                            cart_details = oComm.ResponsivePayment_GiftingProducts(oVariables, Convert.ToBoolean(shipall), template);
                            total = String.Format("{0:c}", oVariables.total_amt + oVariables.tax_amt + oVariables.total_sah);
                            ViewBag.IsBonusSelected = false;
                            if (oVariables.bonus_option == true)
                            {
                                ViewBag.IsBonusSelected = true;
                            }
                            ViewBag.CartSummary = cart_details;
                            ViewBag.Cart = cartId;
                            ViewBag.Total = total;
                            ViewBag.ConfPgTAC = conf_pg_tac;
                            ViewBag.ChoiceBook = oVariables.shopping_cart_items;
                            return View();
                        }

                        oVariables = ShippingModels.AssignBillingToOrderVariable(oVariables, billing);
                        oVariables = oProcess.OrderSubmit(oVariables);
                        if (oVariables != null)
                        {
                            if (oVariables.order_id > 0)
                            {
                                Session["NewSBMDetails"] = null;
                                Session.Add("NewOrderDetails", oVariables);
                                return RedirectToAction("Confirmation", "Home");
                            }
                            else
                            {
                                if (oVariables.err.Length > 0)
                                {
                                    if ((oVariables.order_status == "X") || (oVariables.order_status == "F"))
                                    {
                                        Session["NewSBMDetails"] = null;
                                        Session.Add("NewOrderDetails", oVariables);
                                        return RedirectToAction("orderstatus", "Home");
                                    }
                                    else
                                    {
                                        Session.Add("NewSBMDetails", oVariables);
                                        ViewBag.ErrorMsg = oVariables.err;
                                        oVariables.err = oVariables.err.Replace("<br>", "\\r\\n");
                                    }
                                }
                                else if (oVariables.isSoftDeclined)
                                {
                                    Session["NewSBMDetails"] = null;
                                    return RedirectToAction("ThankYou", "home");
                                }
                                //Setting values if page is getting back to the payment view only.
                                Dictionary<string, string> d = oComm.GetOfferCreatives(oVariables);
                                ViewBag.HeaderImageSrc = oComm.GetDictionaryValue("payment_header", d);
                                if ((string)Request.QueryString["shipall"] != null) { shipall = (string)Request.QueryString["shipall"]; }
                                if ((string)Request.QueryString["template"] != null) { template = (string)Request.QueryString["template"]; }
                                string cartId = oVariables.cart_id.ToString();
                                conf_pg_tac = oVariables.PageVars[0].conf_pg_tac;
                                cart_details = oComm.ResponsivePayment_GiftingProducts(oVariables, Convert.ToBoolean(shipall), template);
                                total = String.Format("{0:c}", oVariables.total_amt + oVariables.tax_amt + oVariables.total_sah);
                                ViewBag.IsBonusSelected = false;
                                if (oVariables.bonus_option == true)
                                {
                                    ViewBag.IsBonusSelected = true;
                                }
                                ViewBag.CartSummary = cart_details;
                                ViewBag.Cart = cartId;
                                ViewBag.Total = total;
                                ViewBag.ConfPgTAC = conf_pg_tac;
                                ViewBag.ChoiceBook = oVariables.shopping_cart_items;
                            }
                        }
                        else
                        {
                            return RedirectToAction("orderstatus", "Home");
                        }
                    }
                    return View();
                }
                else
                {
                    return RedirectToAction("four_for_99_bonus", "Disney");
                }

            }
            catch (Exception ex)
            {
                oComm.SendEmail("Exception Raised in EM Landers Payment Page (Submit) - " + ex.Message.ToString());
                return View();
            }
            finally
            {
                oComm = null;
                oVariables = null;
                oProcess = null;
            }
        }

        #endregion //Four_for_99_Bonus
    }
}