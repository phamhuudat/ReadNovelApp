using System;
using System.Collections.Generic;
using System.Linq;
using NovelApp.Bussiness;
using NovelApp.Helpers;
using NovelApp.Models.BookGwModels;
using NovelApp.Services.Book;
using NovelApp.Services.DatabaseService;
using Xamarin.Forms;

namespace NovelApp.Bussiness
{
    public class DownloadService: IDownloadService
    {
        public Dictionary<int,DownloadInfo> DicDownloadInfos { get; set; }
        private readonly IDatabaseService _databaseService;
        private readonly IBookService _bookService;
        public DownloadService(IDatabaseService database, IBookService bookService)
        {
            _databaseService = database;
            _bookService = bookService;
            DicDownloadInfos = new Dictionary<int, DownloadInfo>();
        }

        public async void Download(Novel novel, int novelId)
        {
           var tbc =  await _bookService.GetTBC(novelId);
           if(tbc != null && tbc.Chapters != null && tbc.Chapters.Any())
           {
                var lisObj = tbc.Chapters.Where(x => x.Type == 0);
                double unitProcess;
                if (lisObj.Any())
                {
                    var downloadInfo = DicDownloadInfos[novelId];
                    try
                    {
                        downloadInfo.Status = Models.Enums.StatusDownload.Running;
                        unitProcess = Math.Round(100d / lisObj.Count(), MidpointRounding.AwayFromZero);
                        var listchapter = new List<ChapterInfo>();
                        foreach (var obj in lisObj)
                        {
                            var chapter = await _bookService.GetContentChapter(novelId, obj.No);
                            if (downloadInfo.Percent + unitProcess <= 100)
                            {
                                downloadInfo.Percent += unitProcess;
                            }
                            listchapter.Add(NovelConverterHelper.ChapterToConverterChapterInfo(chapter));
                        }
                        await _databaseService.SaveBookInfo(NovelConverterHelper.NovelToConverterBook(novel, 3));
                        await _databaseService.SaveChapterInfo(listchapter);
                        downloadInfo.Status = Models.Enums.StatusDownload.Completed;
                    }
                    catch(Exception e)
                    {
                        downloadInfo.Status = Models.Enums.StatusDownload.Not;
                    }
                    finally
                    {
                        downloadInfo.Percent = 0;
                    }
                }
           }
        }

        public void InstanceDownloadInfo(ref DownloadInfo downloadInfo)
        {
            if (DicDownloadInfos.ContainsKey(downloadInfo.NovelId))
            {
                downloadInfo = DicDownloadInfos[downloadInfo.NovelId];
            }
            else
            {
                DicDownloadInfos.Add(downloadInfo.NovelId, downloadInfo);
            }
        }
    }
}
