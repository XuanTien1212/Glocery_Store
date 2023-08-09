using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinalProject.Models;

namespace FinalProject.Controllers
{
    public class MBPhoneController : Controller
    {
        private DBEcommerceWebEntities db = new DBEcommerceWebEntities();
        // GET: MBPhone
        public ActionResult Samsung()
        {
            var products = db.Products;
            return View(products);
        }

        public ActionResult Oppo()
        {
            var products = db.Products;
            return View(products);
        }
        public ActionResult Apple()
        {
            var products = db.Products.Include(p => p.Category);
            return View(products);
        }
        public ActionResult Vivo()
        {
            var products = db.Products;
            return View(products);
        }
        public ActionResult Xiaomi()
        {
            var products = db.Products;
            return View(products);
        }
        public ActionResult Nokia()
        {
            var products = db.Products;
            return View(products);
        }
    }
}