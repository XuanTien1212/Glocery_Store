using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.IO;
using System.Web.Mvc;
using FinalProject.Models;
using PagedList;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace FinalProject.Controllers
{
    public class ProductsController : Controller
    {
        private DBEcommerceWebEntities db = new DBEcommerceWebEntities();

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category).Include(p => p.ProductBrand);
            return View(products.ToList());
        }

        public ActionResult ProductList(int? page, int? categoryId, string SearchString, double min = double.MinValue, double max = double.MaxValue)
        {
            var products = db.Products.Include(p => p.Category);

            
            ViewBag.search = SearchString;
            ViewBag.min = min;
            ViewBag.max = max;
            //Tìm kiếm chuỗi truy vấn theo NamePro, nếu chuỗi truy vấn SearchString khác rỗng, null
            if (!System.String.IsNullOrEmpty(SearchString) && min >= 0 && max > 0)
            {
                products = db.Products.Where(s => s.NamePro.Contains(SearchString.Trim()) && (double)s.Price >= min && (double)s.Price <= max).OrderByDescending(x => x.Price);
            }
            else if (!System.String.IsNullOrEmpty(SearchString))
            {
                products = db.Products.Where(s => s.NamePro.Contains(SearchString.Trim())).OrderByDescending(x => x.Price);
            }
            else
                products = db.Products.Include(p => p.Category).OrderByDescending(x => x.Price);
            // Tìm kiếm chuỗi truy vấn theo đơn giá
            // Khai báo mỗi trang 4 sản phẩm
            int pageSize = 4;
            // Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
            // nếu page = null thì lấy giá trị 1 cho biến pageNumber.
            int pageNumber = (page ?? 1);

            // Nếu page = null thì đặt lại page là 1.
            if (page == null) page = 1;
            ViewBag.Categories = db.Categories.ToList();


            // Trả về các product được phân trang theo kích thước và số trang.
            return View(products.ToPagedList(pageNumber, pageSize));
        }

        // GET: Products/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        public ActionResult ProInfo(string id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CateID = new SelectList(db.Categories, "IDCate", "NameCate");
            ViewBag.BrandID = new SelectList(db.ProductBrands, "IDBrand", "IDBrand");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductNum,ProductID,NamePro,CateID,BrandID,Price,DescriptionPro,Quantity")] Product product, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                if (db.Products.Any(p => p.ProductID == product.ProductID))
                {
                    ModelState.AddModelError("", "Một sản phẩm với cùng mã ProductID đã tồn tại.");
                    ViewBag.CateID = new SelectList(db.Categories, "IDCate", "NameCate", product.CateID);
                    ViewBag.BrandID = new SelectList(db.ProductBrands, "IDBrand", "IDBrand", product.BrandID);
                    return View(product);
                }
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    // Lưu trữ hình ảnh trong thư mục cụ thể
                    string path = Server.MapPath("/Content/image/");
                    string fileName = Path.GetFileName(imageFile.FileName);
                    string fullPath = Path.Combine(path, fileName);
                    imageFile.SaveAs(fullPath);

                    // Lưu đường dẫn hình ảnh vào thuộc tính ImagePro của đối tượng Product
                    product.ImagePro = "/Content/image/" + fileName;
                }
                else
                {
                    ModelState.AddModelError("", "Vui lòng chọn hình ảnh");
                    ViewBag.CateID = new SelectList(db.Categories, "IDCate", "NameCate", product.CateID);
                    ViewBag.BrandID = new SelectList(db.ProductBrands, "IDBrand", "IDBrand", product.BrandID);
                    return View(product);
                }

                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CateID = new SelectList(db.Categories, "IDCate", "NameCate", product.CateID);
            ViewBag.BrandID = new SelectList(db.ProductBrands, "IDBrand", "IDBrand", product.BrandID);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CateID = new SelectList(db.Categories, "IDCate", "NameCate", product.CateID);
            ViewBag.BrandID = new SelectList(db.ProductBrands, "IDBrand", "IDBrand", product.BrandID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductNum,ProductID,NamePro,CateID,BrandID,Price,DescriptionPro,Quantity")] Product product, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    string imagePath = Path.Combine(Server.MapPath("/Content/image/"), Path.GetFileName(imageFile.FileName));
                    imageFile.SaveAs(imagePath);
                    product.ImagePro = "/Content/image/" + imageFile.FileName; // Đường dẫn hoàn chỉnh
                }

                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CateID = new SelectList(db.Categories, "IDCate", "NameCate", product.CateID);
            ViewBag.BrandID = new SelectList(db.ProductBrands, "IDBrand", "IDBrand", product.BrandID);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
    }
}
