using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YoutubeDown.Library.Download.EventArg;
using YoutubeExplode.Exceptions;
using YoutubeExplode.Models;

namespace YoutubeDown.Library.Download
{
    public class AudioDownload : PropertyChangedBase, IDownload
    {
        public Video Video { get; private set; }

        public YoutubeClient YoutubeClient { get; private set; }

        public FileSize AudioSize
        {
            get => audioSize;
            set => NotifyPropertyChanged(ref audioSize, value);
        }

        public FileSize VideoSize
        {
            get => videoSize;
            set => NotifyPropertyChanged(ref videoSize, value);
        }

        public FileSize TotalSize
        {
            get => totalSize;
            set => NotifyPropertyChanged(ref totalSize, value);
        }

        public double DownloadPercentage
        {
            get => downloadPercentage;
            set => NotifyPropertyChanged(ref downloadPercentage, value);
        }

        public DownloadStatus Status
        {
            get => status;
            set => NotifyPropertyChanged(ref status, value);
        }

        public CancellationToken CancellationToken => cancellationTokenSource.Token;

        private FileSize audioSize;
        private FileSize videoSize;
        private FileSize totalSize;
        private IProgress<double> progress;
        private DownloadStatus status;
        private double downloadPercentage;
        private CancellationTokenSource cancellationTokenSource;

        public AudioDownload(Video Video, YoutubeClient youtubeClient, CancellationTokenSource CancellationTokenSource)
        {
            this.Video = Video;
            this.YoutubeClient = youtubeClient;
            this.cancellationTokenSource = CancellationTokenSource;
            this.Status = DownloadStatus.None;
            this.progress = new Progress<double>(x => { DownloadPercentage = x; });
        }

        public async Task DownloadAsync()
        {
            try
            {
                this.Status = DownloadStatus.Downloading;
                //await YoutubeClient.DownloadHighestVideo(Audio, progress, DownloadInfo, MuxingStarted, MuxingFinished, CancellationToken);
            }
            catch (TaskCanceledException)
            {
                Status = DownloadStatus.Canceled;
            }
            catch (InvalidOperationException)
            {
                Status = DownloadStatus.MuxingError;
            }
            catch (VideoUnavailableException)
            {
                Status = DownloadStatus.VideoNotAvaible;
            }
            catch (HttpRequestException)
            {
                Status = DownloadStatus.HttpError;
            }
            catch (Exception)
            {
                // TODO: Write logfile
                Status = DownloadStatus.Error;
            }
        }

        private void DownloadInfo(object sender, AudioDownloadInfoArgs args)
        {
            AudioSize = args.AudioSize;
            TotalSize = args.TotalSize;
        }

        private void MuxingStarted(object sender, EventArgs args) => Status = DownloadStatus.Muxing;

        private void MuxingFinished(object sender, EventArgs args) => Status = DownloadStatus.Finished;

        public void CancelDownload()
        {
            if (!cancellationTokenSource.IsCancellationRequested)
                cancellationTokenSource.Cancel();
        }
    }
}
