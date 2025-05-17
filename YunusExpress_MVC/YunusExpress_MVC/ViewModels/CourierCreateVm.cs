using System.ComponentModel.DataAnnotations;
using YunusExpress_MVC.DataAccess;

namespace YunusExpress_MVC.ViewModels
{
    public class CourierCreateVm
    {
        public int CourierCode { get; set; }
        public string CourierName { get; set; }
        public string CourierPhoneNum { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var _context = (YunusExpressDbContext)validationContext.GetService(typeof(YunusExpressDbContext));

            if (_context.Couriers.Any(c => c.CourierCode == CourierCode))
            {
                yield return new ValidationResult("Bu kuryer nömrəsi artıq mövcuddur.", new[] { nameof(CourierCode) });
            }
        }
    }
}
