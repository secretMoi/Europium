namespace Europium.Helpers.Extensions;

public static class StringExtension
{
    public static string RemoveBefore(this string text, string separator, bool includeSeparator = true)
    {
        var lengthToRemoveAfter = includeSeparator ? separator.Length : 0;
        return text.Substring(text.IndexOf(separator, StringComparison.Ordinal) + lengthToRemoveAfter);
    }
    
    public static string RemoveAfter(this string text, string separator)
    {
        return text.Substring(0, text.IndexOf(separator, StringComparison.Ordinal));
    }
    
    public static string RemoveAfterLast(this string text, string separator)
    {
        return text.Substring(0, text.LastIndexOf(separator, StringComparison.Ordinal) + separator.Length);
    }
    
    public static string RemoveBetween(this string text, char first, char last)
    {
        int start = text.LastIndexOf(first);
        int end = text.IndexOf(last, start);
        return text.Remove(start, end - start + 1);
    }
    
    public static string RemoveAllBetween(this string text, char first, char last)
    {
        while (text.Contains(first))
        {
            text = text.RemoveBetween(first, last);
        }

        return text;
    }
    
    public static string GetOnlyNumeric(this string text)
    {
        return new string(text.Where(c => char.IsDigit(c) || c == '.').ToArray());
    }
}