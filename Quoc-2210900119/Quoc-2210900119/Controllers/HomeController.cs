using Quoc_2210900119.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Quoc_2210900119.Controllers
{
    public class HomeController : Controller
    {
        private CHBHTH2Entities1 db = new CHBHTH2Entities1();

        public ActionResult Index()
        {
            var products = db.SanPhams.ToList();

            // Nếu products là null hoặc không có sản phẩm, trả về một danh sách trống để tránh lỗi
            if (products == null || !products.Any())
            {
                products = new List<SanPham>();
            }

            return View(products);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}