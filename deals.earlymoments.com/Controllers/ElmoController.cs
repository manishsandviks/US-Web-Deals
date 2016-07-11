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
    public class ElmoController : Controller
    {
        #region Three_99

        [PreserveQueryString]
        public ActionResult Three_99()
        {
            ViewData["StatesList"] = UtilitiesModels.GetStateNameList();
            ViewData["GenderList"] = UtilitiesModels.GetChildGenderNameList();

            #region "Commented by Samrat_Globsyn @ 07.11.2016"
            //if ((string)Request.QueryString["vendorcode"] != null) { string aa = (string)Request.QueryString["vendorcode"]; } 
            #endregion

            OfferService offerService = new OfferService();
            return View(offerService.GetDefaultCustomerInfo());
        }


        [HttpPost]
        [PreserveQueryString]
        public ActionResult Three_99(FormCollection form, ShippingModels.ShippingAddress shipping)
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
                    var message = string.Join("<br>", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                    ViewBag.ErrorMsg = message.ToString();
                    return View();
                }

                //var value = HttpContext.Request.Params.Get("vendorcode");
                //oVariables = oProcess.GetOfferAndPageDetails("seuss-winter-595-responsive");
                oVariables = oProcess.GetOfferAndPageDetails("fosina-elmo-399-secure");

                #region "Commented by Samrat_Globsyn @ 07.11.2016"
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
                            // page_log += "Order is NOT processed. Error: " + oVariables.err + " | Status: " + oVariables.order_status + "<br>";
                            if ((oVariables.order_status == "X") || (oVariables.order_status == "F"))
                            {
                                //   page_log += "Order is NOT processed with Order Status X or F. Error: " + oVariables.err + " | Status: " + oVariables.order_status + "<br>";
                                //  Response.Redirect("../orderstatus.aspx" + oComm.GetURIString(), false);
                                // HttpContext.Current.ApplicationInstance.CompleteRequest();
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
                                //oVariables.err = oVariables.err.Replace("<br>", "\\r\\n");
                                ViewBag.ErrorMsg = oVariables.err;
                                oVariables.err = oVariables.err.Replace("<br>", "\\r\\n");
                            }
                            else if ((oVariables.order_status == "N") || (oVariables.redirect_page.Length > 0))
                            {
                                //  page_log += "Order is NOT processed with NO ERROR. Error: " + oVariables.err + " | Status: " + oVariables.order_status + "<br>";
                                Session.Add("NewSBMDetails", oVariables);
                                //   Response.Redirect("../" + oVariables.redirect_page + oComm.GetURIString() + "&template=club", false);
                                //   HttpContext.Current.ApplicationInstance.CompleteRequest();
                                return RedirectToAction("Payment_399", "Elmo");
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
        public ActionResult Payment_399()
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
                    return RedirectToAction("Three_99", "Elmo", routeValues: ViewContextExtensions.OptionalParamters(Request.QueryString));
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
        public ActionResult Payment_399(FormCollection form, ShippingModels.BillingDetails billing)
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
                    //return View();
                }
            }
            else
            {
                //if (string.IsNullOrEmpty(billing.SecurityCaptch))
                //{
                ViewBag.ErrorMsg = "Security Captcha is required.";
                // return View();
                //}
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
                    return RedirectToAction("Three_99", "Elmo", routeValues: ViewContextExtensions.OptionalParamters(Request.QueryString));
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

        #endregion //Four_For_1
    }
}