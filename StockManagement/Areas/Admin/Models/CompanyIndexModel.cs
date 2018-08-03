using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StockManagementOperation;

namespace StockManagement.Areas.Admin.Models
{
    public class CompanyIndexModel
    {

        public double toplamgoruntulenme { get; set; }

        public double gunluksatıs { get; set; }
        public double aylıksatıs { get; set; }
        public double yıllıksatıs { get; set; }
        public double totalsatıs { get; set; }
        public List<Storage> depolar { get; set; }
    }
}