using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinalProject.Models;

namespace FinalProject.Controllers
{
    public class SmartTVController : Controller
    {
        private DBEcommerceWebEntities db = new DBEcommerceWebEntities();
        // GET: SmartTV
        public ActionResult TVCasper()
        {
            var products = db.Products;
            return View(products);
        }
        public ActionResult TVSamsung()
        {
            var products = db.Products;
            return View(products);
        }
        public ActionResult TVXiaomi()
        {
            var products = db.Products;
            return View(products);
        }
    }
}