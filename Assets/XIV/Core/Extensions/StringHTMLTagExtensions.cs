using System.Text;

namespace XIV.Core.Extensions
{
    public static class StringHTMLTagExtensions
    {
		static StringBuilder stringBuilder = new StringBuilder();

		const string TAG_BOLD = "b";
		const string TAG_COLOR = "color";

		static string Format(string tag, string value)
        {
            stringBuilder.Clear();
            stringBuilder.Append("<" + tag + ">");
            stringBuilder.Append(value);
            stringBuilder.Append("</" + tag + ">");

			return stringBuilder.ToString();
        }

		static string Format(string tag, string tagValue, string value)
        {
			if (string.IsNullOrEmpty(tagValue)) return Format(tag, value);

			return Format(tag + "=" + tagValue, value);
        }

        public static string Bold(this string value)
		{
			return Format(TAG_BOLD, value);
        }

        public static string Red(this string value)
		{
			return Format(TAG_COLOR, "red", value);
        }

        public static string Green(this string value)
        {
            return Format(TAG_COLOR, "green", value);
        }

        public static string Blue(this string value)
        {
            return Format(TAG_COLOR, "blue", value);
        }

        public static string Yellow(this string value)
		{
			return Format(TAG_COLOR, "yellow", value);
        }
	}
}