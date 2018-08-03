using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockManagementOperation
{
    public class CategoryManagement
    {
        StockEntities db;

        public CategoryManagement()
        {
            db = new StockEntities();
        }
        public List<Categories> GetAllCategory()
        {

            return (from i in db.Categories where i.IsActive.Value orderby i.OrderNumber select i).ToList();
        }
        public string addCategory(string name, int ordernumber)
        {
            var categorikontrol = (from i in db.Categories where i.CategoryName == name select i).Count();
            if (categorikontrol > 0)
            {
                return "Boyle Bir Categori İsmi Var";
            }
            else
            {
                Categories eklenekcategori = new Categories();
                eklenekcategori.CategoryName = name;
                eklenekcategori.OrderNumber = (byte)ordernumber;
                eklenekcategori.IsActive = true;
                db.Categories.Add(eklenekcategori);
                db.SaveChanges();
                return "Basarıyla eklendi";
            }
        }

        public string UpdateCategory(int categoryıd, string categoriname, int order)
        {
            var deişecekveri = (from i in db.Categories where i.CategoryID == categoryıd select i).SingleOrDefault();
            deişecekveri.CategoryName = categoriname;
            deişecekveri.OrderNumber = (byte)order;
            db.SaveChanges();
            return "Categori basarı ile degistirildi";
        }

        public string getCategoriNameById(int categoryıd)
        {
            var deişecekveri = (from i in db.Categories where i.CategoryID == categoryıd select i).SingleOrDefault();
            return deişecekveri.CategoryName;
        }

    }
}
