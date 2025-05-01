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
            var CourierFullList = await _context.Couriers.Include(x => x.Orders).ToListAsync();
            return View(CourierFullList);
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
    }
}
