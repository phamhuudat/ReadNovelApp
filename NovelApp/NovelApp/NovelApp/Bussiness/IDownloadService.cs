using System;
using NovelApp.Models.BookGwModels;

namespace NovelApp.Bussiness
{
    public interface IDownloadService
    {
        void InstanceDownloadInfo(ref DownloadInfo downloadInfo);
        void Download(Novel novel, int novelId);
    }
}
