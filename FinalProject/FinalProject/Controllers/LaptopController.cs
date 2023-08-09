using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinalProject.Models;

namespace FinalProject.Controllers
{
    public class LaptopController : Controller
    {
        private DBEcommerceWebEntities db = new DBEcommerceWebEntities();
        // GET: Laptop
        public ActionResult Asus()
        {
            var products = db.Products;
            return View(products);
        }
        public ActionResult Acer()
        {
            var products = db.Products;
            return View(products);
        }
        public ActionResult Apple()
        {
            var products = db.Products;
            return View(products);
        }
        public ActionResult Gigabyte()
        {
            var products = db.Products;
            return View(products);
        }
        public ActionResult Dell()
        {
            var products = db.Products;
            return View(products);
        }
        public ActionResult MSI()
        {
            var products = db.Products;
            return View(products);
        }
    }
}