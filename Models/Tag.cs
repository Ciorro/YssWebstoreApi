using System.Diagnostics.CodeAnalysis;

namespace YssWebstoreApi.Models
{
    public record Tag : IParsable<Tag>
    {
        public string Type { get; }
        public string Name { get; }

        public Tag(string type, string name)
        {
            Type = type;
            Name = name;
        }

        public override string ToString()
        {
            return $"{Type}-{Name}";
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

            string type = "";
            string name = "";

            int dashIndex = s.IndexOf('-');
            if (dashIndex == -1)
            {
                type = "tag";
                name = s;
            }
            else
            {
                type = s[..dashIndex];
                name = s[(dashIndex + 1)..];
            }

            if (string.IsNullOrWhiteSpace(type) || string.IsNullOrWhiteSpace(name))
            {
                result = null;
                return false;
            }

            result = new Tag(type, name);
            return true;
        }
    }
}
