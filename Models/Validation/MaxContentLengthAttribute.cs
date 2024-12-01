using System.ComponentModel.DataAnnotations;

namespace YssWebstoreApi.Models.Validation
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class MaxContentLengthAttribute : ValidationAttribute
    {
        public long MaxLength { get; }

        public MaxContentLengthAttribute(long maxLength)
        {
            MaxLength = maxLength;
        }

        public override bool IsValid(object? value)
        {
            if (value is IFormFile file)
            {
                return file.Length <= MaxLength;
            }

            return false;
        }
    }
}
