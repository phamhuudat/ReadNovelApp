using System;
using System.Threading.Tasks;

namespace NovelApp.Services.CacheService
{
    public class CacheService:ICacheService
    {
        public CacheService()
        {
        }

        public Task<string> GetCache(string key)
        {
            throw new NotImplementedException();
        }

        public Task SaveCache(string key, string value)
        {
            throw new NotImplementedException();
        }
    }
}
