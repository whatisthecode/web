using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models.Mapping;

namespace WebApplication2.Services
{
    public static class Utils
    {
        public static Response checkInput(Object input, params String[] requiredFields)
        {
            String missingFields = "";
            Boolean havingRequiredFields = true;
            JObject jInput = JObject.FromObject(input);
            foreach(var rf in requiredFields)
            {
                if(jInput.SelectToken(rf) == null || jInput.SelectToken(rf).Type == JTokenType.Null ||
                    ((jInput.SelectToken(rf).Type == JTokenType.Array || jInput.SelectToken(rf).Type == JTokenType.Object) && !jInput.SelectToken(rf).HasValues) ||
                    (jInput.SelectToken(rf).Type == JTokenType.String && jInput.SelectToken(rf).ToString() == String.Empty))
                {
                    havingRequiredFields = false;
                    missingFields += rf + " ";
                }
            }
            if (havingRequiredFields)
            {
                return new Response();
            }
            else
            {
                Response check = new Response();
                check.code = "422";
                check.status = "Missing fields : " + missingFields + "or missing value of fields : " + missingFields.TrimEnd();
                return check;
            }
        }
    }
}