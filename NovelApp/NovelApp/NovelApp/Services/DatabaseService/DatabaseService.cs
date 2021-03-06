using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NovelApp.Configurations;
using NovelApp.Models;
using NovelApp.Models.BookGwModels;
using NovelApp.Models.Enums;
using Realms;
using Xamarin.Essentials;

namespace NovelApp.Services.DatabaseService
{
    public class DatabaseService : IDatabaseService
    {
        private ulong _schemaVersion = 1;
        private RealmConfiguration _configuration;
        public DatabaseService()
        {
            _configuration = new RealmConfiguration(AppConstants.AppParameters.DatabaseNovel)
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
        public async Task<ChapterInfo> GetChapterInfos(int novelId,int no)
        {
            var realm = _getInstance();
            var listChapters = realm.All<ChapterInfo>().Where(x => x.No == no&& x.NovelID == novelId).FirstOrDefault();
            return listChapters;
        }

        public async Task<List<BookInfo>> GetDownloadBookInfos()
        {
            var realm = _getInstance();
            var listBook = realm.All<BookInfo>().Where(x => x.ListType == 3);
            return listBook?.ToList().OrderByDescending(x => x.LatestReadTime).ToList();
        }

        public async Task<List<BookInfo>> GetFollowingBookInfos()
        {
            var realm = _getInstance();
            var listBook = realm.All<BookInfo>().Where(x => x.ListType == 2 || x.ListType ==3);
            return listBook?.ToList().OrderByDescending(x => x.LatestReadTime).ToList();
        }

        public async Task<List<BookInfo>> GetRecentlyBookInfos()
        {
            var realm = _getInstance();
            var listBook = realm.All<BookInfo>();   
            return listBook?.ToList().OrderByDescending(x => x.LatestReadTime).ToList();
        }

        public async Task<bool> RemoveBook(int no)
        {
            using (var realm = _getInstance())
            {
                var bookObject = realm.Find<BookInfo>(no);
                if (bookObject != null)
                {
                    var chapters = realm.All<ChapterInfo>();
                    if(chapters!=null&& chapters.Any())
                    {
                        chapters = chapters.Where(x => x.NovelID == no);
                    }
                    using (var transaction = realm.BeginWrite())
                    {
                        realm.Remove(bookObject);
                        if (chapters != null && chapters.Any())
                            realm.RemoveRange(chapters);
                        transaction.Commit();
                    }
                }
                
            }
            return true;
        } 

        public async Task<StatusEnum> SaveBookInfo(BookInfo bookInfo)
        {
            try
            {
                using (var realm = _getInstance())
                {   
                    var obj = realm.Find<BookInfo>(bookInfo.ID);
                    int listType = bookInfo.ListType;
                    if (obj == null)
                    {
                        await realm.WriteAsync((x) =>
                        {
                            x.Add(bookInfo);
                        });
                        obj = bookInfo;
                    }
                    else
                    {
                        var tran = realm.BeginWrite();
                        {
                            listType = obj.ListType = bookInfo.ListType > obj.ListType? bookInfo.ListType:obj.ListType;
                            obj.LastChapter = bookInfo.LastChapter;
                            obj.LastReadTime = bookInfo.LastReadTime;
                            tran.Commit();
                        }
                    }

                    var listObj = realm.All<BookInfo>().Where(x => x.ListType == listType);
                    if ((listType == 1 && listObj != null && listObj.Any() && listObj.Count() > 10) ||
                        (listType == 2 && listObj != null && listObj.Any() && listObj.Count() > 50))
                    {
                        listObj.ToList().OrderByDescending(x => x.LatestReadTime);
                        var lastObj = listObj.Last();
                        using (var transaction = realm.BeginWrite())
                        {
                            realm.Remove(lastObj);
                            transaction.Commit();
                        }
                    }


                }
            }
            catch(Exception e)
            {
                return StatusEnum.Error;
            }
            
            return StatusEnum.Success;
        }

        public async Task<bool> SaveChapterInfo(List<ChapterInfo> chapter)
        {

            using (var realm = _getInstance())
            {
                    await realm.WriteAsync((x) =>
                    {
                        chapter.ForEach(obj =>
                        {
                            var item =  x.Find<ChapterInfo>(obj.Id);
                            if(item != null)
                            {
                                x.Remove(item);
                                
                            }
                            x.Add(obj);
                        });
                        
                    });
            }
            return true;
        }

        public async Task<bool> SaveReadStatus(int chapter, int no)
        {
            using (var realm = _getInstance())
            {
                var book = realm.Find<BookInfo>(no);
                if (book != null)
                {
                    using (var tran = realm.BeginWrite())
                    {
                        book.ReadState = chapter;
                        book.LastReadTime = DateTime.Now.ToString();
                        tran.Commit();
                    }
                }
                
            }
            return true;
        }

        public async  Task<BookInfo> GetBookInfo(int novelId)
        {
            //using
            var realm = _getInstance();
            {
                var book = realm.Find<BookInfo>(novelId);
                return book;
            }
        }

        public async Task<bool> SaveFilters(List<FilterInfo> filters)
        {
            var realm = _getInstance();
            {
                await realm.WriteAsync((x) =>
                {
                    filters.ForEach(obj =>
                    {
                        var item = x.Find<FilterInfo>(obj.Type);
                        if (item != null)
                        {
                            x.Remove(item);
                        }
                        x.Add(obj);
                    });

                });
            }
            return true;
        }

        public async Task<List<FilterInfo>> GetFilters()
        {
            var realm = _getInstance();
            {
                var list = realm.All<FilterInfo>();
                return (list!=null && list.Any())? list.ToList(): null;
            }
        }
    }
}
