using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using YunusExpress_MVC.DataAccess;
using YunusExpress_MVC.Models;
using YunusExpress_MVC.ViewModels;

namespace YunusExpress_MVC.Controllers
{
    public class ReceiverController(YunusExpressDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await _context.Receivers.ToListAsync());
        }

        public async Task<IActionResult> ReceiverList()
        {
            return Json(await _context.Receivers.ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ReceiverCreateVm vm)
        {
            Receiver receiver = new Receiver
            {
                ReceiverName = vm.ReceiverName,
                ReceiverAddress = vm.ReceiverAddress,
                ReceiverPhoneNum = vm.ReceiverPhoneNum,
                ClientCode = vm.ClientCode,
                IsEDV = vm.IsEDV,
                ContractDate = vm.ContractDate,

                BankName = vm.BankName,
                BankCode = vm.BankCode,
                BankVoen = vm.BankVoen,
                Swift = vm.Swift,
                Voen = vm.Voen,
                Iban = vm.Iban,
                Mh = vm.Mh,
                QiymetVar = vm.QiymetVar,
            };
            await _context.Receivers.AddAsync(receiver);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> HesabFaktura(string senderName)
        {
            var orders = await _context.Orders
                .Include(x => x.Receiver)
                .Include(x => x.DeliveryZone)
                .Include(x => x.ToCourier)
                .Include(x => x.FromCourier)
                .Include(x => x.ServiceType)
                .ToListAsync();

            // Kod siyahısı üçün
            var senders = await _context.Receivers
                .Select(r => new SelectListItem
                {
                    Value = r.ReceiverName,    // kod
                    Text = $"{r.ClientCode} ({r.ReceiverName})"
                })
                .Distinct()
                .ToListAsync();
            // Kod → Ad map-i
            var senderNamesMap = await _context.Receivers
           .GroupBy(r => r.ClientCode)
           .Select(g => g.First()) // Təkrarlardan qaçın
           .ToDictionaryAsync(x => x.ClientCode, x => x.ReceiverName);

            ViewBag.Senders = senders;
            ViewBag.SenderNames = senderNamesMap;

            if (!string.IsNullOrEmpty(senderName) && senderName != "Hamısı")
            {
                orders = orders.Where(x => x.ReceiverName == senderName).ToList();
            }

            var receivers = await _context.Receivers.ToListAsync();
            var totalSalary = orders.Sum(o => o.FinalPrice);

            var model = new ReceiverMontlySalaryVM
            {
                Orders = orders,
                Receivers = receivers,
                TotalSalary = totalSalary
            };

            return View(model);
        }

        public async Task<IActionResult> GetReceiverOrders(string? receiverName)
        {

            var ordersQuery = _context.Orders
                .Include(o => o.ServiceType)
                .Include(o => o.Receiver)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(receiverName))
                ordersQuery = ordersQuery.Where(o => o.Receiver.ReceiverName == receiverName);

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
    }
}

