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
    
    public partial class C_Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public C_Order()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
        }
    
        public string OrderID { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public string AgentID { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentStatus { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public Nullable<decimal> OrderProductTotalBill { get; set; }
        public Nullable<bool> OrderDeleted { get; set; }
        public Nullable<long> PaymentTransactionNo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
