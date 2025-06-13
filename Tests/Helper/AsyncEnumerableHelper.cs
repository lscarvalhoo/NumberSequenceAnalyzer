namespace Tests.Helper
{
    public static class AsyncEnumerableHelper
    {
        public static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(this IEnumerable<T> source)
        {
            foreach (var item in source)
            {
                yield return await Task.FromResult(item);
            }
        }
    }
}