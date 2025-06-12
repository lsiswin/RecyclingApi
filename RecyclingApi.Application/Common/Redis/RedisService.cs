using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace RecyclingApi.Application.Common.Redis;

public class RedisService : IRedisService
{
    private readonly IDatabase _database;
    private readonly ISubscriber _subscriber;
    private readonly ILogger<RedisService> _logger;

    public RedisService(IConnectionMultiplexer redis, ILogger<RedisService> logger)
    {
        _database = redis.GetDatabase();
        _subscriber = redis.GetSubscriber();
        _logger = logger;
    }

    #region String Operations

    public async Task<bool> SetStringAsync(string key, string value, TimeSpan? expiry = null)
    {
        try
        {
            return await _database.StringSetAsync(key, value, expiry);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error setting string value for key {Key}", key);
            return false;
        }
    }

    public async Task<string?> GetStringAsync(string key)
    {
        try
        {
            var value = await _database.StringGetAsync(key);
            return value.HasValue ? value.ToString() : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting string value for key {Key}", key);
            return null;
        }
    }

    public async Task<bool> DeleteAsync(string key) => await _database.KeyDeleteAsync(key);
    public async Task<bool> ExistsAsync(string key) => await _database.KeyExistsAsync(key);
    public async Task<bool> ExpireAsync(string key, TimeSpan expiry) => await _database.KeyExpireAsync(key, expiry);

    #endregion

    #region Hash Operations

    public async Task<bool> HashSetAsync(string key, string field, string value) => await _database.HashSetAsync(key, field, value);
    public async Task<string?> HashGetAsync(string key, string field) => await _database.HashGetAsync(key, field);
    public async Task<Dictionary<string, string>> HashGetAllAsync(string key) => (await _database.HashGetAllAsync(key)).ToDictionary(kv => kv.Name.ToString(), kv => kv.Value.ToString());
    public async Task<bool> HashDeleteAsync(string key, string field) => await _database.HashDeleteAsync(key, field);
    public async Task<bool> HashExistsAsync(string key, string field) => await _database.HashExistsAsync(key, field);

    #endregion

    #region List Operations

    public async Task<long> ListPushAsync(string key, string value) => await _database.ListLeftPushAsync(key, value);
    public async Task<string?> ListPopAsync(string key) => await _database.ListLeftPopAsync(key);
    public async Task<List<string>> ListRangeAsync(string key, long start = 0, long stop = -1) => (await _database.ListRangeAsync(key, start, stop)).Select(v => v.ToString()).ToList();
    public async Task<long> ListLengthAsync(string key) => await _database.ListLengthAsync(key);

    #endregion

    #region Set Operations

    public async Task<bool> SetAddAsync(string key, string value) => await _database.SetAddAsync(key, value);
    public async Task<bool> SetRemoveAsync(string key, string value) => await _database.SetRemoveAsync(key, value);
    public async Task<List<string>> SetMembersAsync(string key) => (await _database.SetMembersAsync(key)).Select(v => v.ToString()).ToList();
    public async Task<bool> SetContainsAsync(string key, string value) => await _database.SetContainsAsync(key, value);

    #endregion

    #region Sorted Set Operations

    public async Task<bool> SortedSetAddAsync(string key, string value, double score) => await _database.SortedSetAddAsync(key, value, score);
    public async Task<List<string>> SortedSetRangeByRankAsync(string key, long start = 0, long stop = -1) => (await _database.SortedSetRangeByRankAsync(key, start, stop)).Select(v => v.ToString()).ToList();
    public async Task<List<string>> SortedSetRangeByScoreAsync(string key, double min = double.NegativeInfinity, double max = double.PositiveInfinity) => (await _database.SortedSetRangeByScoreAsync(key, min, max)).Select(v => v.ToString()).ToList();
    public async Task<bool> SortedSetRemoveAsync(string key, string value) => await _database.SortedSetRemoveAsync(key, value);

    #endregion

    #region JSON Operations

    public async Task<bool> SetObjectAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var json = JsonConvert.SerializeObject(value);
        return await _database.StringSetAsync(key, json, expiry);
    }

    public async Task<T?> GetObjectAsync<T>(string key) where T : class
    {
        var json = await _database.StringGetAsync(key);
        return json.HasValue ? JsonConvert.DeserializeObject<T>(json!) : null;
    }

    #endregion

    #region Pub/Sub Operations

    public async Task PublishAsync(string channel, string message) => await _subscriber.PublishAsync(channel, message);
    public async Task SubscribeAsync(string channel, Action<string, string> handler) => await _subscriber.SubscribeAsync(channel, (ch, msg) => handler(ch!, msg!));

    #endregion

    #region Lock Operations

    public async Task<bool> AcquireLockAsync(string key, string value, TimeSpan expiry) => await _database.StringSetAsync($"lock:{key}", value, expiry, When.NotExists);

    public async Task<bool> ReleaseLockAsync(string key, string value)
    {
        const string script = @"if redis.call('get', KEYS[1]) == ARGV[1] then return redis.call('del', KEYS[1]) else return 0 end";
        var result = await _database.ScriptEvaluateAsync(script, new RedisKey[] { $"lock:{key}" }, new RedisValue[] { value });
        return result.ToString() == "1";
    }

    #endregion

    #region Batch Operations

    public async Task<bool> SetMultipleAsync(Dictionary<string, string> keyValues, TimeSpan? expiry = null)
    {
        var batch = _database.CreateBatch();
        var tasks = keyValues.Select(kv => batch.StringSetAsync(kv.Key, kv.Value, expiry)).ToArray();
        batch.Execute();
        var results = await Task.WhenAll(tasks);
        return results.All(r => r);
    }

    public async Task<Dictionary<string, string?>> GetMultipleAsync(IEnumerable<string> keys)
    {
        var keyArray = keys.ToArray();
        var values = await _database.StringGetAsync(keyArray.Select(k => (RedisKey)k).ToArray());
        var result = new Dictionary<string, string?>();
        for (int i = 0; i < keyArray.Length; i++)
        {
            result[keyArray[i]] = values[i].HasValue ? values[i].ToString() : null;
        }
        return result;
    }

    #endregion
} 