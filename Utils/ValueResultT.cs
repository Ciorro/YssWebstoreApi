using System.Diagnostics.CodeAnalysis;

namespace YssWebstoreApi.Utils
{
    public class ValueResult<T> : ValueResult
    {
        protected ValueResult(T value)
            : base(value!)
        { }

        private ValueResult(Error error)
            : base(error)
        { }

        public new T GetValue()
        {
            if (!Success)
            {
                throw new NullReferenceException("The result is not successful and the value is null.");
            }

            return (T)Value!;
        }

        public bool TryGetValue([MaybeNullWhen(false)] out T value)
        {
            if (Value is T resultValue)
            {
                value = resultValue;
                return Success;
            }

            value = default;
            return false;
        }

        public static ValueResult<T> Ok(T value)
        {
            return new ValueResult<T>(value);
        }


        public static implicit operator ValueResult<T>(T value)
        {
            return new ValueResult<T>(value);
        }

        public static implicit operator ValueResult<T>(Error error)
        {
            return new ValueResult<T>(error);
        }
    }
}
