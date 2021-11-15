using NovelApp.Configurations;
using NovelApp.Models.Caches;
using Realms;
using System;
using System.Threading.Tasks;

namespace NovelApp.Services.CacheService
{
    public class CacheService : ICacheService
    {
        private ulong _schemaVersion = 1;
        private RealmConfiguration _configuration;
        public CacheService()
        {
            _configuration = new RealmConfiguration(AppConstants.CacheParameter.DatabaseLocalRealm)
            {
                // khi cần thay đổi một object realm hay thay đổi cấu trúc trong model để lưu cache vào realm thì chỉ cần tăng version lên 1
                SchemaVersion = _schemaVersion,
                MigrationCallback = (migration, version) =>
                {
                    // khi tăng version lên thì xóa toàn bộ dữ liệu trong version cũ đi
                    migration.OldRealm.WriteAsync(realm => realm.RemoveAll());
                },
            };
        }
        private Realm _getInstance()
        {
            return Realm.GetInstance(_configuration);
        }
        public string GetCache(string key)
        {
            using (var realm = _getInstance())
            {
                var cache = realm.Find<CacheApp>(key);
                if (cache != null)
                {
                    return cache.Value;
                }
                else
                    return null;
            }
        }

        public void SaveCache(string key, string value)
        {
            using (var realm = _getInstance())
            {
                var cache = realm.Find<CacheApp>(key);
                if (cache != null)
                {
                    using (var tran = realm.BeginWrite())
                    {
                        cache.Value = value;
                        tran.Commit();
                    }
                }
                else
                {
                    realm.Write(() =>
                    {
                        realm.Add(new CacheApp() { Key=key,Value = value});
                    });
                }    
            }
        }
    }
}
