using System;

namespace Api.Utils
{
    public static class Regex
    {
        public static bool IsValid(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                return false;
            }

            try
            {
                _ = System.Text.RegularExpressions.Regex.Match(string.Empty, pattern);
            }
            catch (Exception ex)
                when (ex is ArgumentException)
            {
                return false;
            }

            return true;
        }
    }
}