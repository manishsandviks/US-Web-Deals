using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using System.Collections.Specialized;
using System.ComponentModel;

namespace deals.earlymoments.com.Utilities
{

    public static class ViewContextExtensions
    {
        public static RouteValueDictionary GetCombinedRouteValues(this ViewContext viewContext, object newRouteValues)
        {
            RouteValueDictionary combinedRouteValues = new RouteValueDictionary(viewContext.RouteData.Values);
            NameValueCollection queryString = viewContext.RequestContext.HttpContext.Request.QueryString;

            foreach (string key in queryString.AllKeys.Where(key => key != null))
                combinedRouteValues[key] = queryString[key];

            if (newRouteValues != null)
            {
                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(newRouteValues))
                    combinedRouteValues[descriptor.Name] = descriptor.GetValue(newRouteValues);
            }
            return combinedRouteValues;
        }
    }

    public class PreserveQueryStringAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var redirectResult = filterContext.Result as RedirectToRouteResult;
            if (redirectResult == null)
            {
                return;
            }

            var query = HttpUtility.ParseQueryString(filterContext.HttpContext.Request.UrlReferrer.Query);
            foreach (string key in query.Keys)
            {
                if (!redirectResult.RouteValues.ContainsKey(key))
                {
                    redirectResult.RouteValues.Add(key, query[key]);
                }
            }
        }
    }

}