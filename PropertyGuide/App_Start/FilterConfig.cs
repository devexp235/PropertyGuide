﻿using System.Web;
using System.Web.Mvc;

namespace PropertyGuide
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new AuthorizeAttribute());//make the whole application accessible only by the authenticated users
            filters.Add(new HandleErrorAttribute());
        }
    }
}
