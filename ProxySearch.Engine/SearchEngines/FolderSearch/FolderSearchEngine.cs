using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ProxySearch.Common;
using ProxySearch.Engine.Properties;

namespace ProxySearch.Engine.SearchEngines.FolderSearch
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

        public async Task<Uri> GetNext()
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
            return await Task.Run(() => 
            {
                try
                {
                    return new List<string>(Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories));
                }
                catch(Exception exception)
                {
                    Context.Get<IExceptionLogging>().Write(exception);
                    return new List<string>();
                }
            });
        }
    }
}
