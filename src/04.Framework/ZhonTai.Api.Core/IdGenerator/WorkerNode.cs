using FreeRedis;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ZhonTai.Plugin.Cache;
using ZhonTai.Utils.Extensions;

namespace ZhonTai.Api.Core.IdGenerator;

public sealed class WorkerNode
{
    private readonly ILogger<WorkerNode> _logger;
    private readonly IRedisClient _redisProvider;

    public WorkerNode(ILogger<WorkerNode> logger
       , IRedisClient redisProvider)
    {
        _redisProvider = redisProvider;
        _logger = logger;
    }

    internal async Task InitWorkerNodesAsync(string serviceName)
    {
        var workerIdSortedSetCacheKey = GetWorkerIdCacheKey(serviceName);

        if (!_redisProvider.Exists(workerIdSortedSetCacheKey))
        {
            _logger.LogInformation($"Starting {nameof(InitWorkerNodesAsync)}:{0}", workerIdSortedSetCacheKey);

            using var lockController = _redisProvider.Lock(workerIdSortedSetCacheKey, 5);
            if (lockController == null)
            {
                await Task.Delay(300);
                await InitWorkerNodesAsync(serviceName);
            }

            long count = 0;
            try
            {
                var set = new Dictionary<long, double>();
                for (long index = 0; index <= IdHelper.MaxWorkerId; index++)
                {
                    set.Add(index, DateTime.Now.ToTimestamp(true));
                }
                
                var scoreMembers = set.Select(x => new ZMember(x.Key.ToString(), x.Value.ToDecimal())).ToArray();
                count = await _redisProvider.ZAddAsync(workerIdSortedSetCacheKey, scoreMembers);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                lockController.Unlock();
            }

            _logger.LogInformation($"Finlished {nameof(InitWorkerNodesAsync)}:{0}:{1}", workerIdSortedSetCacheKey, count);
        }
        else
            _logger.LogInformation("Exists WorkerNodes:{0}", workerIdSortedSetCacheKey);
    }

    internal async Task<long> GetWorkerIdAsync(string serviceName)
    {
        var workerIdSortedSetCacheKey = GetWorkerIdCacheKey(serviceName);

        var script = @"local workerids = redis.call('ZRANGE', KEYS[1], ARGV[1], ARGV[2])
                    redis.call('ZADD', KEYS[1], ARGV[3], workerids[1])
                    return workerids[1]";
        var keys = new string[] { workerIdSortedSetCacheKey };
        var arguments = new object[] { 0, 0, DateTime.Now.ToTimestamp(true) };
        var luaResult = await _redisProvider.EvalAsync(script, keys, arguments);
        var workerId = luaResult.ToLong();

        _logger.LogInformation("Get WorkerNodes:{0}", workerId);

        return workerId;
    }

    internal async Task RefreshWorkerIdScoreAsync(string serviceName, long workerId, double? workerIdScore = null)
    {
        if (workerId < 0 || workerId > IdHelper.MaxWorkerId)
            throw new Exception(string.Format("worker Id can't be greater than {0} or less than 0", IdHelper.MaxWorkerId));

        var workerIdSortedSetCacheKey = GetWorkerIdCacheKey(serviceName);
        var score = workerIdScore == null ? DateTime.Now.ToTimestamp(true) : workerIdScore.Value;

        var redisClient = _redisProvider;
        if (workerIdScore != null)
        {
            var cacheConfig = AppInfo.GetRequiredService<CacheConfig>();
            redisClient = new RedisClient(cacheConfig.ConnectionString)
            {
                Serialize = JsonConvert.SerializeObject,
                Deserialize = JsonConvert.DeserializeObject
            };
        }
        await redisClient.ZAddAsync(workerIdSortedSetCacheKey, new ZMember[] { new ZMember(workerId.ToString(), score.ToDecimal()) });

        _logger.LogDebug("Refresh WorkerNodes:{0}:{1}", workerId, score);
    }

    internal static string GetWorkerIdCacheKey(string serviceName) => $"zhontai:workerids:{serviceName}";
}
