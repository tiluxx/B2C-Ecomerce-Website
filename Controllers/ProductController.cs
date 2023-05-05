﻿using B2C_Ecomerce_Website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace B2C_Ecomerce_Website.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            Product product = new Product();
            List<Product> res = product.SelectProductQuery();
            ViewBag.ProductList = res;
            return View();
        }

        // GET: Product/Cart
        public ActionResult Cart()
        {
            if (Session["CurrCartList"] != null)
            {
                List<Product> productList = (List<Product>)Session["CurrCartList"];
                ViewBag.ProductList = productList;
            }

            if (Session["CustomerID"] != null)
            {
                Cart cart = new Cart();
                CartDetail customerCartDetail = new CartDetail();
                Cart customerCart = cart.SelectCustomerCartQuery(Session["CustomerID"].ToString());
                if (customerCart.CartID != null)
                {
                    List<Product> productList = customerCartDetail.SelectProductCartQuery(customerCart.CartID);
                    ViewBag.ProductList = productList;
                }
            }
            return View();
        }

        // GET: Product/OrderPreview
        public ActionResult OrderPreview()
        {
            List<Product> productList = new List<Product>();
            int formCount = Request.Form.Count / 8;
            decimal totalBill = 0;

            for (int i = 0; i < formCount; i++)
            {
                int productQuantity = Convert.ToInt32(Request.Form["ProductQuantity[" + i + "]"]);
                decimal productPrice = Convert.ToDecimal(Request.Form["ProductPrice[" + i + "]"]);
                totalBill += productPrice * productQuantity;

                Product product = new Product
                {
                    ProductID = Request.Form["ProductID[" + i + "]"],
                    ProductName = Request.Form["ProductName[" + i + "]"],
                    ProductSize = Request.Form["ProductSize[" + i + "]"],
                    ProductUnitSize = Request.Form["ProductUnitSize[" + i + "]"],
                    ProductBrand = Request.Form["ProductBrand[" + i + "]"],
                    ProductOrigin = Request.Form["ProductOrigin[" + i + "]"],
                    ProductQuantity = Convert.ToInt32(Request.Form["ProductAvailableQuantity[" + i + "]"]),
                    ProductOrderQuantity = productQuantity,
                    ProductPrice = productPrice
                };
                productList.Add(product);
            }

            ViewBag.ProductList = productList;
            ViewBag.TotalBill = totalBill;
            return View();
        }

        public ActionResult AddToCart(Product product)
        {
            // Login case
            if (Session["CustomerID"] != null)
            {
                string agentId = Session["AgentID"].ToString();
                Cart cart = new Cart();
                Cart customerCart = cart.SelectCustomerCartQuery(agentId);
                if (customerCart == null || customerCart.CartID == null)
                {
                    string newCartId = cart.GetNewCartID();
                    cart.AddCartQuery(newCartId, agentId);

                    CartDetail agentCartDetail = new CartDetail();
                    agentCartDetail.AddCartDetailQuery(newCartId, product.ProductID);
                    List<Product> cartList = agentCartDetail.SelectProductCartQuery(newCartId);

                    Session["CurrCartList"] = cartList;
                    Session["CartQuan"] = cartList.Count();
                }
                else
                {
                    CartDetail cartDetail = new CartDetail();
                    if (cartDetail.CheckProductExist(customerCart.CartID, product.ProductID))
                    {
                        ViewBag.AddCartMessage = "You have add this product already";
                        return RedirectToAction("Index", "Product");
                    }
                    cartDetail.AddCartDetailQuery(customerCart.CartID, product.ProductID);
                    List<Product> cartList = cartDetail.SelectProductCartQuery(customerCart.CartID);

                    Session["CurrCartList"] = cartList;
                    Session["CartQuan"] = cartList.Count();
                }
                return RedirectToAction("Index", "Home");

            }

            // Not login case
            if (Session["CurrCartList"] == null)
            {
                List<Product> cartList = new List<Product>();
                cartList.Add(product);
                Session["CurrCartList"] = cartList;
                Session["CartQuan"] = 1;
            }
            else
            {
                List<Product> cartList = (List<Product>)Session["CurrCartList"];
                if (cartList.Contains(product))
                {
                    ViewBag.AddCartMessage = "You have add this product already";
                    return RedirectToAction("Index", "Home");
                }
                cartList.Add(product);
                Session["CurrCartList"] = cartList;
                Session["CartQuan"] = Convert.ToInt32(Session["CartQuan"].ToString()) + 1;
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult RemoveItemFromCart(Product product)
        {
            // Delete product out of current cart
            List<Product> cartList = (List<Product>)Session["CurrCartList"];
            cartList.RemoveAll(cartItem => cartItem.ProductID == product.ProductID);

            // Update the table
            if (Session["AgentID"] != null)
            {
                Cart cart = new Cart();
                Cart customerCart = cart.SelectCustomerCartQuery(Session["AgentID"].ToString());
                CartDetail customerCartDetail = new CartDetail();
                customerCartDetail.DeleteProductCartQuery(customerCart.CartID, product.ProductID);
            }

            // Return to view
            Session["CurrCartList"] = cartList;
            Session["CartQuan"] = Convert.ToInt32(Session["CartQuan"].ToString()) - 1;
            return RedirectToAction("Cart", "Product");
        }
    }
}