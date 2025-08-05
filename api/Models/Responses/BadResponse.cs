namespace APIPractice.Models.Responses
{
    public class BadResponse<T>
    {
        public int Status { get; set; }
        public string Message { get; set; }

        public T? ErrorDetails { get; set; }

        public BadResponse(int status,string message, T? details=default)
        {
            Status = status;
            Message = message;
            ErrorDetails = details;
        }

        public static BadResponse<T> Execute(T details)
        {
            return new BadResponse<T>(StatusCodes.Status400BadRequest, "Error occured", details);
        }
    }
}
