namespace Jlox
{
    public static class StringExtensions
    {
        /// <summary>
        /// Mimics Java method, where the 2nd parameter is the end index, not the length of the substring.
        /// </summary>
        public static string JavaSubstring(this string s, int start, int end)
        {
            return s.Substring(start, end - start);
        }
    }
}
