using Divisima.BL.Repositories;
using Divisima.DAL.Entities;
using Divisima.UI.Models;
using Divisima.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Divisima.UI.Controllers
{
    public class CartController : Controller
    {
        IRepository<Product> repoProduct;
        IRepository<Order> repoOrder;
        IRepository<OrderDetail> repoOrderDetail;
        public CartController(IRepository<Product> _repoProduct, IRepository<Order> _repoOrder, IRepository<OrderDetail> _repoOrderDetail)
        {
            repoProduct = _repoProduct;
            repoOrder = _repoOrder;
            repoOrderDetail = _repoOrderDetail;
        }

        [Route("/sepet")]
        public IActionResult Index()
        {
            if (Request.Cookies["MyCart"] != null)
            {
                List<Cart> carts = JsonConvert.DeserializeObject<List<Cart>>(Request.Cookies["MyCart"]);
                if (carts.Any())
                {

                    return View(carts);
                }
            }
            return Redirect("/");
        }

        [Route("/sepet/sepeteekle"), HttpPost]
        public string AddCart(int productid, int quantity)
        {
            Product product = repoProduct.GetAll(x => x.ID == productid).Include(x => x.ProductPictures).FirstOrDefault();
            bool varMi = false;
            if (product != null)
            {
                Cart cart = new Cart()
                {
                    ProductID = productid,
                    ProductName = product.Name,
                    ProductPicture = product.ProductPictures.FirstOrDefault().Picture,
                    ProductPrice = product.Price,
                    Quantity = quantity
                };
                List<Cart> carts = new List<Cart>();
                if (Request.Cookies["MyCart"] != null)
                {
                    carts = JsonConvert.DeserializeObject<List<Cart>>(Request.Cookies["MyCart"]);
                    foreach (Cart c in carts)
                    {
                        if (c.ProductID == productid)
                        {
                            varMi = true;
                            c.Quantity += quantity;
                            break;
                        }
                    }
                }
                if (!varMi)
                    carts.Add(cart);
                CookieOptions options = new();
                options.Expires = DateTime.Now.AddHours(2);
                Response.Cookies.Append("MyCart", JsonConvert.SerializeObject(carts), options);
                return product.Name;
                
            }
            else
                return "~ Ürün Bulunamadı";
        }

        [Route("/sepet/sepetsayisi")]
        public int CartCount()
        {
            int geri = 0;
            if (Request.Cookies["MyCart"] != null)
            {
                List<Cart> carts = JsonConvert.DeserializeObject<List<Cart>>(Request.Cookies["MyCart"]);
                geri = carts.Sum(x => x.Quantity);
                //geri = carts.Count(); kaç kalem ürün ise o değer döner
            }
            return geri;
        }

        [Route("/sepet/sepettensil")]
        public string RemoveCart(int productid)
        {
            string rtn = "";
            if (Request.Cookies["MyCart"] != null)
            {
                List<Cart> carts = JsonConvert.DeserializeObject<List<Cart>>(Request.Cookies["MyCart"]);
                bool varmi = false;
                foreach (Cart c in carts)
                {
                    if (c.ProductID == productid)
                    {
                        varmi = true;
                        carts.Remove(c);
                        break;
                    }
                }
                if (varmi)
                {
                    CookieOptions options = new();
                    options.Expires = DateTime.Now.AddHours(2);
                    Response.Cookies.Append("MyCart", JsonConvert.SerializeObject(carts), options);
                    rtn = "YES";
                }
            }

            return rtn;
        }

        [Route("/sepet/alisverisitamamla")]
        public IActionResult CheckOut()
        {
            ViewBag.ShipmentFee = 1000;
            if (Request.Cookies["MyCart"] != null)
            {
                List<Cart> carts = JsonConvert.DeserializeObject<List<Cart>>(Request.Cookies["MyCart"]);
                CheckOutVM checkOutVM = new CheckOutVM()
                {
                    Order = new Order(),
                    Carts = carts
                };
                return View(checkOutVM);
            }
            else
                return Redirect("/");
        }


        [Route("/sepet/alisverisitamamla"), HttpPost, ValidateAntiForgeryToken]
        public IActionResult CheckOut(CheckOutVM model)
        {
            model.Order.RecDate = DateTime.Now;
            string orderNumber = DateTime.Now.Microsecond.ToString()+DateTime.Now.Minute.ToString()+DateTime.Now.Second.ToString()+DateTime.Now.Hour.ToString()+DateTime.Now.Microsecond.ToString()+DateTime.Now.Microsecond.ToString();
            if (orderNumber.Length > 20)
                orderNumber = orderNumber.Substring(0, 20);
                model.Order.OrderNumber = orderNumber;
            
                model.Order.OrderStatus=EOrderStatus.Hazirlaniyor;
                repoOrder.Add(model.Order);
                List<Cart> carts = JsonConvert.DeserializeObject<List<Cart>>(Request.Cookies["MyCart"]);
                foreach (Cart c in carts)
                {
                    OrderDetail orderDetail = new OrderDetail
                    {
                        OrderID = model.Order.ID,
                        ProductID=c.ProductID,
                        ProductName = c.ProductName,
                        ProductPicture = c.ProductPicture,
                        ProductPrice = c.ProductPrice,
                        Quantity = c.Quantity,
                    };
                    repoOrderDetail.Add(orderDetail);
                    Product product = repoProduct.GetAll(x=>x.ID == c.ProductID).FirstOrDefault();
                    product.Stock -= c.Quantity;
                    repoProduct.Update(product);
                }
                
            
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddHours(-1);
            Response.Cookies.Append("MyCart", JsonConvert.SerializeObject(carts), options);

            return Redirect("/");
        }
    }
}
