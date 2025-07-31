namespace APIPractice.Models.Responses
{
    public class OkResponse<T>
    {
        public int status {  get; set; }
        public string message { get; set; }
        public T? data { get; set; } 

        public OkResponse(int status, string message,T? data = default)
        {
            this.message= message;
            this.status = status;
            this.data = data;
        }
        public static OkResponse<T?> Success(T data)
        {
            return new OkResponse<T?>(StatusCodes.Status200OK, "success", data);
        }
        public static OkResponse<T?> Empty()
        {
            return new OkResponse<T?>(StatusCodes.Status200OK, "No data found");
        }
    }
}
