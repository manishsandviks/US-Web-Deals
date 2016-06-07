using System;
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
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [PreserveQueryString]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [PreserveQueryString]
        public ActionResult Confirmation()
        {
            OrderVariables oVariables = new OrderVariables();
            OrderProcess oProcess = new OrderProcess();
            CommonModels oComm = new CommonModels();
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
                        shipping_address = oComm.Confirmation_ShippingAddress(oVariables);
                        ViewBag.ShippingAddress = shipping_address;
                        billing_address = oComm.Confirmation_BillingAddress(oVariables, template);
                        ViewBag.BillingAddress = billing_address;

                        orderSummary = oComm.ResponsiveConfirmation_GiftingProducts(oVariables);

                        ViewBag.Cart = orderSummary;
                        string email = oVariables.email.ToString();
                        ViewBag.email = email;
                        if (!string.IsNullOrEmpty(email))
                        {
                            //ProcessConfirmationEmail(oVariables, shipping_address, billing_address, shipall);
                            NewConfirmationEmailtoExpertSender(oVariables, shipping_address, billing_address, shipall);

                            BuildDynamicPixel(oVariables);

                            //internalPixel = "<img src='https://ping.earlymoments.com/conversion.ashx?o=" + oVariables.order_id + "&e=" + oVariables.email + "' width='1' height='1' border='0' /><iframe src='https://services.earlymoments.com/ping/p.ashx?source=7630&brand=e&data=" + oVariables.order_id.ToString() + "&type=ifr' height='1' width='1' frameborder='0'></iframe><br /><img src='https://services.earlymoments.com/ping/p.ashx?source=7630&brand=e&data=" + oVariables.order_id.ToString() + "&type=img' width='1' height='1' border='0' />";
                            //internalPixel = "<iframe src='https://services.earlymoments.com/ping/p.ashx?source=7630&brand=e&data=" + oVariables.order_id.ToString() + "&type=ifr' height='1' width='1' frameborder='0'></iframe><br /><img src='https://services.earlymoments.com/ping/p.ashx?source=7630&brand=e&data=" + oVariables.order_id.ToString() + "&type=img' width='1' height='1' border='0' />";
                            //Session["IsPostBack"] = true;

                            FirePostBackPixel(oVariables);
                        }
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                oComm.SendEmail("Page_Load() <br />Exception Raised from Confirmaiton Page in EM Landers. Exception: " + ex.Message.ToString() + ".<br />More Data (Offer URL): "
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

        [PreserveQueryString]
        public ActionResult OrderStatus()
        {
            OrderVariables oVariables = new OrderVariables();
            CommonModels oComm = new CommonModels();
            try
            {
                int errorCode = 0;
                if (Session["NewOrderDetails"] != null)
                {
                    oVariables = new OrderVariables();
                    oVariables = Session["NewOrderDetails"] as OrderVariables;
                   
                    if (oVariables != null)
                    {
                        errorCode = oVariables.error_code;
                    }
                    ViewBag.ErrorMessage = GetError(errorCode.ToString());
                }
                else
                {
                    ViewBag.ErrorMessage = GetError(errorCode.ToString());
                }
            }
            catch (Exception ex)
            {
                if (oVariables != null)
                {
                    oComm.SendEmail("Page_Load() <br />Exception Raised from Confirmaiton Page in EM Landers. Exception: " + ex.Message.ToString() + ".<br />More Data (Offer URL): "
                      + oVariables.referring_url + ". <br />More Data (Order Id):" + oVariables.order_id);
                }
            }
            finally
            {
                Session["NewOrderDetails"] = null;
                oVariables = null;
                oComm = null;

            }
            return View();
        }

        public ActionResult ThankYou()
        {
            return View();
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

        private void NewConfirmationEmailtoExpertSender(OrderVariables oVariables, string ship_address, string bill_address, string shipall)
        {
            StreamReader emailReader;
            var oComm = new CommonModels();
            try
            {
                if (oVariables.order_id <= 0) return;

                var filePath = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "assets\\emails\\newOC.txt";

                emailReader = new StreamReader(filePath);
                var emailHtml = emailReader.ReadToEnd();

                if (emailHtml.Length <= 0) return;

                var tempBillFname = oVariables.bill_to_fname.Length > 0
                    ? oVariables.bill_to_fname.ToUpper().Substring(0, 1) +
                      oVariables.bill_to_fname.ToLower().Substring(1)
                    : "";
                var tempBillLname = oVariables.bill_to_lname.Length > 0
                    ? oVariables.bill_to_lname.ToUpper().Substring(0, 1) +
                      oVariables.bill_to_lname.ToLower().Substring(1)
                    : "";

                string optoutText = @"<p style=""text-align:left; font-family:'Open Sans', Helvetica, tahoma; font-size:14px; line-height: 1.3; margin-left:35px; margin-right:35px; margin-top:0px;  color:#1a5eab;""><span style=""font-family:'Open Sans', Helvetica, tahoma; !important;"">If you no longer want to receive this order, please click on the following link within 24 hours: <a href='https://www.earlymoments.com/optout' target = '_blank' > Cancel Order </a><br /><br /> If you are unable to click on the above link, please copy and paste the below link in the browser: https://www.earlymoments.com/optout</span></p>";

                var projectSpecificText = string.Empty;
                if (oVariables.project.In("DBU"))
                {
                    projectSpecificText = @"<p style=""text-align:left; font-family:'Open Sans', Helvetica, tahoma; font-size:14px; line-height: 1.3; margin-left:35px; margin-right:35px; margin-top:0px;  color:#1a5eab; background-color:yellow;""><span style=""font-family:'Open Sans', Helvetica, tahoma; !important;""><span style=""font-size:18px; font-weight:bold;"">Your companion ebooks are waiting for you!</span><br />Your Disney ebooks are available now through the official Early Moments <a href='https://services.earlymoments.com/ping/redirect.ashx?newUrl=https://itunes.apple.com/us/app/early-moments/id661335739?mt=8&source=7635&brand=e&data=!_OrderNumber_!&type=red' target='_blank'>iPad app</a>. Download the FREE Early Moments app from <a href='https://services.earlymoments.com/ping/redirect.ashx?newUrl=https://itunes.apple.com/us/app/early-moments/id661335739?mt=8&source=7635&brand=e&data=!_OrderNumber_!&type=red' target='_blank'>iTunes</a>, and follow the instructions to login for the first time.<br /><br /><a href='https://services.earlymoments.com/ping/redirect.ashx?newUrl=https://itunes.apple.com/us/app/early-moments/id661335739?mt=8&source=7635&brand=e&data=!_OrderNumber_!&type=red' target='_blank'><img src='https://enrollments.earlymoments.com/assets/images/available_on_the_app_store.png' alt='Earlymoments.com' width='130' height='35' border='0' /></a><br /><br />If you are not using an iPad, <a href='https://services.earlymoments.com/ping/redirect.ashx?newUrl=https://ebooks.earlymoments.com/default.aspx?src=cemail&source=7636&brand=e&data=!_OrderNumber_!&type=red' target='_blank'>click here</a> and follow the instructions to use our eReader.<br /></span></p>";
                    projectSpecificText = projectSpecificText.Replace("!_OrderNumber_!", Convert.ToString(oVariables.order_id));
                }

                emailHtml = emailHtml.Replace("!_BillName_!", tempBillFname);
                emailHtml = emailHtml.Replace("!_OrderNumber_!", Convert.ToString(oVariables.order_id));

                var tmpStr = @"<table width=""90%"" border=""0"" cellspacing=""0"" cellpadding=""8""><tr bgcolor=""#9fbcd8""><td colspan=""2"" valign=""middle""><p style=""text-align:center; font-family:'Open Sans', Helvetica, tahoma; font-size:16px; line-height: 1.3; margin-left:0px;margin-right:0px; margin-top:0px; margin-bottom:0px;  color:#143884; font-weight:bold;""><span style=""font-family:'Open Sans', Helvetica, tahoma; !important;"">ORDER SUMMARY</span></p></td></tr><tr><td width=""50%"" valign=""middle"" bgcolor=""#dbe6f0""><p style=""text-align:left; font-family:'Open Sans', Helvetica, tahoma; font-size:14px; line-height: 1.3; margin-left:25px; margin-right:0px; margin-top:0px; margin-bottom:0px;  color:#1a5eab; font-weight:bold;"">Item</p></td><td align=""center"" valign=""middle"" bgcolor=""#dbe6f0""><p style=""text-align:center; font-family:'Open Sans', Helvetica, tahoma; font-size:14px; line-height: 1.3; margin-left:5px; margin-right:5px; margin-top:0px; margin-bottom:0px;  color:#1a5eab;font-weight:bold;""><span style=""font-family:'Open Sans', Helvetica, tahoma; !important;"">Price</span></p></td></tr>";

                var prod = oVariables.proj_desc;
                if (Convert.ToBoolean(shipall))
                {
                    prod += "<br>(<span style='font-size:9px;'>Will be shipped all at once</span>)";
                }

                foreach (var oShipVars in oVariables.ShipVars)
                {
                    if (oShipVars.selected)
                    {
                        if ((oComm.OfferItemsDisplayType(oShipVars.OfferVars) > 1) ||
                            (oShipVars.OfferVars.Count == 1))
                        {
                            foreach (var oOffers in oShipVars.OfferVars)
                            {
                                tmpStr += @"<tr><td width=""50%"" valign=""middle"" bgcolor=""#dbe6f0""><p style=""text-align:left; font-family:'Open Sans', Helvetica, tahoma; font-size:14px; line-height: 1.3; margin-left:25px; margin-right:0px; margin-top:0px; margin-bottom:0px;  color:#1a5eab;""><span style=""font-family:'Open Sans', Helvetica, tahoma; !important;"">"
                                          + oOffers.item_desc
                                          + @"</span></p></td><td align=""center"" valign=""middle"" bgcolor=""#dbe6f0""><p style=""text-align:center; font-family:'Open Sans', Helvetica, tahoma; font-size:14px; line-height: 1.3; margin-left:5px; margin-right:5px; margin-top:0px; margin-bottom:0px; color:#c65696; font-weight: bold;""><span style=""font-family:'Open Sans', Helvetica, tahoma; !important;"">" + ((oOffers.item_cost == 0) ? ((oComm.GetPriceDisplayType(oOffers.oeprop, oOffers.item_cost) == "") ? String.Format("{0:c}", oOffers.item_cost * oShipVars.quantity) : "<strong>" + oComm.GetPriceDisplayType(oOffers.oeprop, oOffers.item_cost) + "</strong>") : String.Format("{0:c}", oOffers.item_cost * oShipVars.quantity)) + @"</span></p></td></tr>";
                            }
                        }
                        else
                        {
                            string tmpProdlist = "";
                            foreach (var oOffers in oShipVars.OfferVars)
                            {
                                tmpProdlist += oOffers.item_desc + "<br />";
                            }
                            tmpStr += @"<tr><td width=""50%"" valign=""middle"" bgcolor=""#dbe6f0""><p style=""text-align:left; font-family:'Open Sans', Helvetica, tahoma; font-size:14px; line-height: 1.3; margin-left:25px; margin-right:0px; margin-top:0px; margin-bottom:0px;  color:#1a5eab;""><span style=""font-family:'Open Sans', Helvetica, tahoma; !important;"">"
                                      + tmpProdlist + @"</td><td align=""center"" valign=""middle"" bgcolor=""#dbe6f0""><p style=""text-align:center; font-family:'Open Sans', Helvetica, tahoma; font-size:14px; line-height: 1.3; margin-left:5px; margin-right:5px; margin-top:0px; margin-bottom:0px;  color:#1a5eab;""><span style=""font-family:'Open Sans', Helvetica, tahoma; !important;"">" + ((oShipVars.unit_price == 0) ? "<strong>FREE</strong>" : String.Format("{0:c}", oShipVars.unit_price * oShipVars.quantity)) + @"</span></p></td></tr>";
                        }
                    }
                }

                string shipping = @"<tr><td width=""50%"" valign=""top"" bgcolor=""#dbe6f0""><p style=""text-align:left; font-family:'Open Sans', Helvetica, tahoma; font-size:14px; line-height: 1.3; margin-left:25px; margin-right:0px; margin-top:0px; margin-bottom:0px;  color:#1a5eab;""><span style=""font-family:'Open Sans', Helvetica, tahoma; !important;"">Shipping &amp; Handling</span></p></td><td align=""center"" valign=""bottom"" bgcolor=""#dbe6f0""><p style=""text-align:center; font-family:'Open Sans', Helvetica, tahoma; font-size:14px; line-height: 1.3; margin-left:5px; margin-right:5px; margin-top:0px; margin-bottom:0px;  color:#c65696;font-weight: bold;""><span style=""font-family:'Open Sans', Helvetica, tahoma; !important;"">!_ship_!</span></p></td></tr>";

                if (!oVariables.ship_item_listed)
                {
                    tmpStr += shipping.Replace("!_ship_!",
                        ((oVariables.total_sah == 0)
                            ? "<strong>FREE</strong>"
                            : String.Format("{0:c}", oVariables.total_sah)));
                }

                string taxString = @"<tr><td width=""50%"" valign=""middle"" bgcolor=""#dbe6f0""><p style=""text-align:left; font-family:'Open Sans', Helvetica, tahoma; font-size:14px; line-height: 1.3; margin-left:25px; margin-right:0px; margin-top:0px; margin-bottom:0px;  color:#1a5eab;""><span style=""font-family:'Open Sans', Helvetica, tahoma; !important;"">Tax (estimated)</span></p></td><td align=""center"" valign=""middle"" bgcolor=""#dbe6f0""><p style=""text-align:center; font-family:'Open Sans', Helvetica, tahoma; font-size:14px; line-height: 1.3; margin-left:5px; margin-right:5px; margin-top:0px; margin-bottom:0px;  color:#1a5eab;""><span style=""font-family:'Open Sans', Helvetica, tahoma; !important;"">!_tax_!</span></p></td></tr>";
                tmpStr += taxString.Replace("!_tax_!", String.Format("{0:c}", oVariables.tax_amt));


                string grandTotalText = @"<tr bgcolor=""#9fbcd8""><td width=""50%"" valign=""middle""><p style=""text-align:left; font-family:'Open Sans', Helvetica, tahoma; font-size:14px; line-height: 1.3; margin-left:25px; margin-right:0px; margin-top:0px; margin-bottom:10px;  color:#143884; font-weight:bold;""><span style=""font-family:'Open Sans', Helvetica, tahoma; !important;"">GRAND TOTAL</span></p></td><td align=""center"" valign=""middle""><p style=""text-align:center; font-family:'Open Sans', Helvetica, tahoma; font-size:14px; line-height: 1.3; margin-left:5px; margin-right:5px; margin-top:0px; margin-bottom:10px;  color:#143884; font-weight:bold;""><span style=""font-family:'Open Sans', Helvetica, tahoma; !important;"">!_grandTotal_!</span></p></td></tr></table>";
                tmpStr += grandTotalText.Replace("!_grandTotal_!", String.Format("{0:c}", oVariables.total_amt + oVariables.total_sah + oVariables.tax_amt));

                emailHtml = emailHtml.Replace("!_item_list_!", tmpStr);
                emailHtml = emailHtml.Replace("!_Discount_!",
                    String.Format("{0:c}", oVariables.discount_amt));

                if (oVariables.discount_amt > 0)
                    emailHtml = emailHtml.Replace("!_hidden_!", "block");
                else
                    emailHtml = emailHtml.Replace("!_hidden_!", "none");

                emailHtml = emailHtml.Replace("!_Tax_!", String.Format("{0:c}", oVariables.tax_amt));
                emailHtml = emailHtml.Replace("!_GrandTotal_!",
                    String.Format("{0:c}", oVariables.total_amt + oVariables.total_sah + oVariables.tax_amt));
                emailHtml = emailHtml.Replace("!_ShipAddress_!", ship_address);
                emailHtml = emailHtml.Replace("!_BillAddress_!", bill_address);
                emailHtml = emailHtml.Replace("!_conf_pg_tac_!", oVariables.PageVars[0].conf_pg_tac);
                emailHtml = emailHtml.Replace("!_reward_code_!", "");
                emailHtml = emailHtml.Replace("!_special_text_!", oVariables.special_text);
                emailHtml = emailHtml.Replace("!_email_!", oVariables.email);

                string pixel =
                    "<iframe src='https://services.earlymoments.com/ping/p.ashx?source=7629&brand=e&data=" +
                    oVariables.email.Trim() +
                    "&type=ifr' height='1' width='1' frameborder='0'></iframe><br /><img src='https://services.earlymoments.com/ping/p.ashx?source=7629&brand=e&data=" +
                    oVariables.email.Trim() + "&type=img' width='1' height='1' border='0' />";
                emailHtml = emailHtml.Replace("!_link_!", pixel);

                emailHtml = emailHtml.Replace("*[tr_specialText]*", projectSpecificText);
                emailHtml = emailHtml.Replace("*[tr_optOutText]*", oVariables.opt_out == "Y" ? optoutText : "");
                emailHtml = emailHtml.Replace("*[tr_customerFirstName]*", tempBillFname);
                emailHtml = emailHtml.Replace("*[tr_orderSummary]*", tmpStr);
                emailHtml = emailHtml.Replace("*[tr_specialPromotionalText]*", "");
                emailHtml = emailHtml.Replace("*[tr_childName]*",
                    oVariables.ShipVars[oVariables.default_shp_id].child_fname.Trim().Length == 0
                        ? "your child"
                        : oVariables.ShipVars[oVariables.default_shp_id].child_fname);

                emailHtml = emailHtml.Replace("*[tr_orderNumber]*", oVariables.order_id.ToString());

                var adSpaceHTML = @"<p style=""text-align:left; font-family:'Open Sans', Helvetica, tahoma; font-size:14px; line-height: 1.3; margin-left:35px; margin-right:35px; margin-top:5px;  color:#1a5eab;""><span style=""font-family:'Open Sans', Helvetica, tahoma; !important;""><strong> ***NOTICE: This order qualifies for a CASH BACK REBATE!***&nbsp; <a href=""http://click.email.earlymoments.com/?qs=d4b5be4b2347ace6d8fe6a38c7b3bbfb0035b5c3bf0f285c63a0c9f929518fe74f384500f81401f5"" target=""_blank"">Click Here</a></strong></span></p>";
                emailHtml = emailHtml.Replace("*[tr_adSpace]*", adSpaceHTML);

                var expertSender = new ExpertSender();

                var subscriber = expertSender.AddSubscriber(oVariables.email, oVariables.bill_to_fname,
                    oVariables.bill_to_lname, oVariables.vendor_id, oVariables.ip_address, oVariables.order_id.ToString());
                ////Response.Write("subscriber: " + subscriber);

                ////string __email = ConfigurationManager.AppSettings["tempConfEmail"];
                var emailTran = expertSender.PrepareTransactionalEmail(oVariables.email, emailHtml, oVariables.order_id.ToString(), oVariables.proj_desc);
                ////Response.Write("emailTran: " + emailTran);
            }
            catch (Exception ex)
            {
                oComm.SendEmail("ProcessConfirmationEmail() <br />Exception Raised from Confirmaiton Page in EM Landers. Exception: " + ex.Message.ToString() + ".<br />More Data (Offer URL): "
                    + oVariables.referring_url + ". <br />More Data (Order Id):" + oVariables.order_id);
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


        public static string GetError(string errorcode)
        {
            string errMessage =
                "<div align=\'center\' class=\'FormTable\' style=\'padding-top: 10px;color:red; font-weight:400; font-size:16px; text-align:center; line-height:20px;\' id=\'status\' runat=\'server\'>\r\n" +
                "<strong>Thank you for your interest in Our Products.</strong><br />\r\n    </div>\r\n    <div align=\'center\' style=\'font-weight:300; font-size:14px; text-align:center; line-height:20px;\'>\r\n\t\t!_errorMessage_!\r\n</div>";

            string default_message =
                "Due to technical difficulties, we are not able to process your order.<br />\r\nPlease call us at the number below to place an order via phone.<br />\r\n" +
                "Thank you again for your interest and understanding. Phone: <a href=\'https://www.earlymoments.com/Customer-Care/\'>\r\n1-800-353-3140</a>";
            object result = null;

            try
            {
                if (!string.IsNullOrEmpty(errorcode))
                {
                    if (Convert.ToInt32(errorcode) > 0)
                    {
                        string serviceUrl = System.Configuration.ConfigurationManager.AppSettings["iservice"].ToString();
                        serviceUrl += "errorMessage?ErrorCode=" + errorcode + "&format=json";
                        connectIServices connectIServices = new connectIServices();
                        string obj = connectIServices.returnJson(serviceUrl);
                        var yourOjbect = new JavaScriptSerializer().DeserializeObject(obj);
                        List<object> resultList = new List<object>();
                        Dictionary<string, object> dictStr = new Dictionary<string, object>();
                        dictStr = (Dictionary<string, object>)yourOjbect;

                        //var errorcd = dictStr.Where(b => b.Key == "ErrorCode").Select(b => b.Value).FirstOrDefault();
                        result = dictStr.Where(b => b.Key == "Message").Select(b => b.Value).FirstOrDefault();
                    }
                }

                if (Convert.ToInt32(errorcode) <= 0)
                {
                    result = default_message;
                }
            }
            catch (Exception)
            {
                result = default_message;
            }
            return errMessage.Replace("!_errorMessage_!", result != null ? Convert.ToString(result) : "");
        }
    }
}