using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NovelApp.Models.BookGwModels;
using NovelApp.Models.Enums;

namespace NovelApp.Services.DatabaseService
{
    public interface IDatabaseService
    {
        Task<BookInfo> GetBookInfo(int novelId);
        /// <summary>
        /// Lưu vị trí chapter đang đọc
        /// </summary>
        /// <param name="chapter"></param>
        /// <param name="no"></param>
        /// <returns></returns>
        Task<bool> SaveReadStatus(int chapter, int no);
        /// <summary>
        /// Lưu thông tin sách
        /// </summary>
        /// <param name="bookInfo"></param>
        /// <returns></returns>
        Task<StatusEnum> SaveBookInfo(BookInfo bookInfo);
        /// <summary>
        /// Lưu thông tin chapter
        /// </summary>
        /// <param name="bookInfo"></param>
        /// <returns></returns>
        Task<bool> SaveChapterInfo(List<ChapterInfo> bookInfo);
        /// <summary>
        /// Danh sách book đang đọc 
        /// </summary>
        /// <returns></returns>
        Task<List<BookInfo>> GetRecentlyBookInfos();
        /// <summary>
        /// danh sách book đang theo dõi
        /// </summary>
        /// <returns></returns>
        Task<List<BookInfo>> GetFollowingBookInfos();
        /// <summary>
        /// Danh sách book đã download
        /// </summary>
        /// <returns></returns>
        Task<List<BookInfo>> GetDownloadBookInfos();
        /// <summary>
        /// Lấy danh sách chapter theo chương
        /// </summary>
        /// <param name="no">id book</param>
        /// <returns></returns>
        Task<List<ChapterInfo>> GetChapterInfos(int no);
        /// <summary>
        /// Xóa danh sách trong bảng
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        Task<bool> RemoveBook(int no);
    }
}
