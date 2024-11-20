namespace PostgreSQLFunctionDemo.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
