using System.Diagnostics.CodeAnalysis;

namespace YssWebstoreApi.Utils
{
    public class ValueResult : Result
    {
        protected readonly object? Value;

        protected ValueResult(object value)
            : base()
        {
            Value = value;
        }

        protected ValueResult(Error error)
            : base(error)
        { }

        public object GetValue()
        {
            if (!Success)
            {
                throw new NullReferenceException("The result is not successful and the value is null.");
            }

            return Value!;
        }

        public bool TryGetValue([MaybeNullWhen(false)] out object value)
        {
            value = Value;
            return Success;
        }

        public static ValueResult Ok(object value)
        {
            return new ValueResult(value);
        }
    }
}
