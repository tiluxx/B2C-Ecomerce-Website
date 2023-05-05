//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace B2C_Ecomerce_Website.Models
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Globalization;

    public partial class DeliveryCustomerReceipt
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DeliveryCustomerReceipt()
        {
            this.DeliveryCustomerReceiptDetails = new HashSet<DeliveryCustomerReceiptDetail>();
        }
    
        public string DeliveryCustomerReceiptID { get; set; }
        public string CartID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAddress { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentData { get; set; }
        public Nullable<System.Int64> PaymentTransactionNo { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<decimal> TotalBill { get; set; }
    
        public virtual Cart Cart { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryCustomerReceiptDetail> DeliveryCustomerReceiptDetails { get; set; }

        public string GetPaymentDate
        {
            get
            {
                return PaymentDate.HasValue ? PaymentDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "Not Paid";
            }
        }

        public Int64 GetPaymentTransactionNo
        {
            get
            {
                return PaymentTransactionNo.HasValue ? PaymentTransactionNo.Value : 0;
            }
        }

        public List<DeliveryCustomerReceipt> SelectReceiptQuery(string cartId)
        {

            List<DeliveryCustomerReceipt> res = new List<DeliveryCustomerReceipt>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConn"].ToString()))
            {
                conn.Open();
                string sql = "select * from DeliveryCustomerReceipt where CartID = '" + cartId + "'";
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    DeliveryCustomerReceipt receipt = new DeliveryCustomerReceipt();
                    receipt.DeliveryCustomerReceiptID = dr["DeliveryCustomerReceiptID"].ToString();
                    receipt.CartID = dr["CartID"].ToString();
                    receipt.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                    receipt.CustomerName = dr["CustomerName"].ToString();
                    receipt.CustomerPhone = dr["CustomerPhone"].ToString();
                    receipt.CustomerAddress = dr["CustomerAddress"].ToString();
                    receipt.PaymentMethod = dr["OrderStatus"].ToString();
                    if (!(dr["PaymentDate"] is DBNull))
                    {
                        receipt.PaymentDate = Convert.ToDateTime(dr["PaymentDate"]);
                    }
                    if (!(dr["PaymentTransactionNo"] is DBNull))
                    {
                        receipt.PaymentTransactionNo = Convert.ToInt64(dr["PaymentTransactionNo"]);
                    }
                    res.Add(receipt);
                }
                conn.Close();
            }
            return res;
        }

        public List<DeliveryCustomerReceipt> SelectReceiptByIDQuery(string receiptId)
        {

            List<DeliveryCustomerReceipt> res = new List<DeliveryCustomerReceipt>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConn"].ToString()))
            {
                conn.Open();
                string sql = "select * from DeliveryCustomerReceipt where DeliveryCustomerReceiptID = '" + receiptId + "'";
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    DeliveryCustomerReceipt receipt = new DeliveryCustomerReceipt();
                    receipt.DeliveryCustomerReceiptID = dr["DeliveryCustomerReceiptID"].ToString();
                    receipt.CartID = dr["CartID"].ToString();
                    receipt.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                    receipt.CustomerName = dr["CustomerName"].ToString();
                    receipt.CustomerPhone = dr["CustomerPhone"].ToString();
                    receipt.CustomerAddress = dr["CustomerAddress"].ToString();
                    receipt.PaymentMethod = dr["OrderStatus"].ToString();
                    if (!(dr["PaymentDate"] is DBNull))
                    {
                        receipt.PaymentDate = Convert.ToDateTime(dr["PaymentDate"]);
                    }
                    if (!(dr["PaymentTransactionNo"] is DBNull))
                    {
                        receipt.PaymentTransactionNo = Convert.ToInt64(dr["PaymentTransactionNo"]);
                    }
                    res.Add(receipt);
                }
                conn.Close();
            }
            return res;
        }

        private string GetReceiptDesc()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConn"].ToString()))
            {
                conn.Open();
                string sql = "select top 1 DeliveryCustomerReceiptID from DeliveryCustomerReceipt order by DeliveryCustomerReceiptID desc";
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();
                string res = "";
                while (dr.Read())
                {
                    res = dr["DeliveryCustomerReceiptID"].ToString();
                }
                conn.Close();
                return res;
            }
        }

        public string GetNewReceiptID()
        {
            string res = GetReceiptDesc();
            if (res != null && !res.Equals(""))
            {
                int order = int.Parse(res.Substring(4)) + 1;
                if (order < 10)
                {
                    res = "DRCD00000" + order.ToString();
                }
                else if (order < 100)
                {
                    res = "DRCD0000" + order.ToString();
                }
                else if (order < 1000)
                {
                    res = "DRCD000" + order.ToString();
                }
                else if (order < 10000)
                {
                    res = "DRCD00" + order.ToString();
                }
                else if (order < 100000)
                {
                    res = "DRCD0" + order.ToString();
                }
                else
                {
                    res = "DRCD" + order.ToString();
                }
                return res;
            }
            else
            {
                return "DRCD000001";
            }
        }

        public void AddReceiptQuery(
            string DeliveryCustomerReceiptID,
            string CustomerName,
            string CustomerPhone,
            string CustomerAddress,
            string PaymentMethod,
            decimal TotalBill,
            DateTime CreatedDate,
            string CartID)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConn"].ToString()))
            {
                conn.Open();
                string orderDate = CreatedDate.ToString("yyyy-MM-dd HH:mm:ss");
                string sql = "insert into DeliveryCustomerReceipt" +
                    " (DeliveryCustomerReceiptID, CartID, CreatedDate, CustomerName, CustomerPhone, CustomerAddress, PaymentMethod, TotalBill)" +
                    " values('" + DeliveryCustomerReceiptID + "', '" + CartID + "', '" + orderDate + "', '" + CustomerName + "', '" + CustomerPhone + "', '" + CustomerAddress + "', '" + PaymentMethod + "', " + TotalBill + ")";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void UpdateReceiptQuery(string receiptId, long transactionNo, long payDate)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConn"].ToString()))
            {
                DateTime parsedDate;
                DateTime.TryParseExact(payDate.ToString(), "yyyyMMddHHmmss", null, DateTimeStyles.None, out parsedDate);
                conn.Open();
                string sql = "update DeliveryCustomerReceipt" +
                    " set PaymentTransactionNo = " + transactionNo + ", PaymentDate = '" + parsedDate.ToString() + "'" +
                    " where OrderID = '" + receiptId + "'";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
    }
}
