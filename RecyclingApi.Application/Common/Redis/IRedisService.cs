namespace RecyclingApi.Application.Common.Redis;

public interface IRedisService
{
    // String operations
    Task<bool> SetStringAsync(string key, string value, TimeSpan? expiry = null);
    Task<string?> GetStringAsync(string key);
    Task<bool> DeleteAsync(string key);
    Task<bool> ExistsAsync(string key);
    Task<bool> ExpireAsync(string key, TimeSpan expiry);

    // Hash operations
    Task<bool> HashSetAsync(string key, string field, string value);
    Task<string?> HashGetAsync(string key, string field);
    Task<Dictionary<string, string>> HashGetAllAsync(string key);
    Task<bool> HashDeleteAsync(string key, string field);
    Task<bool> HashExistsAsync(string key, string field);

    // List operations
    Task<long> ListPushAsync(string key, string value);
    Task<string?> ListPopAsync(string key);
    Task<List<string>> ListRangeAsync(string key, long start = 0, long stop = -1);
    Task<long> ListLengthAsync(string key);

    // Set operations
    Task<bool> SetAddAsync(string key, string value);
    Task<bool> SetRemoveAsync(string key, string value);
    Task<List<string>> SetMembersAsync(string key);
    Task<bool> SetContainsAsync(string key, string value);

    // Sorted Set operations
    Task<bool> SortedSetAddAsync(string key, string value, double score);
    Task<List<string>> SortedSetRangeByRankAsync(string key, long start = 0, long stop = -1);
    Task<List<string>> SortedSetRangeByScoreAsync(string key, double min = double.NegativeInfinity, double max = double.PositiveInfinity);
    Task<bool> SortedSetRemoveAsync(string key, string value);

    // JSON operations
    Task<bool> SetObjectAsync<T>(string key, T value, TimeSpan? expiry = null);
    Task<T?> GetObjectAsync<T>(string key) where T : class;

    // Pub/Sub operations
    Task PublishAsync(string channel, string message);
    Task SubscribeAsync(string channel, Action<string, string> handler);

    // Lock operations
    Task<bool> AcquireLockAsync(string key, string value, TimeSpan expiry);
    Task<bool> ReleaseLockAsync(string key, string value);

    // Batch operations
    Task<bool> SetMultipleAsync(Dictionary<string, string> keyValues, TimeSpan? expiry = null);
    Task<Dictionary<string, string?>> GetMultipleAsync(IEnumerable<string> keys);
} 