using Microsoft.EntityFrameworkCore;
using Npgsql;
using PostgreSQLFunctionDemo.Context;
using PostgreSQLFunctionDemo.Services.FunctionDefinitions;

namespace PostgreSQLFunctionDemo.Services
{
    public class DatabaseFunctionService
    {
        private readonly ApplicationDbContext _context;

        public DatabaseFunctionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void CreateFunctions()
        {
            var connection = _context.Database.GetDbConnection();
            connection.Open();

            using var command = connection.CreateCommand();

            // GetFullName fonksiyonunu oluştur
            command.CommandText = GetFullNameFunction.Script;
            command.ExecuteNonQuery();


            command.CommandText = CalculateTotalOrderFunction.Script;
            command.ExecuteNonQuery();


            command.CommandText = GetUserOrderSummaryFunction.Script;
            command.ExecuteNonQuery();

            command.CommandText = GetLastOrderDateFunction.Script;
            command.ExecuteNonQuery();


            connection.Close();
        }
    }
}
