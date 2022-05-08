using System;
using NovelApp.Models.Enums;
using Prism.Mvvm;

namespace NovelApp.Models.BookGwModels
{
    public class DownloadInfo : BindableBase
    {
        private double percent;
        private StatusDownload status;
        public int NovelId { get; set; }
        public StatusDownload Status { get => status; set => SetProperty(ref status, value); }
        public double Percent { get => percent; set => SetProperty(ref percent, value); }
        public DownloadInfo(int novelId)
        {
            NovelId = novelId;
        }
    }
}
