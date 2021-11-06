using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NovelApp.Models.BookGwModels;

namespace NovelApp.Services.Book
{
    public interface IBookService
    {
        /// <summary>
        /// Lấy danh sách NovelList
        /// </summary>
        /// <param name="index">ID cuối của dãy</param>
        /// <returns></returns>
        Task<IEnumerable<Novel>> GetNovelList(int index);
        /// <summary>
        /// search danh sách novel
        /// </summary>
        /// <param name="name">tên search</param>
        /// <param name="index">ID cuối cùng của dãy</param>
        /// <returns></returns>
        Task<IEnumerable<Novel>> SearchNovelList(string name, int index);
        /// <summary>
        /// Lấy chi tiết thông tin của novel
        /// </summary>
        /// <param name="id">id của novel</param>
        /// <returns></returns>
        Task<Novel> GetDetailNovel(int id);
        /// <summary>
        /// Lấy thông tin chi tiết tài khoản
        /// </summary>
        /// <param name="email">mail đăng kí</param>
        /// <returns></returns>
        Task<User> GetDetailUser(string email);
        /// <summary>
        /// Mở chapter để đọc
        /// </summary>
        /// <param name="novelId">id novel</param>
        /// <param name="no">số chapter</param>
        /// <param name="email">email người dùng</param>
        /// <returns></returns>
        Task<ResponsePost> UnlockChapter(int novelId, int no, string email);
        /// <summary>
        /// Lấy nội dung của chapter
        /// </summary>
        /// <param name="novelId"></param>
        /// <param name="no"></param>
        /// <returns>Nội dung của chapter</returns>
        Task<Chapter> GetContentChapter(int novelId, int no);
        /// <summary>
        /// Lấy danh sách các chapter của novel
        /// </summary>
        /// <param name="novelId"></param>
        /// <returns></returns>
        Task<TBC> GetTBC(int novelId);
        /// <summary>
        /// Lấy danh sách comment của tài khoản
        /// </summary>
        /// <param name="novelId">id của novel</param>
        /// <returns>danh sách comment</returns>
        Task<IEnumerable<Comment>> GetCommentList(int novelId);
        /// <summary>
        /// Post comment lên bài novel
        /// </summary>
        /// <param name="novelId"></param>
        /// <param name="content">50 - 100 kí tự</param>
        /// <param name="numbstar">1-5 sao</param>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<ResponsePost> PostComment(int novelId, string content, int numbstar, string email);


    }
}
