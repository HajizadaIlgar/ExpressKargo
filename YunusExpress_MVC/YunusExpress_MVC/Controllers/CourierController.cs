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
