using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace NotesAPI.JwtTokenHelper
{
    public class GlobalConstants
    {
        public static string JwtTokenIssuer
        {
            get
            {
                return ConfigurationManager.AppSettings["JwtTokenIssuer"].ToString();
            }
        }        
    }
}