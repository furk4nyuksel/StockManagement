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
    
    public partial class Company
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Company()
        {
            this.CompanyOrderPdf = new HashSet<CompanyOrderPdf>();
            this.DeletedCompany = new HashSet<DeletedCompany>();
            this.Products = new HashSet<Products>();
            this.Storage = new HashSet<Storage>();
        }
    
        public int CompanyID { get; set; }
        public Nullable<int> UserID { get; set; }
        public string CompanyName { get; set; }
        public string BankAccountNo { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public byte[] CompanyPhoto { get; set; }
    
        public virtual AppUsers AppUsers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompanyOrderPdf> CompanyOrderPdf { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeletedCompany> DeletedCompany { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Products> Products { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Storage> Storage { get; set; }
    }
}
