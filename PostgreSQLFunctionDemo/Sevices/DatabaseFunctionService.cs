//using Microsoft.EntityFrameworkCore;
//using Npgsql;
//using PostgreSQLFunctionDemo.Context;
//using PostgreSQLFunctionDemo.Services.FunctionDefinitions;

//namespace PostgreSQLFunctionDemo.Services
//{
//    public class DatabaseFunctionService
//    {
//        private readonly ApplicationDbContext _context;

//        public DatabaseFunctionService(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public void CreateFunctions()
//        {
//            var connection = _context.Database.GetDbConnection();
//            connection.Open();

//            using var command = connection.CreateCommand();

//            // GetFullName fonksiyonunu oluştur



//            command.CommandText = CalculateTotalOrderFunction.Script;
//            command.ExecuteNonQuery();


//            command.CommandText = GetUserOrderSummaryFunction.Script;
//            command.ExecuteNonQuery();

//            command.CommandText = GetLastOrderDateFunction.Script;
//            command.ExecuteNonQuery();


//            connection.Close();
//        }
//    }
//}
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

            if (connection is NpgsqlConnection npgsqlConnection) // NpgsqlConnection kontrolü
            {
                npgsqlConnection.Open();

                using var command = npgsqlConnection.CreateCommand(); // NpgsqlCommand kullanılıyor

                // Fonksiyonları kontrol et ve sadece eksik olanları ekle
                AddFunctionIfNotExists(command, "CalculateTotalOrder", CalculateTotalOrderFunction.Script);
                AddFunctionIfNotExists(command, "GetUserOrderSummary", GetUserOrderSummaryFunction.Script);
                AddFunctionIfNotExists(command, "GetLastOrderDate", GetLastOrderDateFunction.Script);


                npgsqlConnection.Close();
            }
            else
            {
                throw new InvalidOperationException("Connection is not an NpgsqlConnection.");
            }
        }

        private void AddFunctionIfNotExists(NpgsqlCommand command, string functionName, string functionScript)
        {
            // Veritabanında fonksiyonun varlığını kontrol et
            command.CommandText = $@"
                SELECT COUNT(*) 
                FROM pg_proc 
                WHERE proname = '{functionName}';
            ";
            var exists = (long)command.ExecuteScalar() > 0;

            // Eğer fonksiyon mevcut değilse ekle
            if (!exists)
            {
                command.CommandText = functionScript;
                command.ExecuteNonQuery();
            }
        }
    }
}
