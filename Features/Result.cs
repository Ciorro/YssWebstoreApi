namespace YssWebstoreApi.Features
{
    public class Result
    {
        public bool Success { get; }
        public Error? Error { get; }

        protected Result()
        {
            Success = true;
        }

        protected Result(Error error)
        {
            Error = error;
        }

        public static Result Ok()
        {
            return new Result();
        }

        public static Result Fail(Error error)
        {
            return new Result(error);
        }

        public static implicit operator Result(Error error)
        {
            return new Result(error);
        }
    }
}
