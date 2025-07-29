namespace APIPractice.Models.DTO.TaskHistory
{
    public class CreateTaskHistoryDto
    {
        public Guid OrderId { get; set; }

        public Guid EmployeeId { get; set; }
    }
}
