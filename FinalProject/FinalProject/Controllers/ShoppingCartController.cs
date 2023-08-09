using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinalProject.Models;

namespace FinalProject.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart
        DBEcommerceWebEntities db = new DBEcommerceWebEntities();
        // GET: ShoppingCart, chuẩn bị dữ liệu cho View
        public ActionResult ShowCart()
        {
            if (Session["Cart"] == null)
                return View("ShowCart");
            Cart _cart = Session["Cart"] as Cart;
            return View(_cart);
        }
        // Tạo mới giỏ hàng, nguồn được lấy từ Session
        public Cart GetCart()   
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart == null || Session["Cart"] == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
        // Thêm sản phẩm vào giỏ hàng
        public ActionResult AddToCart(string id)
        {
            // lấy sản phẩm theo id
            var _pro = db.Products.SingleOrDefault(s => s.ProductID == id);
            if (_pro != null)
            {
                GetCart().Add_Product_Cart(_pro);
            }
            return RedirectToAction("ShowCart", "ShoppingCart");
        }
        // Cập nhật số lượng và tính lại tổng tiền
        public ActionResult Update_Cart_Quantity(FormCollection form)
        {
            Cart cart = Session["Cart"] as Cart;
            string id_pro = Request.Form["idPro"];
            int _quantity = int.Parse(Request.Form["cartQuantity"]);
            cart.Update_quantity(id_pro, _quantity);

            return RedirectToAction("ShowCart", "ShoppingCart");
        }
        // Xóa dòng sản phẩm trong giỏ hàng
        public ActionResult RemoveCart(string id)
        {
            Cart cart = Session["Cart"] as Cart;
            cart.Remove_CartItem(id);

            return RedirectToAction("ShowCart", "ShoppingCart");
        }
        // Tính tổng tiền đơn hàng
        public PartialViewResult BagCart()
        {
            decimal total_money_item = 0;
            Cart cart = Session["Cart"] as Cart;
            if (cart != null)
                total_money_item = cart.Total_money();
            ViewBag.TotalCart = total_money_item;
            return PartialView("BagCart");
        }
        public ActionResult CheckOut(FormCollection form)
        {
            try
            {
                Cart cart = Session["Cart"] as Cart;
                OrderPro _order = new OrderPro();
                Customer _cus = new Customer();
                _order.DateOrder = DateTime.Today;
                _cus.AddressDelivery = form["AddressDelivery"];
                _order.IDCus = int.Parse(form["CodeCustomer"]);
                db.OrderProes.Add(_order);
                foreach (var item in cart.Items)
                {
                    // lưu dòng sản phẩm vào chi tiết hóa đơn
                    OrderDetail _order_detail = new OrderDetail();
                    _order_detail.IDOrder = _order.ID;
                    _order_detail.IDPro = item._product.ProductID;
                    _order_detail.UnitPrice = (double)item._product.Price;
                    _order_detail.Quantity = item._quantity;
                    db.OrderDetails.Add(_order_detail);
                    foreach (var p in db.Products.Where(s => s.ProductID == _order_detail.IDPro)) //lấy ID Product đang có trong giỏ hàng
                    {
                        var update_quan_pro = p.Quantity - item._quantity;
                        //số lượng tồn mới = số lượng tồn - số lượng đã mua
                        if (update_quan_pro > 0)
                            p.Quantity = update_quan_pro; //thực hiện cập nhật lại số lượng tồn cho cột Quantity của bảng Product
                        else
                            p.Quantity = 0;
                    }
                }
                db.SaveChanges();
                cart.ClearCart();
                return RedirectToAction("CheckOut_Success", "ShoppingCart");
            }
            catch
            {
                return Content("Quý khách vui lòng đăng nhập để thực hiện thanh toán!");
            }
        }

        [ChildActionOnly]
        public PartialViewResult DiscountInputPartial()
        {
            return PartialView();
        }

        // Thông báo thanh toán thành công
        public ActionResult CheckOut_Success()
        {
            return View();
        }
    }
}