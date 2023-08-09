using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinalProject.Models;

namespace FinalProject.Controllers
{
    public class MonitorController : Controller
    {
        private DBEcommerceWebEntities db = new DBEcommerceWebEntities();
        // GET: Monitor
        public ActionResult AOC()
        {
            var products = db.Products;
            return View(products);
        }

        public ActionResult Asus()
        {
            var products = db.Products;
            return View(products);
        }
        public ActionResult Dell()
        {
            var products = db.Products.Include(p => p.Category);
            return View(products);
        }
        public ActionResult HP()
        {
            var products = db.Products;
            return View(products);
        }
        public ActionResult Samsung()
        {
            var products = db.Products;
            return View(products);
        }
        public ActionResult Huawei()
        {
            var products = db.Products;
            return View(products);
        }
    }
}