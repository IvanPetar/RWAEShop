using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RWAEshopDAL.Models;
using RWAEshopDAL.Services;
using RWAEShopMVC.ViewModels;

namespace RWAEShopMVC.Controllers
{
    public class OrderController : Controller
    {

        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderService, IProductService productService, IUserService userService, IMapper mapper)
        {
            _orderService = orderService;
            _productService = productService;
            _userService = userService;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)
        {
            var username = User.Identity.Name;
            var user = _userService.GetAllUsers().FirstOrDefault(u => u.Username == username);
            if (user == null) return Unauthorized();

            // check is there open order for user
            var order = _orderService.GetAllOrders()
                .FirstOrDefault(o => o.UserId == user.IdUser);

            // create new order if not exists
            if (order == null)
            {
                order = new Order
                {
                    UserId = user.IdUser,
                    OrderDate = DateTime.Now,
                    OrderItems = new List<OrderItem>(),
                    TotalAmount = 0
                };
                _orderService.CreateOrder(order);
            }

            // check is there any products in base
            var existingItem = order.OrderItems.FirstOrDefault(oi => oi.ProductId == productId);
            var product = _productService.GetProduct(productId);
            if (product == null || product.Quantity < quantity) return BadRequest("Not enough products.");

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                order.OrderItems.Add(new OrderItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    Price = product.Price
                });
            }

            // Smanji zalihu
            product.Quantity -= quantity;
            _productService.UpdateProduct(product);

            // Ažuriraj total
            order.TotalAmount = order.OrderItems.Sum(x => x.Quantity * x.Price);

            // Spremi order
            _orderService.UpdateOrder(order);

            return RedirectToAction("Cart");
        }

        // Prikaz košarice
        public IActionResult Cart()
        {
            var username = User.Identity.Name;
            var user = _userService.GetAllUsers().FirstOrDefault(u => u.Username == username);
            if (user == null) return Unauthorized();

            var order = _orderService.GetAllOrders().FirstOrDefault(o => o.UserId == user.IdUser);
            if (order == null)
                return View(new OrderVM { OrderItems = new List<OrderItemVM>() });

            var orderVm = _mapper.Map<OrderVM>(order);
            return View(orderVm);
        }

        // Očisti košaricu
        [HttpPost]
        public IActionResult ClearCart(int orderId)
        {
            var order = _orderService.GetOrder(orderId);
            if (order != null)
            {
                // Vrati količine proizvoda
                foreach (var item in order.OrderItems)
                {
                    var product = _productService.GetProduct(item.ProductId);
                    if (product != null)
                    {
                        product.Quantity += item.Quantity;
                        _productService.UpdateProduct(product);
                    }
                }

                _orderService.DeleteOrder(orderId);
            }

            return RedirectToAction("Cart");
        }


        [HttpPost]
        public IActionResult RemoveItem(int orderId, int productId)
        {
           
            var item = _orderService.GetOrder(orderId)?.OrderItems.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                var product = _productService.GetProduct(productId);
                if (product != null)
                {
                    product.Quantity += item.Quantity;
                    _productService.UpdateProduct(product);
                }

                _orderService.RemoveOrderItem(orderId, productId);
            }

            return RedirectToAction("Cart");
        }


        [HttpPost]
        public IActionResult UpdateQuantity(int orderId, int productId, int quantity)
        {
            var order = _orderService.GetOrder(orderId);
            var item = order.OrderItems.FirstOrDefault(oi => oi.ProductId == productId);
            if (item != null && quantity > 0)
            {
                var product = _productService.GetProduct(productId);

                if (product != null)
                {
                    product.Quantity += item.Quantity; 
                    if (product.Quantity < quantity)
                        return BadRequest("Not enough products!");
                    product.Quantity -= quantity; 
                    _productService.UpdateProduct(product);
                }

                item.Quantity = quantity;
                order.TotalAmount = order.OrderItems.Sum(i => i.Quantity * i.Price);
                _orderService.UpdateOrder(order);
            }
            return RedirectToAction("Cart");
        }

    }
}
