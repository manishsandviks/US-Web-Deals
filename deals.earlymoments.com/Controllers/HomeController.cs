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
                            ProcessConfirmationEmail(oVariables, shipping_address, billing_address, shipall);
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

        public ActionResult OrderStatus()
        {
            return View();
        }

        public ActionResult ThankYou()
        {
            return View();
        }

        private void ProcessConfirmationEmail(OrderVariables oVariables, string ship_address, string bill_address, string shipall)
        {
            StreamReader emailReader;
            OrderProcess oProcess = new OrderEngine.OrderProcess();
            CommonModels oComm = new CommonModels();
            try
            {
                if (oVariables.order_id > 0)
                {
                    string emailHTML = "";


                    if (oVariables.email_template == "4")
                    {
                        oProcess.ProcessConfirmationEmails(oVariables);
                    }
                    else
                    {
                        string tmp = oProcess.BuildConfirmationEmail(oVariables.email_template);

                        if (tmp.Trim().Length == 0) { tmp = "general.htm"; }

                        //string filePath = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "assets\\emails\\" + tmp;
                        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Assets\emails\" + tmp);
                        emailReader = new StreamReader(filePath);
                        emailHTML = emailReader.ReadToEnd();
                        if (emailHTML.Length > 0)
                        {
                            string temp_bill_fname = oVariables.bill_to_fname.Length > 0 ? oVariables.bill_to_fname.ToUpper().Substring(0, 1) + oVariables.bill_to_fname.ToLower().Substring(1) : "";
                            string temp_bill_lname = oVariables.bill_to_lname.Length > 0 ? oVariables.bill_to_lname.ToUpper().Substring(0, 1) + oVariables.bill_to_lname.ToLower().Substring(1) : "";

                            string non_optput_text = "Visit us online at <a href='https://www.earlymoments.com' target='_blank'>www.earlymoments.com</a><br />";

                            emailHTML = emailHTML.Replace("!_spl_optout_text_!", non_optput_text);

                            emailHTML = emailHTML.Replace("!_BillName_!", temp_bill_fname);
                            emailHTML = emailHTML.Replace("!_OrderNumber_!", Convert.ToString(oVariables.order_id));

                            string tmp_str = "<tr bgcolor='#eeeeee'><td style='width: 293px;'><div style='color: #333333; float: left;'><strong>Item Description</strong></div></td><td width='60px' valign='middle' align='right'><div style='color: #333333; float: left; display:table-cell; vertical-align:middle;'><strong>Item Price</strong></div></td></tr>";
                            string space_column = "<tr height='5px''><td colspan='2'></td></tr>";

                            string prod = oVariables.proj_desc;

                            if (Convert.ToBoolean(shipall)) { prod += "<br>(<span style='font-size:9px;'>Will be shipped all at once</span>)"; }

                            tmp_str += "<tr><td style='width: 293px;' valign='top' align='left'>" + prod + "</td><td style='width: 60px;' valign='top' align='right'><div style='float: right; display:table-cell; vertical-align:middle;'>" + ((oVariables.ShipVars[0].unit_price == 0) ? "<strong>FREE</strong>" : String.Format("{0:c}", oVariables.ShipVars[0].unit_price)) + "</div></td></tr>";

                            string shipping = "<tr><td style='width: 293px;' valign='top' align='left'>Shipping and Handling</td><td style='width: 60px;' valign='top' align='right'><div style='float: right; display:table-cell; vertical-align:middle;'>!_ship_!</div></td></tr>";

                            if (!oVariables.ship_item_listed) { tmp_str += shipping.Replace("!_ship_!", ((oVariables.total_sah == 0) ? "<strong>FREE</strong>" : String.Format("{0:c}", oVariables.total_sah))); }

                            emailHTML = emailHTML.Replace("!_item_list_!", tmp_str);
                            emailHTML = emailHTML.Replace("!_Discount_!", String.Format("{0:c}", oVariables.discount_amt));

                            if (oVariables.discount_amt > 0)
                                emailHTML = emailHTML.Replace("!_hidden_!", "block");
                            else
                                emailHTML = emailHTML.Replace("!_hidden_!", "none");

                            emailHTML = emailHTML.Replace("!_Tax_!", String.Format("{0:c}", oVariables.tax_amt));
                            emailHTML = emailHTML.Replace("!_GrandTotal_!", String.Format("{0:c}", oVariables.total_amt + oVariables.total_sah + oVariables.tax_amt));
                            emailHTML = emailHTML.Replace("!_ShipAddress_!", ship_address);
                            emailHTML = emailHTML.Replace("!_BillAddress_!", bill_address);
                            emailHTML = emailHTML.Replace("!_conf_pg_tac_!", oVariables.PageVars[0].conf_pg_tac);
                            emailHTML = emailHTML.Replace("!_reward_code_!", "");
                            emailHTML = emailHTML.Replace("!_special_text_!", oVariables.special_text);
                            emailHTML = emailHTML.Replace("!_email_!", oVariables.email);

                            string pixel = "<iframe src='https://services.earlymoments.com/ping/p.ashx?source=7629&brand=e&data=" + oVariables.email.Trim() + "&type=ifr' height='1' width='1' frameborder='0'></iframe><br /><img src='https://services.earlymoments.com/ping/p.ashx?source=7629&brand=e&data=" + oVariables.email.Trim() + "&type=img' width='1' height='1' border='0' />";
                            emailHTML = emailHTML.Replace("!_link_!", pixel);
                            if ((oVariables.bill_to_fname.ToLower().Trim() == "xyz") && (oVariables.bill_to_lname.ToLower().Trim() == "xyz"))
                            {
                                oComm.SendEmail("\"Early Moments\"CustomerCare@Earlymoments.com", oVariables.email, "Confirmation of EarlyMoments Order # " + oVariables.order_id.ToString(), emailHTML, "");
                            }
                            else
                            {
                                try
                                {
                                    oProcess.LogConfirmationEmail(emailHTML, oVariables.cart_id.ToString());
                                }
                                catch (Exception ex)
                                {
                                    oComm.SendEmail("", "", "EM Confirmation Page: Important", "Error while logging Confirmation email to Database - error: " + ex.Message.ToString(), "");
                                }
                            }
                        }
                    }
                }
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
    }
}