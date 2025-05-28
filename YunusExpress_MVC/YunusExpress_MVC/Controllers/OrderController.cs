using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using YunusExpress_MVC.DataAccess;
using YunusExpress_MVC.Models;
using YunusExpress_MVC.ViewModels;

namespace YunusExpress_MVC.Controllers
{
    public class OrderController(YunusExpressDbContext _context) : Controller
    {
        public async Task<IActionResult> Index(string senderName, string courierName, DateTime? startDate, DateTime? endDate, string type)
        {
            var orders = _context.Orders
                .Include(x => x.Receiver)
                .Include(x => x.DeliveryZone)
                .Include(x => x.ToCourier)
                .Include(x => x.FromCourier)
                .Include(x => x.ServiceType)
                .AsQueryable();

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
            var senderNamesList = await _context.Receivers
                 .Select(r => new { r.ClientCode, r.ReceiverName })
                 .ToListAsync();

            ViewBag.Senders = senders;
            ViewBag.SenderNames = senderNamesList;


            var courierList = await _context.Couriers
                .Select(x => new SelectListItem
                {
                    Value = x.CourierName.ToString(),   // KOD dropdown-un value-sudur
                    Text = $"{x.CourierCode} ({x.CourierName})"     // Dropdown-da görünəcək də koddur
                })
                .Distinct()
                .ToListAsync();

            // KOD → AD xəritəsi
            var courierNames = await _context.Couriers
               .ToDictionaryAsync(r => r.CourierCode.ToString(), r => r.CourierName);

            ViewBag.Couriers = courierList;
            ViewBag.CourierNames = courierNames;



            if (!string.IsNullOrEmpty(senderName) && senderName != "Hamısı")
            {
                orders = orders.Where(x => x.ReceiverName == senderName);
            }

            if (!string.IsNullOrEmpty(courierName))
            {
                orders = orders.Where(o => o.ToCourier.CourierName.Contains(courierName));
            }

            if (startDate.HasValue && endDate.HasValue)
            {
                var start = startDate.Value.Date;
                var end = endDate.Value.Date.AddDays(1); // endDate gününü də daxil etmək üçün
                orders = orders.Where(x => x.StartDate >= start && x.StartDate < end);
            }
            else if (startDate.HasValue)
            {
                var date = startDate.Value.Date;
                orders = orders.Where(x => x.StartDate.Date == date);
            }
            else if (endDate.HasValue)
            {
                var date = endDate.Value.Date;
                orders = orders.Where(x => x.StartDate.Date <= date);
            }
            if (type == "waybill")
            {
                // Məsələn: Waybill-ə görə `SenderName == "Private"`
                orders = orders.Where(x => x.ReceiverName == "Private");
            }
            if (type == "paylanma")
            {
                orders = orders.Where(x => x.IsPaylanma);
            }
            return View(await orders.ToListAsync());
        }
        [HttpPost]
        public IActionResult UpdatePaylanma([FromBody] PaylanmaDto data)
        {
            var sifaris = _context.Orders.FirstOrDefault(s => s.OrderNo == data.id);
            if (sifaris == null)
                return NotFound();

            sifaris.IsPaylanma = data.IsPaylanma;
            _context.SaveChanges();

            return Ok();
        }

        [HttpGet]
        public IActionResult GetSenders()
        {
            var senders = _context.Receivers
                .Select(o => new
                {
                    clientCode = o.ClientCode,
                    receiverName = o.ReceiverName
                })
                .Distinct()
                .ToList();

            return Json(senders);
        }

        [HttpGet]
        public IActionResult GetCouriers()
        {
            var couriers = _context.Couriers
                .Select(o => new
                {
                    courierCode = o.CourierId,
                    courierName = o.CourierName
                })
                .Distinct()
                .ToList();

            return Json(couriers);
        }


        [HttpGet]
        public IActionResult GetReceiverById(int id)
        {
            var receiver = _context.Receivers.FirstOrDefault(r => r.Id == id);
            if (receiver == null) return NotFound();

            return Json(receiver);
        }


        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsInvoiceNoAvailable(int invoiceNo)
        {
            var exists = await _context.Orders.AnyAsync(o => o.InvoiceNo == invoiceNo);
            return Json(!exists);
        }

        public async Task<IActionResult> Create()
        {

            ViewBag.Service = await _context.ServiceTypes.ToListAsync();

            var receivers = await _context.Receivers.ToListAsync();

            ViewBag.Receiver = receivers.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = $"{x.ClientCode} ({x.ReceiverName})"
            }).ToList();

            ViewBag.ReceiverCodes = receivers.ToDictionary(x => x.Id.ToString(), x => x.ClientCode);


            ViewBag.Sender = await _context.Senders.Select(x => new SelectListItem
            {
                Value = x.SenderId.ToString(),
                Text = $"{x.SenderName} ({x.SenderAddress.ToString()})"
            }).ToListAsync();

            ViewBag.Courier = await _context.Couriers
                .Select(c => new
                {
                    CourierId = c.CourierId,
                    DisplayText = c.CourierCode + "  |    " + c.CourierName
                }).ToListAsync();


            ViewBag.DeliveryZone = await _context.DeliveryZones.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = $"{x.Name}"
            }).ToListAsync();

            var lastOrderNo = await _context.Orders
               .OrderByDescending(o => o.OrderNo)
               .Select(o => o.OrderNo)
               .FirstOrDefaultAsync();

            ViewBag.NextOrderNo = (lastOrderNo) + 1;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderCreateVm vm)
        {
            if (!ModelState.IsValid)
                return View(vm);
            var receiver = await _context.Receivers
                .FirstOrDefaultAsync(r => r.Id == vm.ReceiverId);

            if (receiver == null)
            {
                ModelState.AddModelError("ReceiverId", "Qəbul edən şəxs tapılmadı.");
                return View(vm);
            }

            Order order = new Order
            {
                InvoiceNo = vm.InvoiceNo,
                StartDate = vm.StartDate,

                SenderAddress = vm.SenderAddress,
                SenderName = vm.SenderName,
                SenderPhoneNum = vm.SenderPhoneNum,

                ReceiverName = vm.ReceiverName,
                ReceiverPhoneNum = vm.ReceiverPhoneNum,
                ReceiverAddress = vm.ReceiverAddress,
                ReceiverId = vm.ReceiverId,

                ServiceId = vm.ServiceId,
                ToCourierId = vm.ToCourierId,
                FromCourierId = vm.FromCourierId,
                DeliveryZoneId = vm.DeliveryZoneId,

                ZengEdeninAdi = vm.ZengEdeninAdi,

                OrderPrice = vm.OrderPrice,
                SpecialPrice = vm.SpecialPrice,
                Discount = vm.Discount,
                FinalPrice = vm.FinalPrice,
                EDV = receiver.IsEDV,
                Note = vm.Note
            };

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");

        }
        public async Task<IActionResult> Update(int? orderNo)
        {
            if (orderNo == null) return BadRequest();
            var data = await _context.Orders.Where(x => x.OrderNo == orderNo).Select(x => new OrderUpdateVm
            {
                OrderNo = x.OrderNo,
                InvoiceNo = x.InvoiceNo,
                StartDate = x.StartDate,

                SenderAddress = x.SenderAddress,
                SenderName = x.SenderName,
                SenderPhoneNum = x.SenderPhoneNum,

                ReceiverName = x.ReceiverName,
                ReceiverPhoneNum = x.ReceiverPhoneNum,
                ReceiverAddress = x.ReceiverAddress,

                ZengEdeninAdi = x.ZengEdeninAdi,

                ServiceId = x.ServiceId,

                ToCourierId = x.ToCourierId,
                FromCourierId = x.FromCourierId,

                DeliveryZoneId = x.DeliveryZoneId,

                OrderPrice = x.OrderPrice,
                SpecialPrice = x.SpecialPrice,
                Discount = x.Discount,
                FinalPrice = x.FinalPrice,
                EDV = x.EDV,
                Note = x.Note

            }).FirstOrDefaultAsync();


            ViewBag.Service = await _context.ServiceTypes.ToListAsync();
            ViewBag.Receiver = await _context.Receivers.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = $"{x.ClientCode} ({x.ReceiverName.ToString()})"
            }).ToListAsync();
            ViewBag.Sender = await _context.Senders.Select(x => new SelectListItem
            {
                Value = x.SenderId.ToString(),
                Text = $"{x.SenderName} ({x.SenderAddress.ToString()})"
            }).ToListAsync();

            ViewBag.Courier = await _context.Couriers
            .Select(c => new
            {
                CourierId = c.CourierId,
                DisplayText = c.CourierCode + " | " + c.CourierName
            }).ToListAsync();

            ViewBag.DeliveryZone = await _context.DeliveryZones.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = $"{x.Name} "
            }).ToListAsync();

            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? orderNo, OrderUpdateVm vm)
        {
            if (orderNo == null) return BadRequest();
            var data = await _context.Orders.Where(x => x.OrderNo == orderNo).FirstOrDefaultAsync();
            if (data == null) return NotFound();
            if (data is not null)
            {
                data.InvoiceNo = vm.InvoiceNo;
                data.StartDate = vm.StartDate;

                data.SenderAddress = vm.SenderAddress;
                data.SenderName = vm.SenderName;
                data.SenderPhoneNum = vm.SenderPhoneNum;

                data.ReceiverName = vm.ReceiverName;
                data.ReceiverPhoneNum = vm.ReceiverPhoneNum;
                data.ReceiverAddress = vm.ReceiverAddress;


                data.ServiceId = vm.ServiceId;

                data.ToCourierId = vm.ToCourierId;
                data.FromCourierId = vm.FromCourierId;

                data.DeliveryZoneId = vm.DeliveryZoneId;

                data.ZengEdeninAdi = vm.ZengEdeninAdi;

                data.OrderPrice = vm.OrderPrice;
                data.SpecialPrice = vm.SpecialPrice;
                data.Discount = vm.Discount;
                data.EDV = vm.EDV;
                data.FinalPrice = vm.FinalPrice;
                data.Note = vm.Note;

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ReceiverData()
        {
            return Json(await _context.Receivers.Select(x => new
            {
                x.ClientCode,
                x.Id,
                x.ReceiverName,
                x.ReceiverPhoneNum,
                x.ReceiverAddress,
                x.QiymetVar,
                x.IsEDV,
            }).ToListAsync());
        }
        public async Task<IActionResult> Delete(int orderNo)
        {
            var data = await _context.Orders.Where(x => x.OrderNo == orderNo).FirstOrDefaultAsync();
            if (data == null)
                return NotFound();
            _context.Orders.Remove(data);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult GetClientCode(int id)
        {
            var receiver = _context.Receivers.FirstOrDefault(r => r.Id == id);

            if (receiver == null)
                return NotFound();

            return Json(new { clientCode = receiver.ClientCode });
        }
        //filterizasiya
        [HttpGet]
        public async Task<IActionResult> OrderList()
        {
            var ordersList = await _context.Orders.ToListAsync();
            return Json(ordersList);
        }

        public async Task<IActionResult> InvoiceTab()
        {
            var now = DateTime.Now;
            var startOfMonth = new DateTime(now.Year, now.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1);

            var model = await _context.Receivers
                .Select(r => new ReceiverDebtViewModel
                {
                    id = r.Id,
                    ReceiverName = r.ReceiverName,
                    TotalDebt = r.Orders
                        .Where(o => o.StartDate >= startOfMonth && o.StartDate < endOfMonth && !o.IsPaylanma)
                        .Sum(o => (decimal?)o.FinalPrice) ?? 0
                })
                .ToListAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ClearDebt(int id)
        {
            var now = DateTime.Now;
            var startOfMonth = new DateTime(now.Year, now.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1);

            var orders = await _context.Orders
                .Where(o => o.ReceiverId == id && o.StartDate >= startOfMonth && o.StartDate < endOfMonth && !o.IsPaylanma)
                .ToListAsync();

            foreach (var order in orders)
            {
                order.IsPaylanma = true;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(InvoiceTab));
        }

        public async Task<IActionResult> ForPrintInvoice(int? id)
        {
            InvoiceDto dto = new InvoiceDto();
            dto.PartOfInvoice = await _context.Receivers.Include(x => x.Orders)
            .Where(x => x.Id == id)
            .Select(x => new InvoiceToListVM
            {
                ReceiverName = x.ReceiverName,
                ReceiverAddress = x.ReceiverAddress,
                ReceiverPhoneNum = x.ReceiverPhoneNum,
                BankName = x.BankName,
                BankCode = x.BankCode,
                ClientCode = x.ClientCode,
                BankVoen = x.BankVoen,
                Voen = x.Voen,
                Iban = x.Iban,
                Swift = x.Swift,
                Mh = x.Mh,
                Orders = x.Orders.Select(x => x.InvoiceNo).FirstOrDefault(),
                ContractDate = x.ContractDate,
            }).ToListAsync();

            dto.Orders = await _context.Orders
                .Where(x => x.ReceiverId == id && x.IsPaylanma != true)
                .Include(x => x.Receiver)
                .Include(x => x.DeliveryZone)
                .Include(x => x.ServiceType)
                .Include(x => x.ToCourier)
                .Include(x => x.FromCourier)
              .ToListAsync();

            dto.PartOfPrice = await _context.Orders
                .Where(x => x.ReceiverId == id && x.IsPaylanma != true)
                .Select(x => new PriceToListVM
                {
                    OrderNo = x.OrderNo,
                    Total = x.FinalPrice.ToString("0.00"),
                    Edv = x.EDV,
                    Discount1 = x.Discount,
                    Discount2 = x.Discount,
                }).ToListAsync();

            dto.TotalPrice = await _context.Orders
                .Where(x => x.ReceiverId == id && x.IsPaylanma != true)
                .SumAsync(x => x.FinalPrice);

            dto.Receivers = await _context.Receivers.Where(x => x.Id == id).ToListAsync();

            return View(dto);
        }

    }
}
