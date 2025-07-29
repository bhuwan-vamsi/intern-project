namespace APIPractice.Models.DTO.TaskHistory
{
    public class GetTaskHistoryDto
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }

        public Guid EmployeeId { get; set; }

        public DateTime AcceptedAt { get; set; }

        public DateTime? CompletedAt { get; set; }
    }
}
