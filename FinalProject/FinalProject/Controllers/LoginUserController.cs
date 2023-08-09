using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using FinalProject.Models;


namespace FinalProject.Controllers
{
    public class LoginUserController : Controller
    {
        DBEcommerceWebEntities db = new DBEcommerceWebEntities();
        // GET: LoginUser
        // Phương thức tạo view cho Login
        // Phương thức tạo view cho Login
        public ActionResult Index()
        {
            return View();
        }
        // Xử lý tìm kiếm ID, password trong AdminUser và thông báo
        [HttpPost]
        public ActionResult LoginAccount(AdminUser _user)
        {
            var check = db.AdminUsers.
                Where(s => s.NameUser == _user.NameUser && s.PasswordUser == _user.PasswordUser).FirstOrDefault();
            if (check == null)
            {
                ViewBag.ErrorInfo = "Sai Info";
                return View("Index");
            }
            else
            {
                db.Configuration.ValidateOnSaveEnabled = false;
                Session["ID"] = _user.ID;
                Session["NameUser"] = _user.NameUser;
                Session["PasswordUser"] = _user.PasswordUser;
                Session["RoleUser"] = _user.RoleUser;

                Session["chon"] = _user.RoleUser;
                string chon = _user.RoleUser;
                //var ch = Session["chon"];
                //return RedirectToAction("ProductList", "Products");
                if (chon.ToString() == "Sanpham")
                    return RedirectToAction("Index", "Products");
                else if (chon.ToString() == "Khachhang")
                    return RedirectToAction("Index", "Customers");
                else if (chon.ToString() == "Donhang")
                    return RedirectToAction("Index", "OrderProes");
                else if (chon.ToString() == "Admin")
                    return RedirectToAction("RegisterUser", "LoginUser");
                else
                    return RedirectToAction("Index", "Home");
            }
        }
        // Register
        [HttpGet]
        public ActionResult RegisterUser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RegisterUser(AdminUser _user)
        {
            if (ModelState.IsValid)
            {
                db.Configuration.ValidateOnSaveEnabled = false;
                db.AdminUsers.Add(_user);
                db.SaveChanges();
            }
            else
            {
                ViewBag.ErrorRegister = "ID này đã có rồi !!!";
            }
            return View();
        }

        [ChildActionOnly]
        public PartialViewResult menuPartial()
        {
            return PartialView();
        }

        public ActionResult LogOutUser()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }
}