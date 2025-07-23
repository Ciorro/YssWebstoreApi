namespace YssWebstoreApi.Utils
{
    public record Error(string code, string message)
    {
        public string Code { get; } = code;
        public string Message { get; } = message;

        public Error(string code)
            : this(code, "")
        { }
    }
}
