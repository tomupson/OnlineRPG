public static class StringExtensions
{
    public static string ToUpperFirstChar(this string s)
    {
        char[] chars = s.ToCharArray();
        chars[0] = char.ToUpper(chars[0]);
        return chars.ToString();
    }
}