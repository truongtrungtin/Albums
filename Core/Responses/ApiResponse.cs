namespace Infrastructure.Data.Responses
{
    public class ApiResponse
    {
        public int code { get; set; }
        public bool isSuccess { get; set; }
        public string message { get; set; }
        public object data { get; set; }

    }
}
