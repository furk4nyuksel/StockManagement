using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StockManagementOperation;
namespace StockManagement.Models
{
    public class TotalOrderModelList
    {
        public Order Order { get; set; }
        public List<OrderDetails> OrderDetails { get; set; }
        public int GetQuantity { get; set; }
        public int GetOrderID { get; set; }
        public double GetTotalAmount { get; set; }
    }
}