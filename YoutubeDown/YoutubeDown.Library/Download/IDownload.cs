using System.Threading;
using System.Threading.Tasks;

namespace YoutubeDown.Library.Download
{
    public interface IDownload
    {
        CancellationToken CancellationToken { get; }
        double DownloadPercentage { get; set; }
        DownloadStatus Status { get; set; }
        FileSize TotalSize { get; set; }
        YoutubeClient YoutubeClient { get; }

        void CancelDownload();
        Task DownloadAsync();
    }
}