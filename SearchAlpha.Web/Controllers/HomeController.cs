using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SearchAlpha.Web.Controllers
{
    public class HomeController : Controller
    {
        private CatalogSearch _catalogSearch = new CatalogSearch();

        [HttpGet]
        public ActionResult Index()
        {
            //Check that the user entered in a valid service url and api-key
            if (CatalogSearch.errorMessage != null)
                ViewBag.errorMessage = "Please ensure that you have added your SearchServiceName and SearchServiceApiKey to the Web.config. Error: " + CatalogSearch.errorMessage;
            return View();
        }

        [HttpGet]
        public ActionResult Search(string q = "")
        {
            dynamic result = null;

            // If blank search, assume they want to search everything
            if (string.IsNullOrWhiteSpace(q))
                q = "*";

            result = _catalogSearch.Search(q);
            ViewBag.searchString = q;
            //ViewBag.color = color;
            //ViewBag.category = category;
            //ViewBag.priceFrom = priceFrom;
            //ViewBag.priceTo = priceTo;
            //ViewBag.sort = sort;

            return View("Index", result);
        }

        [HttpGet]
        public ActionResult Suggest(string term)
        {
            var options = new List<string>();
            if (term.Length >= 3)
            {
                var result = _catalogSearch.Suggest(term);

                foreach (var option in result.value)
                {
                    options.Add((string)option["@search.text"] + " (" + (string)option["productNumber"] + ")");
                }
            }

            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = options
            };
        }
    }
}


/*

 public static object IndexStructure()
    {
      return new[]
      {
        new
        {
          Name = "organisationID",
          Type = "Edm.String",
          Key = true,
          Searchable = false,
          Filterable = false,
          Sortable = false,
          Facetable = false,
          Retrievable = true,
          Suggestions = false
        },
        new
        {
          Name = "organisationName",
          Type = "Edm.String",
          Key = false,
          Searchable = true,
          Filterable = false,
          Sortable = true,
          Facetable = false,
          Retrievable = true,
          Suggestions = true
        },
        new
        {
          Name = "url",
          Type = "Edm.String",
          Key = false,
          Searchable = true,
          Filterable = false,
          Sortable = false,
          Facetable = false,
          Retrievable = true,
          Suggestions = false
        },
        new
        {
          Name = "address1",
          Type = "Edm.String",
          Key = false,
          Searchable = true,
          Filterable = false,
          Sortable = false,
          Facetable = false,
          Retrievable = true,
          Suggestions = false
        },
        new
        {
          Name = "address2",
          Type = "Edm.String",
          Key = false,
          Searchable = true,
          Filterable = false,
          Sortable = false,
          Facetable = false,
          Retrievable = true,
          Suggestions = false
        },
        new
        {
          Name = "address3",
          Type = "Edm.String",
          Key = false,
          Searchable = true,
          Filterable = false,
          Sortable = false,
          Facetable = false,
          Retrievable = true,
          Suggestions = false
        },
        new
        {
          Name = "city",
          Type = "Edm.String",
          Key = false,
          Searchable = true,
          Filterable = false,
          Sortable = false,
          Facetable = false,
          Retrievable = true,
          Suggestions = false
        },
        new
        {
          Name = "county",
          Type = "Edm.String",
          Key = false,
          Searchable = true,
          Filterable = false,
          Sortable = false,
          Facetable = false,
          Retrievable = true,
          Suggestions = false
        },
        new
        {
          Name = "postcode",
          Type = "Edm.String",
          Key = false,
          Searchable = true,
          Filterable = false,
          Sortable = false,
          Facetable = false,
          Retrievable = true,
          Suggestions = false
        },
         new
        {
          Name = "organisationType",
          Type = "Edm.String",
          Key = false,
          Searchable = true,
          Filterable = false,
          Sortable = false,
          Facetable = false,
          Retrievable = true,
          Suggestions = false
        },
        new
        {
          Name = "geocode",
          Type = "Edm.GeographyPoint",
          Key = false,
          Searchable = false,
          Filterable = true,
          Sortable = true,
          Facetable = false,
          Retrievable = true,
          Suggestions = false
        }

*/