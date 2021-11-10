using NovelApp.Models.BookGwModels;
using NovelApp.Services.RequestProvider;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NovelApp.Services.Book
{
    public class BookService : IBookService
    {
        private readonly IRequestProvider _requestProvider;
        public BookService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }
        public async Task<IEnumerable<Comment>> GetCommentList(int novelId)
        {
            var parameters = new List<RequestParameter>() {
                new RequestParameter(name:"novelid",value:novelId)};
            var result = await _requestProvider.Get<IEnumerable<Comment>>("book/comments", parameters);
            if (result != null)
                return result.Data;
            return null;
        }

        public async Task<Chapter> GetContentChapter(int novelId, int no)
        {
            var parameters = new List<RequestParameter>() {
                new RequestParameter(name:"novelid",value:novelId),
                new RequestParameter(name:"no",value:no)};
            var result = await _requestProvider.Get<Chapter>("book/chapter", parameters);
            if (result != null)
                return result.Data;
            return null;
        }

        public async Task<Novel> GetDetailNovel(int id)
        {
            var parameters = new List<RequestParameter>() {
                new RequestParameter(name:"id",value:id)};
            var result = await _requestProvider.Get<Novel>("book/novelinfo", parameters);
            if (result != null)
                return result.Data;
            return null;
        }

        public async Task<User> GetDetailUser(string email)
        {
            var parameters = new List<RequestParameter>() {
                new RequestParameter(name:"email",value:email)};
            var result = await _requestProvider.Get<User>("book/user/info", parameters);
            if (result != null)
                return result.Data;
            return null;
        }

        public async Task<IEnumerable<Novel>> GetNovelList(int index)
        {
            var parameters = new List<RequestParameter>() {
                new RequestParameter(name:"index",value:index)};
            var result = await _requestProvider.Get<IEnumerable<Novel>>("book/list", parameters);
            if (result != null)
                return result.Data;
            return null;
        }

        public async Task<TBC> GetTBC(int novelId)
        {
            var parameters = new List<RequestParameter>() {
                new RequestParameter(name:"novelid",value:novelId)};
            var result = await _requestProvider.Get<TBC>("book/tbc", parameters);
            if (result != null)
                return result.Data;
            return null;
        }

        public async Task<ResponsePost> PostComment(int novelId, string content, int numbstar, string email)
        {
            var parameters = new List<RequestParameter>() {
                new RequestParameter(name:"novelid",value:novelId),
                new RequestParameter(name:"content",value:content),
                new RequestParameter(name:"star",value:numbstar),
                new RequestParameter(name:"email",value:email)};
            var result = await _requestProvider.Get<ResponsePost>("book/postcomment", parameters);
            if (result != null)
                return result.Data;
            return null;
        }

        public async Task<IEnumerable<Novel>> SearchNovelList(string name, int index)
        {
            var parameters = new List<RequestParameter>() {
                new RequestParameter(name:"name",value:name),
                new RequestParameter(name:"index",value:index)};
            var result = await _requestProvider.Get<IEnumerable<Novel>>("book/search", parameters);
            if (result != null)
                return result.Data;
            return null;
        }

        public async Task<ResponsePost> UnlockChapter(int novelId, int no, string email)
        {
            var parameters = new List<RequestParameter>() {
                new RequestParameter(name:"novelid",value:novelId),
                new RequestParameter(name:"no",value:no),
                new RequestParameter(name:"email",value:email)};
            var result = await _requestProvider.Post<ResponsePost>("book/chapter/unlock", parameters);
            if (result != null)
                return result.Data;
            return null;
        }
    }
}
