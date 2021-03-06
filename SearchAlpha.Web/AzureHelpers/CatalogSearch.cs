﻿using System;
using System.Configuration;
using System.Globalization;
using System.Net.Http;

namespace SearchAlpha.Web.AzureHelpers
{
    public class CatalogSearch
    {
        private static readonly Uri _serviceUri;
        private static HttpClient _httpClient;
        public static string errorMessage;

        // sample search
        //https://alpha.search.windows.net/indexes/organisations/docs?api-version=2014-07-31-Preview&search=Leeds

        static CatalogSearch()
        {
            try
            {
                _serviceUri = new Uri("https://" + ConfigurationManager.AppSettings["SearchServiceName"] + ".search.windows.net");
                _httpClient = new HttpClient();
                _httpClient.DefaultRequestHeaders.Add("api-key", ConfigurationManager.AppSettings["SearchServiceApiKey"]);
            }
            catch (Exception e)
            {
                errorMessage = e.Message.ToString();
            }
        }

        public dynamic Search(string searchText, string organisationType, string sort)
        {
            string search = "&search=" + Uri.EscapeDataString(searchText);
            string facets = "&facet=organisationType";
            //string paging = "&$top=10";
            string filter = BuildFilter(organisationType);
            string orderby = BuildSort(sort);

            var uri = new Uri(_serviceUri, "/indexes/organisations/docs?$count=true" + search + facets + filter + orderby);
            HttpResponseMessage response = AzureSearchHelper.SendSearchRequest(_httpClient, HttpMethod.Get, uri);
            AzureSearchHelper.EnsureSuccessfulSearchResponse(response);

            return AzureSearchHelper.DeserializeJson<dynamic>(response.Content.ReadAsStringAsync().Result);
        }

        public dynamic Suggest(string searchText)
        {
            // we still need a default filter to exclude discontinued products from the suggestions
            var uri = new Uri(_serviceUri, "/indexes/organisations/docs/suggest?search=" + Uri.EscapeDataString(searchText));
            HttpResponseMessage response = AzureSearchHelper.SendSearchRequest(_httpClient, HttpMethod.Get, uri);
            AzureSearchHelper.EnsureSuccessfulSearchResponse(response);

            return AzureSearchHelper.DeserializeJson<dynamic>(response.Content.ReadAsStringAsync().Result);
        }

        private string BuildSort(string sort)
        {
            if (string.IsNullOrWhiteSpace(sort))
            {
                return string.Empty;
            }

            // could also add asc/desc if we want to allow both sorting directions
            if (sort == "listPrice" || sort == "organisationType")
            {
                return "&$orderby=" + sort;
            }

            throw new Exception("Invalid sort order");
        }

        private string BuildFilter(string organisationType)
        {
            // carefully escape and combine input for filters, injection attacks that are typical in SQL
            // also apply here. No "DROP TABLE" risk, but a well injected "or" can cause unwanted disclosure

            //string filter = "&$filter=discontinuedDate eq null";
            string filter = "";

            if (!string.IsNullOrWhiteSpace(organisationType))
            {
                filter += "&$filter=organisationType eq '" + EscapeODataString(organisationType) + "'";
            }

            //if (!string.IsNullOrWhiteSpace(category))
            //{
            //    filter += " and categoryName eq '" + EscapeODataString(category) + "'";
            //}

            //if (priceFrom.HasValue)
            //{
            //    filter += " and listPrice ge " + priceFrom.Value.ToString(CultureInfo.InvariantCulture);
            //}

            //if (priceTo.HasValue && priceTo > 0)
            //{
            //    filter += " and listPrice le " + priceTo.Value.ToString(CultureInfo.InvariantCulture);
            //}

            return filter;
        }

        private string EscapeODataString(string s)
        {
            return Uri.EscapeDataString(s).Replace("\'", "\'\'");
        }
    }
}
