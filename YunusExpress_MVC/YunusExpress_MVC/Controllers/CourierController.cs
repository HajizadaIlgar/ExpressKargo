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
        public async Task<IActionResult> CourierHomePage()
        {
            return View();
        }
        public async Task<IActionResult> CourierIndex()
        {
            var orders = await _context.Orders.Include(o => o.ServiceType)
                                             .Include(o => o.ToCourier)
                                             .ToListAsync();

            var couriers = await _context.Couriers.ToListAsync();

            var totalSalary = orders.Sum(o => o.FinalPrice * 0.36m);

            var model = new CourierSalaryPageViewModel
            {
                Orders = orders,
                Couriers = couriers,
                TotalSalary = totalSalary
            };

            return View(model);
        }

        public IActionResult GetCourierOrders(string? courierName)
        {
            var ordersQuery = _context.Orders
                .Include(o => o.ServiceType)
                .Include(o => o.ToCourier)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(courierName))
                ordersQuery = ordersQuery.Where(o => o.ToCourier.CourierName == courierName);

            var orders = ordersQuery.ToList();

            var data = orders.Select(o => new
            {
                invoiceNo = o.InvoiceNo,
                senderName = o.SenderName,
                senderAddress = o.SenderAddress,
                startDate = o.StartDate.ToString("yyyy-MM-dd HH:mm"),
                serviceType = o.ServiceType.ServiceType,
                courierCode = o.ToCourier.CourierCode,
                courierName = o.ToCourier.CourierName,
                receiverName = o.ReceiverName,
                receiverAddress = o.ReceiverAddress,
                finalPrice = o.FinalPrice,
                salaryAmount = Math.Round(o.FinalPrice * 0.36m, 2)
            }).ToList();

            var totalSalary = data.Sum(x => x.salaryAmount);

            return Json(new { orders = data, totalSalary });
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
                CourierCode = vm.CourierCode,
                CourierPhoneNum = vm.CourierPhoneNum,
            };
            await _context.Couriers.AddAsync(courier);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> IsCourierCodeExists(int code)
        {
            bool exists = await _context.Couriers.AnyAsync(c => c.CourierCode == code);
            return Json(new { exists });
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
