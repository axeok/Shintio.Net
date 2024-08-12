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
    
    public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> enumerable)
    {
        return enumerable.SelectMany(i => i);
    }
}