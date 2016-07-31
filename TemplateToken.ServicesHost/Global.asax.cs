using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using TemplateToken.Services.Config;

namespace TemplateToken.ServicesHost
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var config = GlobalConfiguration.Configuration;

            RouteConfig.RegisterRoutes(config);
            WebApiConfig.Configure(config);
            AutofacWebApi.Initialize(config);
        }
    }
}
