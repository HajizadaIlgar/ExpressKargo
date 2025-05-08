using Microsoft.AspNetCore.Mvc;
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
                IsEDV = vm.IsEdv,
            };

            await _context.Receivers.AddAsync(receiver);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
