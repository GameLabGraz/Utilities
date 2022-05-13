
public static class StringExtensions
{
    public static string RemoveLast(this string text, string character)
    {
        return text.Length < 1 ? text : text.Remove(text.LastIndexOf(character), character.Length);
    }
}
