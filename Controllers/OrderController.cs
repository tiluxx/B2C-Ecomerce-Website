using B2C_Ecomerce_Website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace B2C_Ecomerce_Website.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }

        // GET: Order/Result
        public ActionResult Result()
        {
            return View();
        }

        // GET: Order/ViewOrder
        public ActionResult ViewOrder()
        {
            return View("Index");
        }

        public ActionResult VnPaymentResponse(bool status, string message, long transactionNo = 0, long payDate = 0, string orderId = "")
        {
            if (status)
            {
                DeliveryCustomerReceipt receipt = new DeliveryCustomerReceipt();
                receipt.UpdateReceiptQuery(orderId, transactionNo, payDate);
            }
            ViewBag.Message = message;
            return View("Result");
        }

        [HttpPost]
        public ActionResult PlaceOrder()
        {
            DeliveryCustomerReceipt receipt = new DeliveryCustomerReceipt();
            string newReceiptID = receipt.GetNewReceiptID();
            string customerName = Request.Form["CustomerName"];
            string customerPhone = Request.Form["CustomerPhone"];
            string customerAddress = Request.Form["CustomerAddress"];
            string paymentMethod = Request.Form["PaymentMethod"];
            decimal totalBill = Convert.ToInt64(Request.Form["TotalBill"]);
            DateTime createdDate = DateTime.Now;
            string email = Request.Form["EmailInput"];
            receipt.AddReceiptQuery(newReceiptID, customerName, customerPhone, customerAddress, paymentMethod, totalBill, createdDate);

            // Get the acutal number of groups of product's information
            int formCount = Request.Form.Count / 8;
            for (int i = 0; i < formCount; i++)
            {
                string productID = Request.Form["ProductID[" + i + "]"];
                int productQuantity = Convert.ToInt32(Request.Form["ProductQuantity[" + i + "]"]);
                int productAvailableQuantity = Convert.ToInt32(Request.Form["ProductAvailableQuantity[" + i + "]"]);
                int newProductQuantity = productAvailableQuantity - productQuantity;

                Product product = new Product();
                product.UpdateProductQuantity(productID, newProductQuantity);
                DeliveryCustomerReceiptDetail receiptDetail = new DeliveryCustomerReceiptDetail();
                receiptDetail.AddReceiptDetailQuery(newReceiptID, productID, productQuantity);
            }

            Session["CurrCartList"] = new List<Product>();
            Session["CartQuan"] = 0;

            // Send confirm message to email
            MailMessage mail = new MailMessage();
            mail.To.Add(email);
            mail.From = new MailAddress("lethanhtienhqv@gmail.com");
            mail.Subject = "Order Confirm Letter";
            string Body = "Dear customer,<br />Thank you for your consideration to choose our service. We are grateful to say that your order is placed successfully and in the way to process.This is your Receipt ID to view your order status: " + newReceiptID + "<br />Sincerely,<br /> Distributor";
            mail.Body = Body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential("lethanhtienhqv@gmail.com", "nhqpbctsxsoqfdxi"),
                EnableSsl = true
            };
            smtp.Send(mail);

            if (paymentMethod.Equals("VNPay"))
            {
                return RedirectToAction("CreatePayment", "Payment", new { amount = totalBill, orderId = newReceiptID });
            }

            ViewBag.Message = "Place order successfully";
            return View("Result");
        }

        [HttpPost]
        public ActionResult ViewOrderByOrderID()
        {
            DeliveryCustomerReceipt receipt = new DeliveryCustomerReceipt();
            List<DeliveryCustomerReceipt> receiptList = receipt.SelectReceiptByIDQuery(Request.Form["OrderID"]);
            ViewBag.OrderList = receiptList;
            return View("ViewOrder");
        }
    }
}