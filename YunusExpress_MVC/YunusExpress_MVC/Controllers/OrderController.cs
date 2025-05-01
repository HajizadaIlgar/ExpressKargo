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
        public async Task<IActionResult> Index()
        {
            return View(await _context.Orders.Include(x => x.DeliveryZone).Include(x => x.Courier).Include(x => x.ServiceType).ToListAsync());
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Service = await _context.ServiceTypes.ToListAsync();
            ViewBag.Receiver = await _context.Receivers.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = $"{x.ReceiverName} ({x.ReceiverAddress.ToString()})"
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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderCreateVm vm)
        {
            Order order = new Order
            {
                OrderNo = vm.OrderNo,
                InvoiceNo = vm.InvoiceNo,
                StartDate = vm.StartDate,

                ServiceId = vm.ServiceId,

                CourierId = vm.CourierId,
                DeliveryZoneId = vm.DeliveryZoneId,

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
                ServiceId = x.ServiceId,

                CourierId = x.CourierId,
                OrderPrice = x.OrderPrice,
                SpecialPrice = x.SpecialPrice,
                Discount = x.Discount,
                EDV = x.EDV,
                Note = x.Note
            }).FirstOrDefaultAsync();

            ViewBag.Service = await _context.ServiceTypes.ToListAsync();
            ViewBag.Receiver = await _context.Receivers.ToListAsync();
            ViewBag.Sender = await _context.Senders.ToListAsync();
            ViewBag.Courier = await _context.Couriers.ToListAsync();

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

                data.ServiceId = vm.ServiceId;
                data.CourierId = vm.CourierId;

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
    }
}
