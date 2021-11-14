using System;
using System.Threading.Tasks;

namespace NovelApp.Services.CacheService
{
    public interface ICacheService
    {
        /// <summary>
        /// Save Cache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task SaveCache(string key, string value);
        /// <summary>
        /// GetCache value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<string> GetCache(string key);
    }
}
