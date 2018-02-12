using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YoutubeExplode.Models;

namespace YoutubeDown.Library.Download
{
    public class VideoDownload
    {
        public event EventHandler StatusChanged;

        public DownloadStatus Status
        {
            get => status;
            set
            {
                status = value;
                StatusChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private DownloadStatus status;
        private Video video;
        private CancellationToken cancellationToken;

        public VideoDownload(Video Video, CancellationToken CancellationToken)
        {
            this.video = Video;
            this.cancellationToken = CancellationToken;
            this.Status = DownloadStatus.Queued;
        }

        public async Task DownloadVideoAsync()
        {
        }

        public void CancelDownload()
        {

        }
    }
}
