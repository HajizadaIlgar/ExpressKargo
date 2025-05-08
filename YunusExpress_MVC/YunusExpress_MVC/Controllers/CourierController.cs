using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YunusExpress_MVC.DataAccess;
using YunusExpress_MVC.Models;
using YunusExpress_MVC.ViewModels;

namespace YunusExpress_MVC.Controllers
{
    public class CourierController(YunusExpressDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var CourierFullList = await _context.Couriers.ToListAsync();
            return View(CourierFullList);
        }
        public async Task<IActionResult> CourierIndex()
        {
            var couriers = _context.Couriers.ToList();
            var orders = _context.Orders
                .Include(o => o.ServiceType)
                .ToList();

            var salaryList = couriers.Select(courier =>
            {
                var courierOrders = orders.Where(o => o.CourierId == courier.CourierId);

                var totalDeliveryAmount = courierOrders.Sum(order => CalculateTotalDeliveryPrice(order));

                var orderNo = orders.Where(y => y.OrderNo == courier.OrdersId);
                return new CourierSalaryViewModel
                {
                    OrderNo = courier.OrdersId,
                    CourierName = courier.CourierName,
                    TotalDeliveredAmount = totalDeliveryAmount,
                    Salary = totalDeliveryAmount * 0.36m
                };
            }).ToList();

            return View(salaryList);
        }
        private decimal CalculateTotalDeliveryPrice(Order order)
        {
            decimal orderPrice = order.OrderPrice;
            decimal specialPrice = order.SpecialPrice.HasValue ? order.SpecialPrice.Value : 0;
            decimal discount = order.Discount.HasValue ? order.Discount.Value : 0;
            bool hasDiscount = order.Discount.HasValue && order.Discount.Value > 0;
            bool hasSpecialPrice = order.SpecialPrice.HasValue && order.SpecialPrice.Value > 0;

            decimal totalBeforeDiscount = orderPrice + specialPrice;

            decimal discountedPrice = hasDiscount ? totalBeforeDiscount - (totalBeforeDiscount * discount / 100) : totalBeforeDiscount;

            if (order.EDV == true)
            {
                decimal edvAmount = discountedPrice * 0.18m;
                return discountedPrice + edvAmount;
            }

            return discountedPrice;
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CourierCreateVm vm)
        {
            if (vm is null) return NotFound();
            Courier courier = new Courier
            {
                CourierName = vm.CourierName,
                CourierPhoneNum = vm.CourierPhoneNum,
            };
            await _context.Couriers.AddAsync(courier);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var data = await _context.Couriers.Where(x => x.CourierId == id).FirstOrDefaultAsync();
            if (data == null) return NotFound();
            _context.Couriers.Remove(data);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
