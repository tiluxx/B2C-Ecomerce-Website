using B2C_Ecomerce_Website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace B2C_Ecomerce_Website.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Product product = new Product();
            List<Product> res = product.SelectProductQuery();
            ViewBag.ProductList = res;
            return View();
        }
    }
}