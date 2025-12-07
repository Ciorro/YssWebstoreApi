namespace YssWebstoreApi.Utils
{
    public record Error
    {
        public string Code { get; }
        public int HttpStatus { get; }
        public string Message { get; }

        public Error(string code, int httpStatus)
            : this(code, httpStatus, "")
        { }

        public Error(string code, int httpStatus, string message)
        {
            Code = code;
            HttpStatus = httpStatus;
            Message = message;
        }
    }
}
