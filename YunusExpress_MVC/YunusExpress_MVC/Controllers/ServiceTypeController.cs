using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YunusExpress_MVC.DataAccess;
using YunusExpress_MVC.Models;
using YunusExpress_MVC.ViewModels;

namespace YunusExpress_MVC.Controllers
{
    public class ServiceTypeController(YunusExpressDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await _context.ServiceTypes.ToListAsync());
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ServiceTypesVm vm)
        {
            ServiceTypes service = new ServiceTypes
            {
                ServiceType = vm.ServiceName,
            };
            await _context.ServiceTypes.AddAsync(service);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var data = await _context.ServiceTypes.FirstOrDefaultAsync();
            if (data == null)
                return NotFound();
            _context.ServiceTypes.Remove(data);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
