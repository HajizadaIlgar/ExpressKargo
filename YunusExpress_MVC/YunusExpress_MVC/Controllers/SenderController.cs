using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YunusExpress_MVC.DataAccess;
using YunusExpress_MVC.Models;
using YunusExpress_MVC.ViewModels;

namespace YunusExpress_MVC.Controllers
{
    public class SenderController(YunusExpressDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await _context.Senders.ToListAsync());
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(SenderCreateVm vm)
        {
            Sender sender = new Sender
            {
                SenderName = vm.SenderName,
                SenderAddress = vm.SenderAddress,
                SenderPhoneNum = vm.SenderPhoneNum,
            };

            await _context.Senders.AddAsync(sender);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
