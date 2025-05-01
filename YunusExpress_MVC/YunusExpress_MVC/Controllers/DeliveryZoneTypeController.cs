using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YunusExpress_MVC.DataAccess;
using YunusExpress_MVC.Models;
using YunusExpress_MVC.ViewModels;

namespace YunusExpress_MVC.Controllers
{
    public class DeliveryZoneTypeController(YunusExpressDbContext _context) : Controller
    {
        public async Task<ActionResult> Index()
        {
            return View(await _context.DeliveryZoneTypes.ToListAsync());
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(DeliveryTypeVm vm)
        {
            DeliveryZoneType type = new DeliveryZoneType
            {
                TypeName = vm.TypeName,
            };
            await _context.DeliveryZoneTypes.AddAsync(type);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }
    }
}
