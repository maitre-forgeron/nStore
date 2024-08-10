namespace NStore.Shared.Extensions
{
    public static class StringExtensions
    {
        public static bool IsUuid(this string input) => Guid.TryParseExact(input, "D", out Guid result);
    }
}
