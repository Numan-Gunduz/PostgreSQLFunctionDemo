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
            try
            {
                var totalOrder = _context.Database
                    .SqlQueryRaw<decimal>("SELECT calculatetotalorder({0})", userId)
                    .AsEnumerable()
                    .FirstOrDefault();

                return Json(new { TotalOrder = totalOrder });
            }
            catch (Exception ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
                Console.WriteLine("Stack Trace: " + ex.StackTrace);
                return Json(new { Error = ex.Message });
            }
        }
        [HttpPost]
        public IActionResult GetFullName(int userId)
        {
           
                // Kullanıcı bilgilerini al
                var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            

                // GetFullName fonksiyonunu çağır
                     var fullName = _context.Database
                    .SqlQueryRaw<string>("SELECT FullName FROM GetFullName({0}, {1})", user.FirstName, user.LastName)
                    .FirstOrDefault();

                return Json(new { FullName = fullName });
            

        }
        [HttpPost]
        public IActionResult GetUserOrderSummary(int userId, bool includeDetails)
        {
            try
            {
                // PostgreSQL'den TEXT olarak oku
                var result = _context.Database
                    .SqlQueryRaw<string>("SELECT GetUserOrderSummary({0}, {1})", userId, includeDetails)
                    .AsEnumerable()
                    .FirstOrDefault();

                if (string.IsNullOrEmpty(result))
                {
                    return Content("No data found for the user.");
                }

                // Gelen sonucu doğrudan string olarak döndür
                return Content(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
                Console.WriteLine("Stack Trace: " + ex.StackTrace);
                return Content($"Error: {ex.Message}");
            }
        }
        [HttpPost]
        public IActionResult GetLastOrderDate(int userId)
        {
            try
            {
                var lastOrderDate = _context.Database
                    .SqlQueryRaw<DateTime?>("SELECT GetLastOrderDate({0})", userId)
                    .AsEnumerable()
                    .FirstOrDefault();

                return Json(new { LastOrderDate = lastOrderDate });
            }
            catch (Exception ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
                return Json(new { Error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult GetOrderCountByUser(int userId)
        {
            try
            {
                // Veritabanı fonksiyonunu çağır
                var orderCount = _context.Database
                    .SqlQueryRaw<int>($"SELECT {GetOrderCountByUserFunction.Name}({userId})")
                    .AsEnumerable()
                    .FirstOrDefault();

                return Json(new { OrderCount = orderCount });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return Json(new { Error = ex.Message });
            }
        }




    }
}
