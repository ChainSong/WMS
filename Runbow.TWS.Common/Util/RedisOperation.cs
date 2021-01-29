using CSRedis;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Runbow.TWS.Common
{
    public static class RedisOperation
    {
       
        public static CSRedisClient cs = new CSRedisClient(RedisConstants.RedisPath);
        public static bool SetList(string key, object value, int expireSeconds = -1, RedisExistence? exists = null)
        {
            key = RedisConstants.RedisPrefix + key;
            object redisValule = cs.SerializeRedisValueInternal(value);
            if (expireSeconds <= 0 && exists == null) return cs.ExecuteScalar(key, (c, k) => c.Value.Set(k, redisValule)) == "OK";
            return false;
        }
        public static T GetList<T>(string key)
        {
            key = RedisConstants.RedisPrefix + key;
            return cs.DeserializeRedisValueInternal<T>(cs.ExecuteScalar(key, (c, k) => c.Value.GetBytes(k)));
        }
       
        public static long Del(params string[] key)
        {
            for (int j=0;j<key.Length;j++)
            {
                key[j] = RedisConstants.RedisPrefix + key[j];
            }
            return cs.ExecuteNonQuery(key, (c, k) => c.Value.Del(k));
        }
        public static bool Exists(string key)
        {
            key = RedisConstants.RedisPrefix + key;
            return cs.ExecuteScalar(key, (c, k) => c.Value.Exists(k));
        }
        public static RedisScan<string> Scan(int cursor, string pattern = null, int? count = null)
        {
            return cs.Scan(cursor, pattern, count);
        }
        public static string[] Like(string  pattern)
        {
            pattern = RedisConstants.RedisPrefix + pattern;
            return cs.Keys(pattern);
        }
        /// <summary>
        /// 开启分布式锁，若超时返回null
        /// </summary>
        /// <param name="name">锁名称</param>
        /// <param name="timeoutSeconds">超时（秒）</param>
        /// <returns></returns>
        public static CSRedisClientLock Lock(string name, int timeoutSeconds)
        {
            name = $"CSRedisClientLock:{name}";
            var startTime = DateTime.Now;
            while (DateTime.Now.Subtract(startTime).TotalSeconds < timeoutSeconds)
            {
                if (cs.SetNx(name, "1") == true)
                {
                    cs.Expire(name, TimeSpan.FromSeconds(timeoutSeconds));
                    return new CSRedisClientLock { Name = name, _client = cs };
                }
                Thread.CurrentThread.Join(3);
            }
            return null;
        }
    }
}