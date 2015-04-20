using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Engine.Properties;

namespace ProxySearch.Engine.SearchEngines
{
    public class FolderSearchEngine : ISearchEngine
    {
        private string folderPath;
        private List<string> files = null;

        public string Status
        {
            get
            {
                return string.Format(Resources.SearchingInFolderFormat, folderPath);
            }
        }

        public FolderSearchEngine(string folderPath)
        {
            this.folderPath = folderPath;
        }

        public async Task<Uri> GetNext(CancellationTokenSource cancellationTokenSource)
        {
            if (files == null)
                files = await GetFilesAsync();

            if (!files.Any())
                return null;

            string result = files[0];
            files.RemoveAt(0);

            return await Task.FromResult<Uri>(new Uri(result));
        }

        private async Task<List<string>> GetFilesAsync()
        {
            try
            {
                return await Task.FromResult(Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories).ToList());
            }
            catch(DirectoryNotFoundException)
            {
            }

            return await Task.FromResult(new List<string>() { });
        }
    }
}