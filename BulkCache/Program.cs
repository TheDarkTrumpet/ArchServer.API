namespace BulkCache
{
    class Program
    {
        static void Main(string[] args)
        {
            lib.BulkCache cache = new lib.BulkCache();
            cache.CacheAll();
        }
    }
}
