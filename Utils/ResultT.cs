using System.Diagnostics.CodeAnalysis;

namespace YssWebstoreApi.Utils
{
    public class Result<T> : Result
    {
        private readonly T? _value;

        private Result(T value)
            : base()
        {
            _value = value;
        }

        private Result(Error error)
            : base(error)
        { }

        public T GetValue()
        {
            if (!Success)
            {
                throw new NullReferenceException("The result is not successful and the value is null.");
            }

            return _value!;
        }

        public bool TryGetValue([MaybeNullWhen(false)] out T value)
        {
            value = _value;
            return Success;
        }

        public static Result<T> Ok(T value)
        {
            return new Result<T>(value);
        }


        public static implicit operator Result<T>(T value)
        {
            return new Result<T>(value);
        }

        public static implicit operator Result<T>(Error error)
        {
            return new Result<T>(error);
        }
    }
}
