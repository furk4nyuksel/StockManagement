//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StockManagementOperation
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProductDetails
    {
        public int ProductDetailsID { get; set; }
        public string ProductDetailText { get; set; }
        public Nullable<int> ProductID { get; set; }
        public Nullable<int> CargoID { get; set; }
        public Nullable<double> ProductView { get; set; }
    
        public virtual Cargo Cargo { get; set; }
        public virtual Products Products { get; set; }
    }
}
