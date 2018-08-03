using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StockManagementOperation;

namespace StockManagement.Areas.Admin.Models
{

    public class ProductModel
    {
        public List<Categories> categories { get; set; }
        public List<Storage> storage { get; set; }
        public List<Cargo> cargo { get; set; }
        public Products product { get; set; }
    }
}