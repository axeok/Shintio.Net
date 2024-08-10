namespace Shintio.Essentials.Extensions;

public static class EnumerableExtensions
{
    public static async Task WriteTo(this IAsyncEnumerable<string> source, Action<string> action)
    {
        await foreach (var item in source)
        {
            action(item);
        }
    }
}