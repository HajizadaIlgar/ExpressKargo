using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YunusExpress_MVC.DataAccess;
using YunusExpress_MVC.Models;
using YunusExpress_MVC.ViewModels;

namespace YunusExpress_MVC.Controllers
{
    public class DeliveryZoneController(YunusExpressDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await _context.DeliveryZones.Include(x => x.DeliveryZoneType).ToListAsync());
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.DeliveryType = await _context.DeliveryZoneTypes.ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(DeliveryZoneVm vm)
        {
            DeliveryZone zone = new DeliveryZone
            {
                Name = vm.Name,
                DeliveryZoneTypeId = vm.DeliveryZoneTypeId,
            };
            await _context.DeliveryZones.AddAsync(zone);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
