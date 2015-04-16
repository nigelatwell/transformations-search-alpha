using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchAlpha.Web.AzureHelpers
{
    public static class OrganisationTypeMapper
    {
        public static string GetOrganisationTypeForPIMSUrl(string organisationType)
        {
            string result = "dentists";

            switch (organisationType.ToLower())
            {
                case "dentists":
                    result = "dentists";
                    break;
                case "care homes and care at home":
                    result = "careproviders";
                    break;
                case "pharmacy":
                    result = "pharmacies";
                    break;
                case "clinic":
                    result = "clinics";
                    break;
                case "opticians":
                    result = "opticians";
                    break;
                case "hospital":
                    result = "hospitals";
                    break;
                case "gp":
                    result = "gp";
                    break;
                default:
                    result = "dentists";
                    break;
            }

            return result;
        }
    }
}
