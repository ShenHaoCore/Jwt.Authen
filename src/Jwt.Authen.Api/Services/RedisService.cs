using StackExchange.Redis;

namespace Jwt.Authen.Api.Services;

/// <summary>
/// 
/// </summary>
public class RedisService
{
    private readonly ConnectionMultiplexer _redis;
    private readonly IDatabase _db;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="configuration"></param>
    public RedisService(IConfiguration configuration)
    {
        _redis = ConnectionMultiplexer.Connect(configuration["Redis:ConnectionString"] ?? string.Empty); // 初始化连接多路复用器
        _db = _redis.GetDatabase(); // 获取数据库实例，可以指定数据库编号，默认为0
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="connectionString"></param>
    public RedisService(string connectionString)
    {
        _redis = ConnectionMultiplexer.Connect(connectionString); // 初始化连接多路复用器
        _db = _redis.GetDatabase(); // 获取数据库实例，可以指定数据库编号，默认为0
    }

    /// <summary>
    /// 设置字符串类型的键值对
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    /// <param name="expiry">过期时间</param>
    /// <returns>是否成功</returns>
    public async Task<bool> SetStringAsync(string key, string value, TimeSpan? expiry = null)
    {
        return await _db.StringSetAsync(key, value, expiry);
    }

    /// <summary>
    /// 获取字符串类型的值
    /// </summary>
    /// <param name="key">键</param>
    /// <returns>对应的值</returns>
    public async Task<string?> GetStringAsync(string key)
    {
        return await _db.StringGetAsync(key);
    }

    /// <summary>
    /// 删除键值对
    /// </summary>
    /// <param name="key">键</param>
    /// <returns>是否成功</returns>
    public async Task<bool> RemoveKeyAsync(string key)
    {
        return await _db.KeyDeleteAsync(key);
    }

    /// <summary>
    /// 确保在适当的时候关闭连接
    /// </summary>
    public void Close()
    {
        _redis.Close();
    }
}
