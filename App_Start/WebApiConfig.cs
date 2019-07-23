using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Web.Http.Cors;

namespace ClientSide
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //enable cors
            config.EnableCors(new EnableCorsAttribute("http://localhost:4200", headers: "*", methods: "*"));
            //config.EnableCors(new EnableCorsAttribute("http://localhost", headers: "*", methods: "*"));
            //config.EnableCors(new EnableCorsAttribute("http://blrlw9050/QCAPIServer", headers: "*", methods: "*"));
            //config.EnableCors(new EnableCorsAttribute("http://blrlw9050", headers: "*", methods: "*"));
            //config.EnableCors(new EnableCorsAttribute("http://cso-scale-intv", headers: "*", methods: "*"));
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
