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
    
    public partial class ProductPhotograph
    {
        public int PhotoID { get; set; }
        public Nullable<int> ProductID { get; set; }
        public string PhotoSource { get; set; }
    
        public virtual Products Products { get; set; }
    }
}
