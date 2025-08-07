using System.Diagnostics.CodeAnalysis;

namespace YssWebstoreApi.Models
{
    public record Tag : IParsable<Tag>
    {
        public string Group { get; }
        public string Value { get; }

        public Tag(string group, string value)
        {
            Group = group;
            Value = value;
        }

        public override string ToString()
        {
            return $"{Group}-{Value}";
        }

        public static Tag Parse(string s)
            => Parse(s, null);

        public static bool TryParse([NotNullWhen(true)] string? s, [MaybeNullWhen(false)] out Tag result)
            => TryParse(s, null, out result);

        public static Tag Parse(string s, IFormatProvider? provider)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(s);

            if (!TryParse(s, provider, out var tag))
            {
                throw new FormatException();
            }

            return tag;
        }

        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Tag result)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                result = null;
                return false;
            }

            if (!s.All(c => char.IsAsciiLetterOrDigit(c) || c == '-'))
            {
                result = null;
                return false;
            }

            string group = "";
            string value = "";

            int dashIndex = s.IndexOf('-');
            if (dashIndex == -1)
            {
                group = "tag";
                value = s;
            }
            else
            {
                group = s[..dashIndex];
                value = s[(dashIndex + 1)..];
            }

            if (string.IsNullOrWhiteSpace(group) || string.IsNullOrWhiteSpace(value))
            {
                result = null;
                return false;
            }

            result = new Tag(group, value);
            return true;
        }
    }
}
