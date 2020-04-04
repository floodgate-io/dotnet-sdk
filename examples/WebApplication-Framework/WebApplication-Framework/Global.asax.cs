﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using WebApplication_Framework.Services;

namespace WebApplication_Framework
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
        }

        protected void Application_End(object sender, EventArgs e)
        {
            FloodGateWrapper floodgate = FloodGateWrapper.Instance;

            floodgate.Client.Dispose();
        }
    }
}