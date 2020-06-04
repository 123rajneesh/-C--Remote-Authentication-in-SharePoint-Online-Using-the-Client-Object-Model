using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;
using System.Net;
using MSDN.Samples.ClaimsAuth;
using System.Windows.Forms;

namespace Sp_Ctx
{
    class Program
    {
        public const string rtFa_AUTH_COOKIE_NAME = "rtFa";
        [STAThread]
        static void Main(string[] args)
        {   
       
            try
            {
                Console.WriteLine("Enter Sharepoint domain url");
                string targetSiteUrl = Console.ReadLine();
                if (string.IsNullOrEmpty(targetSiteUrl)) { Console.WriteLine("SP_Ctx <url>"); return; }

                //if (args.Length < 1) { Console.WriteLine("SP_Ctx <url>"); return; }
                //string targetSite = args[0];

                string targetSite = targetSiteUrl;
                using (ClientContext ctx = ClaimClientContext.GetAuthenticatedContext(targetSite))
                {
                    if (ctx != null)
                    {
                        ctx.Load(ctx.Web); // Query for Web
                        ctx.ExecuteQuery(); // Execute
                        Console.WriteLine(ctx.Web.Title);
                    }
                }
                Console.ReadLine();
            }
            catch (Exception e)
            {
                throw e;
            } 
            //var rtfaCookie = ExtractRTFACookies();
        }

        static Cookie ExtractRTFACookies()
        {
            string stringCookie = "rtFa=; rtFa=jNKl2H90vy37V9mjZac3Q90IddnRfDjEwBb0Bh27NWImQkI0NjZDQkItNjIxQy00OTNCLTgzNzQtNkM4RjI2REM2QTE4JqqOQonOvSxSyY3POKgoCZirXZLgbMt7i8BwULwZRx4HFWUb9On2fP1UySa8xDDFJu4c1f0yT9HN9WJzXXqMH4eSCBZGnLNKTiRt5WtIY/LD1f4SnY/6WGLZgnugjuur+lCstErpaPQ5eNcA7HsJ2NydSlkCWdb6/mi/UDtxVlwJlg8rkngKyXz6FqQ7GC/dN4iYvI9Txp5WG2PprTlAtKIAO2L4IlSqpnzuooA+TEOdFwInj/cJBhy+rQdl6WU0wmKcxHQbIzvWiXSzBGW0GA0VtUmNsyzDVc5IirsJ0IOdZ6fCxtana4/cA+K5xIDYxOQCniVrMR3/H3ZovXo4z0UAAAA=";

            if (string.IsNullOrEmpty(stringCookie)) return null;

            string cookieValue = stringCookie.Substring(rtFa_AUTH_COOKIE_NAME.Length + 1); // remove one character to accomodate for equals sign

            // ";" is illegal in a cookie value - it is generally used as a value separator.
            // We should never see this, but we sometimes see it followed by a string of the form "rtFa=" which may then be followed by the complete cookie value (again).
            // So we now strip any semicolon and data that may follow.
            int sepIndex = cookieValue.IndexOf(';');
            if (sepIndex > -1)
            {
                //cookieValue = cookieValue.Substring(0, sepIndex).TrimEnd();
                cookieValue = cookieValue.Replace(";", "").Replace("rtFa=", "").Trim();
            }

            return new Cookie(rtFa_AUTH_COOKIE_NAME, cookieValue);
        }


    }
}
