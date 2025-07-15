using Service_Record.DAL.Enums;

namespace Service_Record.Helper
{
    public class APIResponseClass<T>
    {
        public T? result { get; set; }
        public APIResponseStatus apiResponseStatus { get; set; }
        public string? message { get; set; }
    }
}
