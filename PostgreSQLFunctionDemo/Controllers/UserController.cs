using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostgreSQLFunctionDemo.Context;
using PostgreSQLFunctionDemo.Models;
using PostgreSQLFunctionDemo.Services.FunctionDefinitions;

namespace PostgreSQLFunctionDemo.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.Include(u => u.Orders).ToListAsync();
            return View(users);
        }

        [HttpPost]
        public IActionResult CalculateTotalOrder(int userId)
        {         
                var totalOrder = _context.Database
                    .SqlQueryRaw<decimal>("SELECT calculatetotalorder({0})", userId)
                    .AsEnumerable()
                    .FirstOrDefault();

                return Json(new { TotalOrder = totalOrder });
            
        }

        [HttpPost]
        public IActionResult GetUserOrderSummary(int userId, bool includeDetails)
        {       
                // PostgreSQL'den TEXT olarak oku
                var result = _context.Database
                    .SqlQueryRaw<string>("SELECT GetUserOrderSummary({0}, {1})", userId, includeDetails)
                    .AsEnumerable()
                    .FirstOrDefault();

                // Gelen sonucu doğrudan string olarak döndür
                return Content(result);
            
        }
        [HttpPost]
        public IActionResult GetLastOrderDate(int userId)
        {
            
                var lastOrderDate = _context.Database
                    .SqlQueryRaw<DateTime?>("SELECT GetLastOrderDate({0})", userId)
                    .AsEnumerable()
                    .FirstOrDefault();

                return Json(new { LastOrderDate = lastOrderDate });
          
        }





    }
}
