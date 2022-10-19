namespace WebApiBase.Models
{
    public class ResponseVo<T>
    {
        public enum ResType { Success, Error, Warning }
        public int Code { get; set; }

        public ResType Type { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }
    }
}
