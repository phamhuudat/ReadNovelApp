using NovelApp.Models.BookGwModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NovelApp.Services.RequestProvider
{
    public interface IRequestProvider
    {
        Task<ResponseObject<TData>> Get<TData>(string uri, IReadOnlyCollection<RequestParameter> parameters);
        Task<ResponseObject<TData>> Post<TData>(string uri, IReadOnlyCollection<RequestParameter> parameters);
        Task<ResponseObject<TData>> Delete<TData>(string uri, IReadOnlyCollection<RequestParameter> parameters);
        Task<ResponseObject<TData>> Put<TData>(string uri, IReadOnlyCollection<RequestParameter> parameters);
    }
}
