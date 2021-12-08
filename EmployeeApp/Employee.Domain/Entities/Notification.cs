using System.ComponentModel.DataAnnotations.Schema;

namespace Employee.Domain.Entities
{
    public class Notification
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public string TransactionType { get; set; }
    }
}
