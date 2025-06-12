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
                ReceiverDiscount = vm.ReceiverDiscount,

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

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _context.Receivers.Where(x => x.Id == id).Select(x => new ReceiverUpdateVm
            {
                Id = x.Id,
                ClientCode = x.ClientCode,
                ReceiverName = x.ReceiverName,
                ReceiverAddress = x.ReceiverAddress,
                ReceiverPhoneNum = x.ReceiverPhoneNum,
                ContractDate = x.ContractDate,
                ReceiverDiscount = x.ReceiverDiscount,
                IsEDV = x.IsEDV,

                BankName = x.BankName,
                BankCode = x.BankCode,
                BankVoen = x.BankVoen,
                Swift = x.Swift,
                Voen = x.Voen,
                Iban = x.Iban,
                Mh = x.Mh,
                QiymetVar = x.QiymetVar,

            }).FirstOrDefaultAsync();
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, ReceiverUpdateVm vm)
        {
            if (id == null) return BadRequest();
            var data = await _context.Receivers.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (data == null) return NotFound();
            if (data is not null)
            {
                data.ClientCode = vm.ClientCode;
                data.ReceiverName = vm.ReceiverName;
                data.ReceiverAddress = vm.ReceiverAddress;
                data.ContractDate = vm.ContractDate;
                data.ReceiverDiscount = vm.ReceiverDiscount;
                data.IsEDV = vm.IsEDV;
                data.BankName = vm.BankName;
                data.BankCode = vm.BankCode;
                data.BankVoen = vm.BankVoen;
                data.Swift = vm.Swift;
                data.Voen = vm.Voen;
                data.Iban = vm.Iban;
                data.Mh = vm.Mh;
                data.QiymetVar = vm.QiymetVar;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> HesabFaktura(string senderName)
        {
            var orders = await _context.Orders
                .Where(x => x.IsPaylanma != true)
                .Include(x => x.Receiver)
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

            var now = DateTime.Now;
            var startOfMonth = new DateTime(now.Year, now.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1);

            var receivers = await _context.Receivers.ToListAsync();
            var totalSalary = orders.Where(o => o.StartDate >= startOfMonth && o.StartDate < endOfMonth && o.IsPaylanma != true)
                        .Sum(o => (decimal?)o.FinalPrice) ?? 0;
            var finalPrice = orders.Where(o => o.StartDate >= startOfMonth && o.StartDate < endOfMonth && o.IsPaylanma != true)
                    .Sum(o => o.FinalPrice);

            var model = new ReceiverMontlySalaryVM
            {
                Orders = orders,
                Receivers = receivers,
                TotalSalary = totalSalary,
                FinalPrice = finalPrice,
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

