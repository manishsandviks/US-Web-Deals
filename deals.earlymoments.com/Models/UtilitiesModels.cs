using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using System.IO;
namespace deals.earlymoments.com.Models
{
    public class UtilitiesModels
    {
        public class ChildGenderNameList
        {
            public string Code { get; set; }
            public string Name { get; set; }
        }
        public class StatesNameList
        {
            public string Code { get; set; }
            public string Name { get; set; }
        }

        public class MonthList
        {
            public int Value { get; set; }
            public string Name { get; set; }
        }

        public static SelectList GetChildGenderNameList()
        {
            try
            {
                List<SelectListItem> genderList = new List<SelectListItem>();

                SelectListItem li = new SelectListItem();
                li.Value = "1";
                li.Text = "Boy";
                genderList.Add(li);

                SelectListItem li2 = new SelectListItem();
                li2.Value = "2";
                li2.Text = "Girl";
                genderList.Add(li2);

                SelectListItem li3 = new SelectListItem();
                li3.Value = "0";
                li3.Text = "I don't know yet";
                genderList.Add(li3);

                return new SelectList(genderList, "Value", "Text");
            }
            catch
            {
                return null;
            }
        }

        public static SelectList GetStateNameList()
        {
            List<StatesNameList> states = new List<StatesNameList>();
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Assets\StateList.xml");
                XDocument xReader = XDocument.Load(path);
                StatesNameList state = new StatesNameList();
                List<StatesNameList> stateList = (from book in xReader.Element("states").Elements("state")
                                                  select new StatesNameList
                                                  {
                                                      Code = book.Element("value").Value,
                                                      Name = book.Element("name").Value
                                                  }).ToList();

                return new SelectList(stateList, "Code", "Name");
            }
            catch
            { return new SelectList(states, "Code", "Name"); }
        }

        public static SelectList GetYesNoList()
        {
            try
            {

                List<SelectListItem> YesNoList = new List<SelectListItem>();

                SelectListItem li = new SelectListItem();
                li.Value = "True";
                li.Text = "Yes";
                YesNoList.Add(li);

                SelectListItem li2 = new SelectListItem();
                li2.Value = "False";
                li2.Text = "No";
                YesNoList.Add(li2);

                return new SelectList(YesNoList, "Value", "Text");


            }
            catch
            {
                return null;
            }
        }

        //public static SelectList GetBirthYearList(int howMany = 18)
        //{
        //    try
        //    {

        //        List<SelectListItem> YrList = new List<SelectListItem>();

        //        int currentYear = DateTime.Now.Year;
        //        int firstYear = currentYear - howMany;
        //        for (int i = currentYear; i >= firstYear; i--)
        //        {
        //            SelectListItem li = new SelectListItem();
        //            li.Value = i.ToString();
        //            li.Text = i.ToString();
        //            YrList.Add(li);
        //        }
        //        return new SelectList(YrList, "Value", "Text", 0);
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        //public static SelectList GetBirthDayList()
        //{
        //    try
        //    {

        //        List<SelectListItem> YrList = new List<SelectListItem>();


        //        for (int i = 1; i <= 31; i++)
        //        {
        //            SelectListItem li = new SelectListItem();
        //            if (i < 10)
        //            {
        //                li.Value = "0" + i.ToString();
        //                li.Text = "0" + i.ToString();
        //            }
        //            else
        //            {
        //                li.Value = i.ToString();
        //                li.Text = i.ToString();
        //            }


        //            YrList.Add(li);
        //        }
        //        return new SelectList(YrList, "Value", "Text", 0);
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        public static SelectList GetCardExpiryYearList(int howMany = 10)
        {
            try
            {

                List<SelectListItem> YrList = new List<SelectListItem>();

                int currentYear = DateTime.Now.Year;
                int lastYear = currentYear + howMany;
                for (int i = currentYear; i < lastYear; i++)
                {
                    SelectListItem li = new SelectListItem();
                    li.Value = i.ToString();
                    li.Text = i.ToString();
                    YrList.Add(li);
                }
                return new SelectList(YrList, "Value", "Text", 0);
            }
            catch
            {
                return null;
            }
        }

        public static SelectList GetMonthNameList()
        {
            try
            {

                List<SelectListItem> monthList = new List<SelectListItem>();

                SelectListItem li = new SelectListItem();
                li.Value = "01";
                li.Text = "01";
                monthList.Add(li);

                SelectListItem li2 = new SelectListItem();
                li2.Value = "02";
                li2.Text = "02";
                monthList.Add(li2);

                SelectListItem li3 = new SelectListItem();
                li3.Value = "03";
                li3.Text = "03";
                monthList.Add(li3);

                SelectListItem li4 = new SelectListItem();
                li4.Value = "04";
                li4.Text = "04";
                monthList.Add(li4);

                SelectListItem li5 = new SelectListItem();
                li5.Value = "05";
                li5.Text = "05";
                monthList.Add(li5);

                SelectListItem li6 = new SelectListItem();
                li6.Value = "06";
                li6.Text = "06";
                monthList.Add(li6);

                SelectListItem li7 = new SelectListItem();
                li7.Value = "07";
                li7.Text = "07";
                monthList.Add(li7);

                SelectListItem li8 = new SelectListItem();
                li8.Value = "08";
                li8.Text = "08";
                monthList.Add(li8);

                SelectListItem li9 = new SelectListItem();
                li9.Value = "09";
                li9.Text = "09";
                monthList.Add(li9);

                SelectListItem li10 = new SelectListItem();
                li10.Value = "10";
                li10.Text = "10";
                monthList.Add(li10);

                SelectListItem li11 = new SelectListItem();
                li11.Value = "11";
                li11.Text = "11";
                monthList.Add(li11);

                SelectListItem li12 = new SelectListItem();
                li12.Value = "12";
                li12.Text = "12";
                monthList.Add(li12);

                return new SelectList(monthList, "Value", "Text");


            }
            catch
            {
                return null;
            }
        }
    }
}