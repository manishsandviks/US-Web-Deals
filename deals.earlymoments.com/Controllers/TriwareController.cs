using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using deals.earlymoments.com.Models;
using deals.earlymoments.com.Utilities;
using OrderEngine;
namespace deals.earlymoments.com.Controllers
{
    public class TriwareController : Controller
    {
        //
        // GET: /Triwire/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult home()
        {
            return View();
        }
        public ActionResult shipping()
        {
            // ViewBag.Email = !string.IsNullOrEmpty(email) ? email : "";
            ViewData["StatesList"] = UtilitiesModels.GetStateNameList();
            ViewData["GenderList"] = UtilitiesModels.GetChildGenderNameList();
            ViewData["MonthList"] = UtilitiesModels.GetMonthNameList();
            ViewData["YearList"] = UtilitiesModels.GetCardExpiryYearList();
            return View();
        }

        [HttpPost]
        public ActionResult shipping(FormCollection form, ShippingModels.ShoppingOrder shipping)
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
                //string choiceBooks = string.Join(",", SelectedBooks);

                //if (!string.IsNullOrEmpty(choiceBooks))
                //{
                //    oVariables.has_shopping_Cart = true;
                //    oVariables.shopping_cart_items = choiceBooks;
                //}

                oVariables.has_shopping_Cart = true;

                //Submitting shipping details to order engin for order process
                //Code commented for passing to payment page. 
                oVariables = oProcess.OrderSubmit(oVariables);
                if (oVariables != null)
                {
                    if (oVariables.order_id > 0)
                    {
                        Session.Add("NewOrderDetails", oVariables);
                        return RedirectToAction("upsell_offer1", "Triware");
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
                                return RedirectToAction("upsell_offer1", "Triware");
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
                CommonModels oCom = new CommonModels();
                string s = "";
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

        public ActionResult upsell_offer1()
        {
            return View();
        }
        public ActionResult upsell_offer2()
        {
            return View();
        }
        public ActionResult thankyou()
        {
            return View();
        }
    }
}