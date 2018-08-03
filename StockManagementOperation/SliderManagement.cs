using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockManagementOperation
{
    public class SliderManagement
    {
        StockEntities db;

        public SliderManagement()
        {
            db = new StockEntities();
        }

        public List<Slider> GetAllSlider()
        {
            return (from i in db.Slider select i).ToList();
        }

        public string addSlider(string slidername, string photourl, string sliderurl, int ordernum)
        {
            Slider eklenecekslider = new Slider();
            eklenecekslider.SliderName = slidername;
            eklenecekslider.PhotoUrl = photourl;
            eklenecekslider.SliderUrl = sliderurl;
            eklenecekslider.SliderOrderNumber = ordernum;
            db.Slider.Add(eklenecekslider);
            db.SaveChanges();
            return "Slider Eklendi";
        }

        public string deleteSlider(int id)
        {
            var silinecekslider = (from i in db.Slider where i.SliderID == id select i).SingleOrDefault();
            db.Slider.Remove(silinecekslider);
            db.SaveChanges();
            return "Slider silme işlemi basarılı.";
        }
    }
}
