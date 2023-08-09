using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinalProject.Models;

namespace FinalProject.Controllers
{
    public class CustomersController : Controller
    {
        private DBEcommerceWebEntities db = new DBEcommerceWebEntities();

        // GET: Customers
        public ActionResult Index()
        {
            return View(db.Customers.ToList());
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDCus,NameCus,PhoneCus,EmailCus,AddressDelivery,UserName,Password,ConfirmPassword")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xác nhận mật khẩu
                if (customer.Password != customer.ConfirmPassword)
                {
                    ViewBag.ErrorInfo = "Xác nhận mật khẩu không khớp.";
                    return View(customer);
                }

                db.Customers.Add(customer);
                db.SaveChanges();
                TempData["IsRegistered"] = true;
                return RedirectToAction("LoginCus", "Customers");
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDCus,NameCus,PhoneCus,EmailCus,AddressDelivery,UserName,Password")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        // Tạo view cho khách hàng Login
        public ActionResult LoginCus()
        {
            return View();
        }
        // Xử lý tìm kiếm UserName, password trong Customer và thông báo
        [HttpPost]
        public ActionResult LoginAccountCus(Login login)
        {
            if (ModelState.IsValid)
            {
                // Xác thực cho Admin
                var checkAdmin = db.AdminUsers
                    .Where(s => s.NameUser == login.UserName && s.PasswordUser == login.Password)
                    .FirstOrDefault();

                if (checkAdmin != null) // Đăng nhập là Admin
                {
                    db.Configuration.ValidateOnSaveEnabled = false;
                    Session["ID"] = checkAdmin.ID;
                    Session["NameUser"] = checkAdmin.NameUser;
                    Session["PasswordUser"] = checkAdmin.PasswordUser;
                    Session["chon"] = checkAdmin.RoleUser;

                    string chon = checkAdmin.RoleUser;
                    // Xử lý chuyển hướng dựa vào vai trò của người dùng (Admin)
                    if (chon == "Sanpham")
                        return RedirectToAction("Index", "Products");
                    else if (chon == "Khachhang")
                        return RedirectToAction("Index", "Customers");
                    else if (chon == "Donhang")
                        return RedirectToAction("Index", "OrderProes");
                    else if (chon.ToString() == "Admin")
                        return RedirectToAction("RegisterUser", "LoginUser");
                    else
                        return RedirectToAction("Index", "Home");
                }
                else // Đăng nhập là Customer
                {
                    // Check là khách hàng cần tìm
                    var checkCustomer = db.Customers
                        .Where(s => s.UserName == login.UserName && s.Password == login.Password)
                        .FirstOrDefault();

                    if (checkCustomer != null) // Đăng nhập là Customer
                    {
                        db.Configuration.ValidateOnSaveEnabled = false;
                        Session["IDCus"] = checkCustomer.IDCus;
                        Session["UserName"] = checkCustomer.UserName;
                        Session["Password"] = checkCustomer.Password;
                        Session["AddressDelivery"] = checkCustomer.AddressDelivery;
                        Session["NameCus"] = checkCustomer.NameCus;
                        Session["PhoneCus"] = checkCustomer.PhoneCus;
                        // Quay lại trang giỏ hàng với thông tin cần thiết
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            ViewBag.ErrorInfo = "Sai thông tin đăng nhập";
            return View("LoginCus", new Login());
        }
    }
}
