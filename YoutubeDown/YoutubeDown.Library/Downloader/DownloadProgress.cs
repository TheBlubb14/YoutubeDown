using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeDown.Library.Downloader
{
    public struct DownloadProgress
    {
        public long ReadBytes { get; private set; }
        public long TotalBytes { get; private set; }
        public long BytesPerSecond { get; private set; }
        public string Speed => $"{BytesPerSecond.NormalizeFileSize()}/s";
        public double Percentage => (double)ReadBytes / TotalBytes;

        public DownloadProgress(long readBytes, long totalBytes, long bytesPerSecond)
        {
            ReadBytes = readBytes;
            TotalBytes = totalBytes;
            BytesPerSecond = bytesPerSecond;
        }
    }
}
