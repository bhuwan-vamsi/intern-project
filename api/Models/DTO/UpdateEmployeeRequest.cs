namespace APIPractice.Models.DTO
{
    public class UpdateEmployeeRequest
    {
        public Guid EmployeeId { get; set; }
        public Guid? ManagerId { get; set; }
    }
}
