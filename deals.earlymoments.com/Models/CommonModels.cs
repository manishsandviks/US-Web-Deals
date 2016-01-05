using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Configuration;
using OrderEngine;
using System.IO;
using System.Data;
using System.Data.SqlClient;

namespace deals.earlymoments.com.Models
{
    public class CommonModels
    {
        public static double GetRegions(string areaId)
        {
            OrderEngine.CommonMethods oComm = new OrderEngine.CommonMethods();
            double tax = oComm.ReturnTax("BRU", addZeros(areaId, 5));
            return tax;
        }

        public static string addZeros(string value, int len)
        {
            for (int i = value.Length; i < len; i++)
            {
                value = "0" + value;
            }
            return value;
        }

        public void SendEmail(string from, string to, string subject, string body, string smtp)
        {
            MailMessage oMail;
            SmtpClient oSmtp;
            try
            {
                string smtpserverUrl = ConfigurationManager.AppSettings["SMTPConn"].ToString();
                if (smtp != "") { smtpserverUrl = smtp; }
                oMail = new MailMessage(from, to);
                oMail.Subject = subject;
                oMail.Body = body;
                oMail.IsBodyHtml = true;
                oSmtp = new SmtpClient(smtpserverUrl);
                oSmtp.Send(oMail);

                //MailMessage myMail = new MailMessage();
                //myMail.To.Add(to);
                //myMail.From = new MailAddress("manish.sf0103@gmail.com");
                //myMail.CC.Add("manish.singh@globsyn.com");
                //myMail.Subject = subject;

                //myMail.Body = body;
                //myMail.IsBodyHtml = true;
                //SmtpClient mySmtp = new SmtpClient();
                //mySmtp.Host = "smtp.gmail.com";
                //mySmtp.Port = 587;
                //mySmtp.UseDefaultCredentials = false;
                //mySmtp.Credentials = new System.Net.NetworkCredential
                //("manish.sf0103@gmail.com", "GunjanSingh");// Enter seders User name and password  
                //mySmtp.EnableSsl = true;
                //mySmtp.Send(myMail);
            }
            catch
            {
                // Do Nothing
            }
            finally
            {
                oSmtp = null;
                oMail = null;
            }
        }

        public void SendEmail(string body)
        {
            body += "<br />Error in - " + HttpContext.Current.Request.Url.ToString();
            SendEmail(ConfigurationManager.AppSettings["err_em_from"].ToString(), ConfigurationManager.AppSettings["err_em_to"].ToString(), ConfigurationManager.AppSettings["err_em_subj"].ToString(), body, "");
        }

        public void SendEmail(string subject, string body)
        {
            body += "<br />Error in - " + HttpContext.Current.Request.Url.ToString();
            SendEmail(ConfigurationManager.AppSettings["err_em_from"].ToString(), ConfigurationManager.AppSettings["err_em_to"].ToString(), subject, body, "");
        }

        public void SendEmail(string subject, string body, string mailTo)
        {
            SendEmail(ConfigurationManager.AppSettings["contact_customercare"].ToString(), mailTo, subject, body, "");
        }
        public void SendEmail(string subject, string body, string mailTo, string from)
        {
            SendEmail(from, mailTo, subject, body, "");
        }

        public string GetIPAddress()
        {
            try
            {
                string VisitorsIPAddr = string.Empty;

                if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                {
                    VisitorsIPAddr = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                }
                else if (HttpContext.Current.Request.UserHostAddress.Length != 0)
                {
                    VisitorsIPAddr = HttpContext.Current.Request.UserHostAddress;
                }
                return VisitorsIPAddr.Split(',')[0].ToString();
            }
            catch (Exception ex)
            {
                SendEmail("Error in finding client IP address on EM Enrollments pages - " + ex.Message.ToString());
                return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
        }
        public string LogBrowserCapabilities(System.Web.HttpBrowserCapabilities browser)
        {
            string rtnString = "";
            try
            {
                rtnString = "Browser Capabilities\n"
                            + "Type = " + browser.Type + "\n"
                            + "Name = " + browser.Browser + "\n"
                            + "Version = " + browser.Version + "\n"
                            + "Major Version = " + browser.MajorVersion + "\n"
                            + "Minor Version = " + browser.MinorVersion + "\n"
                            + "Platform = " + browser.Platform + "\n"
                            + "Is Beta = " + browser.Beta + "\n"
                            + "Is Crawler = " + browser.Crawler + "\n"
                            + "Is AOL = " + browser.AOL + "\n"
                            + "Is Win16 = " + browser.Win16 + "\n"
                            + "Is Win32 = " + browser.Win32 + "\n"
                            + "Supports Frames = " + browser.Frames + "\n"
                            + "Supports Tables = " + browser.Tables + "\n"
                            + "Supports Cookies = " + browser.Cookies + "\n"
                            + "Supports VBScript = " + browser.VBScript + "\n"
                            + "Supports JavaScript = " + browser.EcmaScriptVersion.ToString() + "\n"
                            + "Supports Java Applets = " + browser.JavaApplets + "\n"
                            + "Supports ActiveX Controls = " + browser.ActiveXControls + "\n"
                            + "Supports JavaScript Version = " + browser["JavaScriptVersion"] + "\n";
            }
            catch (Exception ex)
            {
                rtnString = ex.Message.ToString();
            }
            return rtnString;
        }

        public string Confirmation_ShippingAddress(OrderVariables oVars)
        {
            string rtn_str = "";
            int i = 0;
            bool isNotOneShot = false;

            while (i < oVars.ShipVars.Count)
            {
                if (oVars.ShipVars[i].offer_type == "B")
                {
                    isNotOneShot = true;
                    if (oVars.ShipVars[i].ship_to_fname.Trim().Length > 0)
                    {
                        rtn_str = oVars.ShipVars[i].ship_to_fname.ToUpper().Substring(0, 1) + oVars.ShipVars[i].ship_to_fname.ToLower().Substring(1) + " " + oVars.ShipVars[i].ship_to_lname.ToUpper().Substring(0, 1) + oVars.ShipVars[i].ship_to_lname.ToLower().Substring(1) + "<br />";
                        rtn_str += (oVars.ShipVars[i].ship_to_apt != null && (oVars.ShipVars[i].ship_to_apt.ToString().Trim().Length > 0) ? oVars.ShipVars[i].ship_to_apt + ", " : "") + oVars.ShipVars[i].ship_to_address1;
                        rtn_str += "<br />" + oVars.ShipVars[i].ship_to_city + ", " + oVars.ShipVars[i].ship_to_state + " " + oVars.ShipVars[i].ship_to_zipcode;
                        break;
                    }
                }
                i++;
            }

            // For offers like Ornamets, where there is NO "BASE" offer, uses the below code to generate the shipping details.
            //if (!isNotOneShot)
            //{
            //    if (rtn_str.Length == 0)
            //    {
            //        i = 0;
            //        while (i < oVars.ShipVars.Count)
            //        {
            //            if (oVars.ShipVars[i].offer_type == "O")
            //            {
            //                if (oVars.ShipVars[i].ship_to_fname.Trim().Length > 0)
            //                {
            //                    rtn_str = oVars.ShipVars[i].ship_to_fname.ToUpper().Substring(0, 1) + oVars.ShipVars[i].ship_to_fname.ToLower().Substring(1) + " " + oVars.ShipVars[i].ship_to_lname.ToUpper().Substring(0, 1) + oVars.ShipVars[i].ship_to_lname.ToLower().Substring(1) + "<br />";
            //                    rtn_str += ((oVars.ShipVars[i].ship_to_apt.ToString().Trim().Length > 0) ? oVars.ShipVars[i].ship_to_apt + ", " : "") + oVars.ShipVars[i].ship_to_address1;
            //                    rtn_str += "<br />" + oVars.ShipVars[i].ship_to_city + ", " + oVars.ShipVars[i].ship_to_state + " " + oVars.ShipVars[i].ship_to_zipcode;
            //                    break;
            //                }
            //            }
            //            i++;
            //        }
            //    }
            //}

            return rtn_str;
        }
        public string Confirmation_BillingAddress(OrderVariables oVars, string template)
        {
            string rtn_str = "";

            try
            {
                if ((oVars.bill_to_fname.Length > 0) && (oVars.bill_to_lname.Length > 0))
                {
                    if (template == "gift")
                    {
                        rtn_str = oVars.bill_to_fname.ToUpper().Substring(0, 1) + oVars.bill_to_fname.ToLower().Substring(1) + " " + oVars.bill_to_lname.ToUpper().Substring(0, 1) + oVars.bill_to_lname.ToLower().Substring(1);
                    }
                    else
                    {
                        rtn_str = oVars.bill_to_fname.ToUpper().Substring(0, 1) + oVars.bill_to_fname.ToLower().Substring(1) + " " + oVars.bill_to_lname.ToUpper().Substring(0, 1) + oVars.bill_to_lname.ToLower().Substring(1) + "<br />";
                        rtn_str += ((oVars.bill_to_apt != null && oVars.bill_to_apt.ToString().Trim().Length > 0) ? oVars.bill_to_apt + ", " : "") + oVars.bill_to_address1;
                        rtn_str += "<br />" + oVars.bill_to_city + ", " + oVars.bill_to_state + " " + oVars.bill_to_zipcode;
                    }
                }
                else
                {
                    rtn_str = "";
                }
            }
            catch
            {
                rtn_str = "";
            }
            return rtn_str;
        }

        public string ResponsivePayment_GiftingProducts(OrderVariables oVars, bool option = false, string template = "")
        {
            string start_product_list = "<table id='txlCart' border='0' width='99%'><tr><td class='bold ash-txt border-dashed'>Item</td><td class='text-right bold ash-txt border-dashed'>Price</td></tr>";
            string prod_list = "<tr><td class='paddingBT3'>!_product_!</td><td class='text-right'>!_cost_!</td></tr>";
            string space_column = "<tr height='5px''><td colspan='2'></td></tr>";
            string dotted_line = "<tr><td colspan='2'><div style='border-bottom: dashed thin #999999; padding-top: 10px; margin-bottom: 10px;clear: both;'></div></td></tr><tr><td colspan='2'></td></tr><tr>";
            string shipping = "<tr><td class='paddingBT3'>Shipping and Handling</td><td class='text-right'>!_ship_!</td></tr>";
            string coupon_text = "<tr><td class='paddingBT3'>Coupon/Discount</td><td class='text-right'>-!_discount_!</td></tr>";
            string tax = "<tr><td class='paddingBT3'>&nbsp;&nbsp;Tax</td><td class='text-right'>!_tax_!</td></tr>";
            string bottom_extra_line = "<tr><td colspan='2' class='paddingBT3'>!_extra_content_!</td></tr>";
            string end_product_list = "</table>";
            string prod = oVars.proj_desc;

            bool isQty = false;
            foreach (ShippingVariables qtyVariables in oVars.ShipVars) { if (qtyVariables.quantity > 1) { isQty = true; } }

            bool eBooks = false;

            if (!isQty)
            {
                if (option) { prod += "<br>(<span style='font-size:9px;'>Will be shipped all at once</span>)"; }

                if (template == "gift")
                {
                    prod_list = prod_list.Replace("!_product_!", prod).Replace("!_cost_!", ((oVars.ShipVars[0].unit_price == 0) ? "<strong>FREE</strong>" : String.Format("{0:c}", oVars.ShipVars[0].unit_price)));
                    prod_list += space_column;
                }
                else
                {
                    prod_list = "";
                    foreach (OrderEngine.ShippingVariables oShipVars in oVars.ShipVars)
                    {
                        if (oShipVars.selected)
                        {
                            if ((OfferItemsDisplayType(oShipVars.OfferVars) > 1) || (oShipVars.OfferVars.Count == 1))
                            {
                                foreach (OrderEngine.OfferProperties oOffers in oShipVars.OfferVars)
                                {
                                    eBooks = oOffers.offer_item == "EBOOK" ? true : false;

                                    prod_list += "<tr><td class='paddingBT3'>" + (eBooks ? "<strong>" + oOffers.item_desc + "</strong>" : oOffers.item_desc) +
                                        "</td><td class='text-right'>" +
                                        ((oOffers.item_cost == 0) ? ((GetPriceDisplayType(oOffers.oeprop, oOffers.item_cost) == "") ? String.Format("{0:c}", oOffers.item_cost) :
                                        GetPriceDisplayType(oOffers.oeprop, oOffers.item_cost)) : String.Format("{0:c}", oOffers.item_cost)) + "</td></tr>";
                                }
                                prod_list += space_column;
                            }
                            else
                            {
                                string tmp_prodlist = "";
                                foreach (OrderEngine.OfferProperties oOffers in oShipVars.OfferVars)
                                {
                                    tmp_prodlist += oOffers.item_desc + "<br />";
                                }
                                prod_list += "<tr><td class='paddingBT3'>" + tmp_prodlist + "</td><td class='text-right'>" + ((oShipVars.unit_price == 0) ? "<strong>FREE</strong>" : String.Format("{0:c}", oShipVars.unit_price)) + "</td></tr>";
                                prod_list += space_column;
                            }
                        }
                    }
                }

                start_product_list += prod_list;
                // This will display Shipping and Handing ONLY when there is NO s&h item listed in the offers.
                if (!oVars.ship_item_listed) { start_product_list += shipping.Replace("!_ship_!", ((oVars.total_sah == 0) ? "<strong>FREE</strong>" : String.Format("{0:c}", oVars.total_sah))); }

                start_product_list += dotted_line;

                if (oVars.discount_amt > 0) { start_product_list += coupon_text.Replace("!_discount_!", String.Format("{0:c}", oVars.discount_amt)); }

                start_product_list += tax.Replace("!_tax_!", String.Format("{0:c}", oVars.tax_amt));

                if (eBooks)
                {
                    if (oVars.opt_out == "Y")
                        start_product_list += bottom_extra_line.Replace("!_extra_content_!", "<img src='Assets/images/payment/ebooks_em_payment_banner_1.jpg' />");
                    else
                        start_product_list += bottom_extra_line.Replace("!_extra_content_!", "<img src='Assets/images/payment/ebooks_em_payment_banner.jpg' />");
                }


                return start_product_list += end_product_list;

            }
            else
            {
                start_product_list = "<table style='border: none; padding: 0px; width:99%;'>"
                    + "<tr><td><strong>Item</strong></td>"
                    + "<td><strong>Price</strong></td>"
                    + "<td><strong>Quantity</strong></td>"
                    + "<td style='padding-left:30px'><strong>Subtotal</strong></td>"
                    + "</tr><tr><td colspan='4'><div style='border-bottom: dashed thin #999999; margin-bottom: 10px; clear: both;'></div></td></tr>";


                prod_list = "<tr><td><div style='color: #333333; float: left;'>!_product_!</div></td>"
                    + "<td><div style='color: #333333; float: left; display:table-cell; vertical-align:middle;'>!_cost_!</div></td>"
                    + "<td><div style='color: #333333; padding-left:20px; float: left; display:table-cell; vertical-align:middle;'>!_quantity_!</div></td>"
                    + "<td><div style='color: #333333; padding-left: 10px; float: right; display:table-cell; vertical-align:middle;'>!_subtotal_!</div></td></tr>";

                space_column = "<tr height='5px''><td colspan='4'></td></tr>";
                dotted_line = "<tr><td colspan='4'><div style='border-bottom: dashed thin #999999; padding-top: 10px; margin-bottom: 10px;clear: both;'></div></td></tr><tr><td colspan='4'></td></tr><tr>";

                coupon_text = "<tr><td><strong>Coupon/Discount</strong></td><td><strong>-!_discount_!</strong></td></tr>";

                shipping = "<tr><td valign='middle' colspan='3' width='99%'><strong>Shipping and Handling</strong></td>"
                    + "<td valign='middle' style='padding-left:50px;float:right;'><strong>!_ship_!</strong></td></tr>";

                tax = "<tr><td valign='middle' colspan='3' width='99%'><strong>Tax</strong></td>"
                    + "<td valign='middle' style='padding-left:50px;float:right;'><strong>!_tax_!</strong></td></tr>";

                end_product_list = "</table>";
                prod = oVars.proj_desc;

                if (option) { prod += "<br>(<span style='font-size:9px;'>Will be shipped all at once</span>)"; }

                if (template == "gift")
                {
                    prod_list = prod_list.Replace("!_product_!", prod).Replace("!_cost_!", ((oVars.ShipVars[0].unit_price == 0) ? "<strong>FREE</strong>" : String.Format("{0:c}", oVars.ShipVars[0].unit_price)));
                    prod_list += space_column;
                }
                else
                {
                    prod_list = "";
                    foreach (OrderEngine.ShippingVariables oShipVars in oVars.ShipVars)
                    {
                        if (oShipVars.selected)
                        {
                            if ((OfferItemsDisplayType(oShipVars.OfferVars) > 1) || (oShipVars.OfferVars.Count == 1))
                            {
                                foreach (OrderEngine.OfferProperties oOffers in oShipVars.OfferVars)
                                {
                                    prod_list += "<tr><td><div style='color: #333333; float: left;'>" + oOffers.item_desc + "</div></td>"
                                        + "<td><div style='color: #333333; float: left; display:table-cell; vertical-align:middle;'>" + ((oOffers.item_cost == 0) ? ((GetPriceDisplayType(oOffers.oeprop, oOffers.item_cost) == "") ? String.Format("{0:c}", oOffers.item_cost) : "<strong>" + GetPriceDisplayType(oOffers.oeprop, oOffers.item_cost) + "</strong>") : String.Format("{0:c}", oOffers.item_cost)) + "</div></td>"
                                        + "<td align='center'><div style='color: #333333; padding-left: 20px; float: left; display:table-cell; vertical-align:middle;'>" + oShipVars.quantity + "</div></td>"
                                        + "<td><div style='color: #333333; padding-left: 10px; float: right; display:table-cell; vertical-align:middle;'>" + ((oOffers.item_cost == 0) ? ((GetPriceDisplayType(oOffers.oeprop, oOffers.item_cost) == "") ? String.Format("{0:c}", oOffers.item_cost * oShipVars.quantity) : "<strong>" + GetPriceDisplayType(oOffers.oeprop, oOffers.item_cost) + "</strong>") : String.Format("{0:c}", oOffers.item_cost * oShipVars.quantity)) + "</div></td>"
                                        + "</tr>";
                                }
                                prod_list += space_column;
                            }
                            else
                            {
                                string tmp_prodlist = "";
                                foreach (OrderEngine.OfferProperties oOffers in oShipVars.OfferVars)
                                {
                                    tmp_prodlist += oOffers.item_desc + "<br />";
                                }
                                prod_list += "<tr><td style='width: 260px;'><div style='color: #333333; padding-left: 20px; width: 260px; float: left;'>"
                                    + tmp_prodlist
                                    + "</div></td><td style='width: 50px;' valign='middle' align='right'><div style='color: #333333; padding-left: 20px; width: 50px; float: left; display:table-cell; vertical-align:middle;'>"
                                    + ((oShipVars.unit_price == 0) ? "<strong>FREE</strong>" : String.Format("{0:c}", oShipVars.unit_price))
                                    + "</div></td></tr>";
                                prod_list += space_column;
                            }
                        }
                    }
                }
                start_product_list += prod_list;
                start_product_list += dotted_line;

                if (oVars.discount_amt > 0) { start_product_list += coupon_text.Replace("!_discount_!", String.Format("{0:c}", oVars.discount_amt)); }

                // This will display Shipping and Handing ONLY when there is NO s&h item listed in the offers.
                if (!oVars.ship_item_listed) { start_product_list += shipping.Replace("!_ship_!", ((oVars.total_sah == 0) ? "<strong>$0.00</strong>" : String.Format("{0:c}", oVars.total_sah))); }
                start_product_list += tax.Replace("!_tax_!", String.Format("{0:c}", oVars.tax_amt));

                return start_product_list += end_product_list;
            }
        }



        public string ResponsiveConfirmation_GiftingProducts(OrderVariables oVars, bool option = false, string template = "")
        {
            string start_product_list = "<table id='tblCart' class='table table-striped bold'><tr><td class='text-center'>QTY</td><td class='text-center'>Item</td><td class='text-center'>Price</td></tr>";
            string prod_list = "<tr><td class='paddingBT3'>!_product_!</td><td class='text-right'>!_cost_!</td></tr>";
            string space_column = "<tr height='5px'><td colspan='3'></td></tr>";
            string dotted_line = "<tr><td colspan='2'><div style='border-bottom: dashed thin #999999; padding-top: 10px; margin-bottom: 10px;clear: both;'></div></td></tr><tr><td colspan='2'></td></tr><tr>";

            string shipping = "<tr><td class='paddingBT3' colspan='2'>Shipping and Handling</td><td class='text-right'>!_ship_!</td></tr>";

            string coupon_text = "<tr><td class='paddingBT3'><strong>Voucher / Coupon / Discount</strong></td><td class='text-right'>!_discount_!</td></tr>";

            string tax = "<tr><td class='paddingBT3 bold' colspan='2'>Tax</td><td class='text-right bold'>!_tax_!</td></tr>";
            string total = "<tr><td class='paddingBT3 bold' colspan='2'>Total</td><td class='text-right bold'>!_total_!</td></tr>";
            string end_product_list = "</table>";
            string prod = oVars.proj_desc;

            bool isQty = false;
            foreach (ShippingVariables qtyVariables in oVars.ShipVars) { if (qtyVariables.quantity > 1) { isQty = true; } }

            bool eBooks = false;

            if (!isQty)
            {
                if (option) { prod += "<br>(<span style='font-size:9px;'>Will be shipped all at once</span>)"; }

                if (template == "gift")
                {
                    prod_list = prod_list.Replace("!_product_!", prod).Replace("!_cost_!", ((oVars.ShipVars[0].unit_price == 0) ? "<strong>FREE</strong>" : String.Format("{0:c}", oVars.ShipVars[0].unit_price)));
                    prod_list += space_column;
                }
                else
                {
                    prod_list = "";
                    foreach (OrderEngine.ShippingVariables oShipVars in oVars.ShipVars)
                    {
                        if (oShipVars.selected)
                        {
                            if ((OfferItemsDisplayType(oShipVars.OfferVars) > 1) || (oShipVars.OfferVars.Count == 1))
                            {
                                foreach (OrderEngine.OfferProperties oOffers in oShipVars.OfferVars)
                                {
                                    eBooks = oOffers.offer_item == "EBOOK" ? true : false;

                                    prod_list += "<tr><td></td>" +
                                    "<td class='paddingBT3'>" +
                                        (eBooks ? "<strong>" + oOffers.item_desc + "</strong>" : oOffers.item_desc) +
                                        "</div></td><td class='text-right'>" +
                                        ((oOffers.item_cost == 0) ? ((GetPriceDisplayType(oOffers.oeprop, oOffers.item_cost) == "") ? String.Format("{0:c}", oOffers.item_cost) : "<strong>" +
                                        GetPriceDisplayType(oOffers.oeprop, oOffers.item_cost) + "</strong>") : String.Format("{0:c}", oOffers.item_cost)) + "</td></tr>";
                                }
                                // prod_list += space_column;
                            }
                            else
                            {
                                string tmp_prodlist = "";
                                foreach (OrderEngine.OfferProperties oOffers in oShipVars.OfferVars)
                                {
                                    tmp_prodlist += oOffers.item_desc + "<br />";
                                }
                                prod_list += "<tr><td>" + oShipVars.OfferVars.Count + "</td><td class='paddingBT3'>" + tmp_prodlist + "</td><td class='text-right'>" + ((oShipVars.unit_price == 0) ? "<strong>FREE</strong>" : String.Format("{0:c}", oShipVars.unit_price)) + "</td></tr>";
                                //  prod_list += space_column;
                            }
                        }
                    }
                }

                start_product_list += prod_list;
                // This will display Shipping and Handing ONLY when there is NO s&h item listed in the offers.
                if (!oVars.ship_item_listed) { start_product_list += shipping.Replace("!_ship_!", ((oVars.total_sah == 0) ? "<strong>FREE</strong>" : String.Format("{0:c}", oVars.total_sah))); }
                // start_product_list += dotted_line;
                if (oVars.discount_amt > 0) { start_product_list += coupon_text.Replace("!_discount_!", String.Format("{0:c}", oVars.discount_amt)); }

                start_product_list += tax.Replace("!_tax_!", String.Format("{0:c}", oVars.tax_amt));

                start_product_list += total.Replace("!_total_!", String.Format("{0:c}", oVars.total_amt + oVars.tax_amt + oVars.total_sah));

                return start_product_list += end_product_list;

            }
            else
            {
                start_product_list = "<table id='tblCart' border='0' width='99%'>"
                    + "<tr><td class='bold ash-txt border-dashed'>Item</td>"
                    + "<td class='bold ash-txt border-dashed'>Price</td>"
                    + "<td class='bold ash-txt border-dashed'>Quantity</td>"
                    + "<td class='bold ash-txt border-dashed'>Subtotal</td>"
                    + "</tr>";

                prod_list = "<tr><td class='paddingBT3'>!_product_!</td>"
                    + "<td class='text-right'>!_cost_!</td>"
                    + "<td class='text-right'>!_quantity_!</td>"
                    + "<td class='text-right'>!_subtotal_!</td></tr>";

                space_column = "<tr height='5px''><td colspan='4'></td></tr>";
                dotted_line = "<tr><td colspan='4'><div style='border-bottom: dashed thin #999999; padding-top: 10px; margin-bottom: 10px;clear: both;'></div></td></tr><tr><td colspan='4'></td></tr><tr>";

                coupon_text = "<tr><td class='paddingBT3' colspan='3'>Coupon/Discount</strong></td><td class='text-right'>!_discount_!</td></tr>";

                shipping = "<tr><td class='paddingBT3' colspan='3'>Shipping and Handling</td><td class='text-right'>!_ship_!</td></tr>";

                tax = "<tr><td class='paddingBT3 bold text-right' colspan='3'>Tax</td><td class='text-right bold'>!_tax_!</td></tr>";

                total = "<tr><td class='paddingBT3 bold'  colspan='3'>Total</td>"
                    + "<td class='text-right bold'>!_total_!</td></tr>";

                end_product_list = "</table>";
                prod = oVars.proj_desc;

                if (option) { prod += "<br>(<span style='font-size:9px;'>Will be shipped all at once</span>)"; }

                if (template == "gift")
                {
                    prod_list = prod_list.Replace("!_product_!", prod).Replace("!_cost_!", ((oVars.ShipVars[0].unit_price == 0) ? "<strong>FREE</strong>" : String.Format("{0:c}", oVars.ShipVars[0].unit_price)));
                    prod_list += space_column;
                }
                else
                {
                    prod_list = "";
                    foreach (OrderEngine.ShippingVariables oShipVars in oVars.ShipVars)
                    {
                        if (oShipVars.selected)
                        {
                            if ((OfferItemsDisplayType(oShipVars.OfferVars) > 1) || (oShipVars.OfferVars.Count == 1))
                            {
                                foreach (OrderEngine.OfferProperties oOffers in oShipVars.OfferVars)
                                {
                                    prod_list += "<tr><td class='paddingBT3'>" + oOffers.item_desc + "</td>"
                                        + "<td class='text-right'>" + ((oOffers.item_cost == 0) ? ((GetPriceDisplayType(oOffers.oeprop, oOffers.item_cost) == "") ? String.Format("{0:c}", oOffers.item_cost) : "<strong>" + GetPriceDisplayType(oOffers.oeprop, oOffers.item_cost) + "</strong>") : String.Format("{0:c}", oOffers.item_cost)) + "</td>"
                                        + "<td class='text-right'>" + oShipVars.quantity + "</td>"
                                        + "<td class='text-right'>" + ((oOffers.item_cost == 0) ? ((GetPriceDisplayType(oOffers.oeprop, oOffers.item_cost) == "") ? String.Format("{0:c}", oOffers.item_cost * oShipVars.quantity) : "<strong>" + GetPriceDisplayType(oOffers.oeprop, oOffers.item_cost) + "</strong>") : String.Format("{0:c}", oOffers.item_cost * oShipVars.quantity)) + "</td>"
                                        + "</tr>";
                                }
                                prod_list += space_column;
                            }
                            else
                            {
                                string tmp_prodlist = "";
                                foreach (OrderEngine.OfferProperties oOffers in oShipVars.OfferVars)
                                {
                                    tmp_prodlist += oOffers.item_desc + "<br />";
                                }
                                prod_list += "<tr><td class='paddingBT3'>"
                                    + tmp_prodlist
                                    + "</td><td class='text-right'>"
                                    + ((oShipVars.unit_price == 0) ? "<strong>FREE</strong>" : String.Format("{0:c}", oShipVars.unit_price))
                                    + "</td></tr>";
                                prod_list += space_column;
                            }
                        }
                    }
                }

                start_product_list += prod_list;
                start_product_list += dotted_line;

                if (oVars.discount_amt > 0) { start_product_list += coupon_text.Replace("!_discount_!", String.Format("{0:c}", oVars.discount_amt)); }

                // This will display Shipping and Handing ONLY when there is NO s&h item listed in the offers.
                if (!oVars.ship_item_listed) { start_product_list += shipping.Replace("!_ship_!", ((oVars.total_sah == 0) ? "<strong>$0.00</strong>" : String.Format("{0:c}", oVars.total_sah))); }
                start_product_list += tax.Replace("!_tax_!", String.Format("{0:c}", oVars.tax_amt));
                start_product_list += total.Replace("!_total_!", String.Format("{0:c}", oVars.total_amt + oVars.tax_amt + oVars.total_sah));

                return start_product_list += end_product_list;
            }
        }

        public string GetPriceDisplayType(string oeprop, double unit_price)
        {
            string rtnvalue = "";

            if ((oeprop == "1") && (unit_price == 0.0))
            {
                rtnvalue = "INCLUDED";
            }
            else if ((oeprop == "1") && (unit_price > 0.0))
            {
                rtnvalue = "";
            }
            else if ((oeprop == "2") && (unit_price == 0.0))
            {
                rtnvalue = "FREE";
            }
            else if ((oeprop == "2") && (unit_price > 0.0))
            {
                rtnvalue = "";
            }

            return rtnvalue;
        }

        public int OfferItemsDisplayType(List<OfferProperties> ofr_dtls)
        {
            int count = 0;
            foreach (var item in ofr_dtls)
            {
                if (item.item_cost > 0.00)
                {
                    count++;
                }
            }
            return count;
        }

        public Dictionary<string, string> GetOfferCreatives(OrderVariables oVars)
        {
            Dictionary<string, string> d = new Dictionary<string, string>();
            bool selected = false;
            foreach (OrderEngine.ShippingVariables oShipVars in oVars.ShipVars)
            {
                if ((oShipVars.selected) && (!selected))
                {
                    selected = true;
                    #region "Select on Project..."
                    switch (oShipVars.project)
                    {
                        case "U":
                            d.Add("project_description", "Customer level USA");
                            d.Add("confirm_header", "Assets/images/brp_confirm_header.gif");
                            d.Add("prod_image", "Assets/images/seuss_confirm_logo.gif");
                            d.Add("payment_header", "Assets/images/payment/seuss_payment_header.gif");
                            break;
                        case "ABU":
                            d.Add("project_description", "My First Steps to Learning");
                            d.Add("confirm_header", "Assets/images/brp_confirm_header.gif");
                            d.Add("prod_image", "Assets/images/seuss_confirm_logo.gif");
                            d.Add("payment_header", "Assets/images/payment/seuss_payment_header.gif");
                            break;
                        case "BBU":
                            d.Add("project_description", "Bearrific Friends Club");
                            d.Add("confirm_header", "Assets/images/brp_confirm_header.gif");
                            d.Add("prod_image", "Assets/images/seuss_confirm_logo.gif");
                            d.Add("payment_header", "Assets/images/payment/seuss_payment_header.gif");
                            break;
                        case "BDU":
                            d.Add("project_description", "Babies First Disney Books");
                            d.Add("confirm_header", "Assets/images/brp_confirm_header.gif");
                            d.Add("prod_image", "Assets/images/seuss_confirm_logo.gif");
                            d.Add("payment_header", "Assets/images/payment/seuss_payment_header.gif");
                            break;
                        case "BEU":
                            d.Add("project_description", "Baby Einstein Discoveries");
                            d.Add("confirm_header", "Assets/images/bye_confirm_header.gif");
                            d.Add("prod_image", "Assets/images/bye_logo.gif");
                            d.Add("payment_header", "Assets/images/payment/bye_payment_header.gif");
                            break;
                        case "BFU":
                            d.Add("project_description", "Baby's First Book Club");
                            d.Add("confirm_header", "Assets/images/brp_confirm_header.gif");
                            d.Add("prod_image", "Assets/images/seuss_confirm_logo.gif");
                            d.Add("payment_header", "Assets/images/payment/seuss_payment_header.gif");
                            break;
                        case "BHU":
                            d.Add("project_description", "My First Dr. Seuss & Friends");
                            d.Add("confirm_header", "Assets/images/mfs_confirm_header.gif");
                            d.Add("prod_image", "Assets/images/mfs_confirm_logo.gif");
                            d.Add("payment_header", "Assets/images/payment/mfs_payment_header.gif");
                            break;
                        case "BKU":
                            d.Add("project_description", "New Book of Knowledge");
                            d.Add("confirm_header", "Assets/images/brp_confirm_header.gif");
                            d.Add("prod_image", "Assets/images/seuss_confirm_logo.gif");
                            d.Add("payment_header", "Assets/images/payment/seuss_payment_header.gif");
                            break;
                        case "BRU":
                            d.Add("project_description", "Dr. Seuss & His Friends");
                            d.Add("confirm_header", "Assets/images/brp_confirm_header.gif");
                            d.Add("prod_image", "Assets/images/seuss_confirm_logo.gif");
                            d.Add("payment_header", "Assets/images/payment/seuss_payment_header.gif");
                            break;
                        case "BSU":
                            d.Add("project_description", "Dr. Seuss");
                            d.Add("confirm_header", "Assets/images/brp_confirm_header.gif");
                            d.Add("prod_image", "Assets/images/seuss_confirm_logo.gif");
                            d.Add("payment_header", "Assets/images/payment/seuss_payment_header.gif");
                            break;
                        case "BVU":
                            d.Add("project_description", "Parent Handbook");
                            d.Add("confirm_header", "Assets/images/brp_confirm_header.gif");
                            d.Add("prod_image", "Assets/images/seuss_confirm_logo.gif");
                            d.Add("payment_header", "Assets/images/payment/seuss_payment_header.gif");
                            break;
                        case "CBU":
                            d.Add("project_description", "Carebears");
                            d.Add("confirm_header", "Assets/images/brp_confirm_header.gif");
                            d.Add("prod_image", "Assets/images/seuss_confirm_logo.gif");
                            d.Add("payment_header", "Assets/images/payment/seuss_payment_header.gif");
                            break;
                        case "CHU":
                            d.Add("project_description", "Cat In The Hat Learning Lib");
                            d.Add("confirm_header", "Assets/images/cith_ll_confirm_header.gif");
                            d.Add("prod_image", "Assets/images/cith_ll_confirm_logo.gif");
                            d.Add("payment_header", "Assets/images/payment/cith_ll_payment_header.gif");
                            break;
                        case "CPU":
                            d.Add("project_description", "CP");
                            d.Add("confirm_header", "Assets/images/brp_confirm_header.gif");
                            d.Add("prod_image", "Assets/images/seuss_confirm_logo.gif");
                            d.Add("payment_header", "Assets/images/payment/seuss_payment_header.gif");
                            break;
                        case "DBU":
                            d.Add("project_description", "Disney Wonderful World Reading");
                            d.Add("confirm_header", "Assets/images/disney_confirm_header.jpg");
                            d.Add("prod_image", "Assets/images/dwwr_logo.gif");
                            d.Add("payment_header", "Assets/images/payment/disney_payment_header.jpg");
                            break;
                        case "DFU":
                            d.Add("project_description", "Disney First Readers");
                            d.Add("confirm_header", "Assets/images/brp_confirm_header.gif");
                            d.Add("prod_image", "Assets/images/seuss_confirm_logo.gif");
                            d.Add("payment_header", "Assets/images/payment/seuss_payment_header.gif");
                            break;
                        case "DPU":
                            d.Add("project_description", "Royal Disney Princess Club");
                            d.Add("confirm_header", "Assets/images/brp_confirm_header.gif");
                            d.Add("prod_image", "Assets/images/seuss_confirm_logo.gif");
                            d.Add("payment_header", "Assets/images/payment/seuss_payment_header.gif");
                            break;
                        case "ELU":
                            d.Add("project_description", "Elmo's Learning Adventure");
                            d.Add("confirm_header", "Assets/images/elmo_confirm_header.jpg");
                            d.Add("prod_image", "Assets/images/elmo_confirm_logo.gif");
                            d.Add("payment_header", "Assets/images/payment/elmo_payment_header.jpg");
                            break;
                        case "FMU":
                            d.Add("project_description", "My First Music Collection");
                            d.Add("confirm_header", "Assets/images/brp_confirm_header.gif");
                            d.Add("prod_image", "Assets/images/seuss_confirm_logo.gif");
                            d.Add("payment_header", "Assets/images/payment/seuss_payment_header.gif");
                            break;
                        case "HBU":
                            d.Add("project_description", "Hispanic Books Programs");
                            d.Add("confirm_header", "Assets/images/brp_confirm_header.gif");
                            d.Add("prod_image", "Assets/images/seuss_confirm_logo.gif");
                            d.Add("payment_header", "Assets/images/payment/seuss_payment_header.gif");
                            break;
                        case "HMU":
                            d.Add("project_description", "Help Me Be Good");
                            d.Add("confirm_header", "Assets/images/brp_confirm_header.gif");
                            d.Add("prod_image", "Assets/images/seuss_confirm_logo.gif");
                            d.Add("payment_header", "Assets/images/payment/seuss_payment_header.gif");
                            break;
                        case "HOU":
                            d.Add("project_description", "Hooked On Phonics");
                            d.Add("confirm_header", "Assets/images/brp_confirm_header.gif");
                            d.Add("prod_image", "Assets/images/seuss_confirm_logo.gif");
                            d.Add("payment_header", "Assets/images/payment/seuss_payment_header.gif");
                            break;
                        case "IKU":
                            d.Add("project_description", "Interactive Kids Club");
                            d.Add("confirm_header", "Assets/images/brp_confirm_header.gif");
                            d.Add("prod_image", "Assets/images/seuss_confirm_logo.gif");
                            d.Add("payment_header", "Assets/images/payment/seuss_payment_header.gif");
                            break;
                        case "LMU":
                            d.Add("project_description", "Little Mermaid Club");
                            d.Add("confirm_header", "Assets/images/brp_confirm_header.gif");
                            d.Add("prod_image", "Assets/images/seuss_confirm_logo.gif");
                            d.Add("payment_header", "Assets/images/payment/seuss_payment_header.gif");
                            break;
                        case "MBU":
                            d.Add("project_description", "MB");
                            d.Add("confirm_header", "Assets/images/brp_confirm_header.gif");
                            d.Add("prod_image", "Assets/images/seuss_confirm_logo.gif");
                            d.Add("payment_header", "Assets/images/payment/seuss_payment_header.gif");
                            break;
                        case "MCU":
                            d.Add("project_description", "Magic Castle Readers");
                            d.Add("confirm_header", "Assets/images/brp_confirm_header.gif");
                            d.Add("prod_image", "Assets/images/seuss_confirm_logo.gif");
                            d.Add("payment_header", "Assets/images/payment/seuss_payment_header.gif");
                            break;
                        case "MOU":
                            d.Add("project_description", "MO");
                            d.Add("confirm_header", "Assets/images/brp_confirm_header.gif");
                            d.Add("prod_image", "Assets/images/seuss_confirm_logo.gif");
                            d.Add("payment_header", "Assets/images/payment/seuss_payment_header.gif");
                            break;
                        case "MSU":
                            d.Add("project_description", "Miscellaneous");
                            d.Add("confirm_header", "Assets/images/brp_confirm_header.gif");
                            d.Add("prod_image", "Assets/images/seuss_confirm_logo.gif");
                            d.Add("payment_header", "Assets/images/payment/seuss_payment_header.gif");
                            break;
                        case "NCU":
                            d.Add("project_description", "Nature's Children");
                            d.Add("confirm_header", "Assets/images/brp_confirm_header.gif");
                            d.Add("prod_image", "Assets/images/seuss_confirm_logo.gif");
                            d.Add("payment_header", "Assets/images/payment/seuss_payment_header.gif");
                            break;
                        case "NJU":
                            d.Add("project_description", "Nick Jr.");
                            d.Add("confirm_header", "Assets/images/brp_confirm_header.gif");
                            d.Add("prod_image", "Assets/images/seuss_confirm_logo.gif");
                            d.Add("payment_header", "Assets/images/payment/seuss_payment_header.gif");
                            break;
                        case "POU":
                            d.Add("project_description", "Pokemon Masters Club");
                            d.Add("confirm_header", "Assets/images/brp_confirm_header.gif");
                            d.Add("prod_image", "Assets/images/seuss_confirm_logo.gif");
                            d.Add("payment_header", "Assets/images/payment/seuss_payment_header.gif");
                            break;
                        case "PRU":
                            d.Add("project_description", "Phonics Reading Program");
                            d.Add("confirm_header", "Assets/images/brp_confirm_header.gif");
                            d.Add("prod_image", "Assets/images/seuss_confirm_logo.gif");
                            d.Add("payment_header", "Assets/images/payment/seuss_payment_header.gif");
                            break;
                        case "SCU":
                            d.Add("project_description", "Scholastic Classics");
                            d.Add("confirm_header", "Assets/images/brp_confirm_header.gif");
                            d.Add("prod_image", "Assets/images/seuss_confirm_logo.gif");
                            d.Add("payment_header", "Assets/images/payment/seuss_payment_header.gif");
                            break;
                        case "SKU":
                            d.Add("project_description", "SK");
                            d.Add("confirm_header", "Assets/images/brp_confirm_header.gif");
                            d.Add("prod_image", "Assets/images/seuss_confirm_logo.gif");
                            d.Add("payment_header", "Assets/images/payment/seuss_payment_header.gif");
                            break;
                        case "STU":
                            d.Add("project_description", "Scholastic Teacher account");
                            d.Add("confirm_header", "Assets/images/brp_confirm_header.gif");
                            d.Add("prod_image", "Assets/images/seuss_confirm_logo.gif");
                            d.Add("payment_header", "Assets/images/payment/seuss_payment_header.gif");
                            break;
                        case "VTU":
                            d.Add("project_description", "Veggie Tales");
                            d.Add("confirm_header", "Assets/images/brp_confirm_header.gif");
                            d.Add("prod_image", "Assets/images/seuss_confirm_logo.gif");
                            d.Add("payment_header", "Assets/images/payment/seuss_payment_header.gif");
                            break;
                        case "WAU":
                            d.Add("project_description", "Word Advantage 220");
                            d.Add("confirm_header", "Assets/images/brp_confirm_header.gif");
                            d.Add("prod_image", "Assets/images/seuss_confirm_logo.gif");
                            d.Add("payment_header", "Assets/images/payment/seuss_payment_header.gif");
                            break;
                        case "WPU":
                            d.Add("project_description", "My First Winnie the Pooh Club");
                            d.Add("confirm_header", "Assets/images/brp_confirm_header.gif");
                            d.Add("prod_image", "Assets/images/seuss_confirm_logo.gif");
                            d.Add("payment_header", "Assets/images/payment/seuss_payment_header.gif");
                            break;
                        case "DCO":
                        case "DPO":
                        case "DOU":
                            d.Add("project_description", "Disney Character Ornaments");
                            d.Add("confirm_header", "Assets/images/dco_confirm_header.gif");
                            d.Add("prod_image", "Assets/images/dco_logo_confirm.gif");
                            d.Add("payment_header", "Assets/images/payment/dco_payment_header.gif");
                            break;
                    }
                    #endregion
                }
            }
            return d;
        }
        public string GetDictionaryValue(string param_name, Dictionary<string, string> d)
        {
            string value = "";

            if (d != null)
            {
                return d.TryGetValue(param_name, out value) == true ? value : "";
            }
            else
            {
                return value;
            }
        }
    }
}