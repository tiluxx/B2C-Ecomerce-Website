using B2C_Ecomerce_Website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace B2C_Ecomerce_Website.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authorize(UserAccount userAccountModel)
        {
            using (DistributorDBEntities db = new DistributorDBEntities())
            {
                var agentAccounts = db.UserAccounts.Where(staff => staff.UserName == userAccountModel.UserName && staff.UserPassword == userAccountModel.UserPassword).FirstOrDefault();
                if (agentAccounts == null)
                {
                    userAccountModel.LoginMessageError = "Invalid account";
                    return View("Index", userAccountModel);
                }
                else
                {
                    // Start session
                    CustomerAccount customerAccount = new CustomerAccount();
                    string customerId = customerAccount.GetCustomerID(userAccountModel.UserName);
                    Session["CustomerID"] = customerId;

                    // Get the current cart of agent
                    Cart cart = new Cart();
                    CartDetail agentCartDetail = new CartDetail();
                    Cart agentCart = cart.SelectCustomerCartQuery(customerId);

                    if (agentCart.CartID != null)
                    {
                        List<Product> productList = agentCartDetail.SelectProductCartQuery(agentCart.CartID);
                        Session["CurrCartList"] = productList;
                        Session["CartQuan"] = productList.Count();
                    }
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        public ActionResult Logout()
        {
            Session["CustomerID"] = null;
            Session["CurrCartList"] = null;
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }
}