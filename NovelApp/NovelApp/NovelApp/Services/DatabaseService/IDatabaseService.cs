using System;
using System.Collections.Generic;
using System.Text;

namespace NovelApp.Services.DatabaseService
{
    public interface IDatabaseService
    {
        Task<bool> SaveBookInfo()
    }
}
