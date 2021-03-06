﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace LabTechnicalTest.API
{
    public class APIHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            //This is included for public APIs - it's removed if we specifically want to disallow cross-domain requests for any reason
            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

            // /api/methodpath?parameter1=value
            string path = context.Request.RawUrl;
            // /api/methodpath
            string absPath = context.Request.Url.AbsolutePath;

            #region Help
            if (absPath.Equals("/api/", StringComparison.OrdinalIgnoreCase))
            {
                //The purpose of this is to document the functionality of the API.
                //Anyone who needs to use the API can be pointed at the /api/ endpoint (which can have aliases for /api/help, for example)
                //This output can then detail all methods available, in an HTML format, that can be explored in a web browser mm 
                HelpText(context);
                return;
            }
            #endregion

            #region PostFile
            if ((absPath.Equals("/api/postfile", StringComparison.OrdinalIgnoreCase))
               || (absPath.Equals("/api/postfile/", StringComparison.OrdinalIgnoreCase)))
            {
                LabTechnicalTest.API.Logic.PostFile.Run(context);
                return;
            }
            #endregion


            //error message if path does not map to any known method
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.Write("Request path not recognised.");
        }

        public void HelpText(HttpContext context)
        {
            context.Response.ContentType = "text/html";

            StringBuilder sb = new StringBuilder();

            sb.Append("Call any method without any parameters to get help text, if relevant.<br /><br />");
            sb.Append("<a href=\"/api/postfile\">/api/postfile</a> : Post a CSV file.<br />");
            context.Response.Write(sb.ToString());
            return;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}