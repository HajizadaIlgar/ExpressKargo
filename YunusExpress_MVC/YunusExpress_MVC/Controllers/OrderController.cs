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
        public async Task<IActionResult> Index(DateTime? startDate)
        {
            var orders = _context.Orders.Include(x => x.DeliveryZone).Include(x => x.Courier).Include(x => x.ServiceType).AsQueryable();


            if (startDate.HasValue)
            {
                var selectedDate = startDate.Value.Date;
                orders = orders.Where(o => o.StartDate.Date == selectedDate);
            }

            return View(orders.ToList());
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Service = await _context.ServiceTypes.ToListAsync();
            var receiverList = await _context.Receivers.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = $"{x.ClientCode} ({x.ReceiverName})"
            }).ToListAsync();

            ViewBag.Receiver = new SelectList(receiverList, "Value", "Text");

            ViewBag.Sender = await _context.Senders.Select(x => new SelectListItem
            {
                Value = x.SenderId.ToString(),
                Text = $"{x.SenderName} ({x.SenderAddress.ToString()})"
            }).ToListAsync();
            ViewBag.Courier = await _context.Couriers.ToListAsync();
            ViewBag.DeliveryZone = await _context.DeliveryZones.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = $"{x.Name} ({x.DeliveryZoneType.TypeName})"
            }).ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderCreateVm vm)
        {
            if (!ModelState.IsValid) return View();

            Order order = new Order
            {
                OrderNo = vm.OrderNo,
                InvoiceNo = vm.InvoiceNo,
                StartDate = vm.StartDate,

                SenderAddress = vm.SenderAddress,
                SenderName = vm.SenderName,
                SenderPhoneNum = vm.SenderPhoneNum,

                ReceiverName = vm.ReceiverName,
                ReceiverPhoneNum = vm.ReceiverPhoneNum,
                ReceiverAddress = vm.ReceiverAddress,


                ServiceId = vm.ServiceId,
                CourierId = vm.CourierId,
                DeliveryZoneId = vm.DeliveryZoneId,

                ZengEdeninAdi = vm.ZengEdeninAdi,

                OrderPrice = vm.OrderPrice,
                SpecialPrice = vm.SpecialPrice,
                Discount = vm.Discount,
                EDV = vm.EDV,
                Note = vm.Note
            };
            await _context.Orders.AddAsync(order);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return BadRequest();
            var data = await _context.Orders.Where(x => x.Id == id).Select(x => new OrderUpdateVm
            {
                Id = x.Id,
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
                CourierId = x.CourierId,
                DeliveryZoneId = x.DeliveryZoneId,

                OrderPrice = x.OrderPrice,
                SpecialPrice = x.SpecialPrice,
                Discount = x.Discount,
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
            ViewBag.Courier = await _context.Couriers.ToListAsync();
            ViewBag.DeliveryZone = await _context.DeliveryZones.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = $"{x.Name} ({x.DeliveryZoneType.TypeName})"
            }).ToListAsync();

            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, OrderUpdateVm vm)
        {
            if (id == null) return BadRequest();
            var data = await _context.Orders.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (data == null) return NotFound();
            if (data is not null)
            {
                data.OrderNo = vm.OrderNo;
                data.InvoiceNo = vm.InvoiceNo;
                data.StartDate = vm.StartDate;

                data.SenderAddress = vm.SenderAddress;
                data.SenderName = vm.SenderName;
                data.SenderPhoneNum = vm.SenderPhoneNum;

                data.ReceiverName = vm.ReceiverName;
                data.ReceiverPhoneNum = vm.ReceiverPhoneNum;
                data.ReceiverAddress = vm.ReceiverAddress;


                data.ServiceId = vm.ServiceId;
                data.CourierId = vm.CourierId;
                data.DeliveryZoneId = vm.DeliveryZoneId;

                data.ZengEdeninAdi = vm.ZengEdeninAdi;

                data.OrderPrice = vm.OrderPrice;
                data.SpecialPrice = vm.SpecialPrice;
                data.Discount = vm.Discount;
                data.EDV = vm.EDV;
                data.Note = vm.Note;

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _context.Orders.FirstOrDefaultAsync();
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
            if (order.ServiceType?.ServiceType == "Express")
            {
                if (order.EDV is not null)
                {
                    decimal discounted = order.OrderPrice - ((order.OrderPrice + order.SpecialPrice) * (order.Discount / 100)) ?? 0;
                    return discounted;
                }
                else
                {
                    decimal discounted = order.OrderPrice - ((order.OrderPrice + order.SpecialPrice) * (order.Discount / 100)) ?? 0;
                    return discounted + (discounted * 18 / 100);
                }
            }
            else
            {
                if (order.EDV is not null)
                {
                    decimal discounted = order.OrderPrice * (order.Discount / 100) ?? 0;
                    return discounted;
                }
                else
                {
                    decimal discounted = order.OrderPrice * (order.Discount / 100) ?? 0;
                    return discounted + (discounted * 18 / 100);
                }
            }
        }
    }
}
