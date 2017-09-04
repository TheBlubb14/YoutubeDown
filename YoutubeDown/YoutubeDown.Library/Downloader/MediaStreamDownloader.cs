using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using YoutubeExplode.Models.MediaStreams;

namespace YoutubeDown.Library.Downloader
{
    public class MediaStreamDownloader
    {
        public async Task DownloadMediaStream(YoutubeExplode.YoutubeClient youtubeClient, MediaStreamInfo mediaStreamInfo, string filePath)
            => await DownloadMediaStream(youtubeClient, mediaStreamInfo, filePath, null).ConfigureAwait(false);

        public async Task DownloadMediaStream(YoutubeExplode.YoutubeClient youtubeClient, MediaStreamInfo mediaStreamInfo, string filePath, IProgress<DownloadProgress> progress)
            => await DownloadMediaStream(youtubeClient, mediaStreamInfo, filePath, progress, CancellationToken.None, 4096).ConfigureAwait(false);

        public async Task DownloadMediaStream(YoutubeExplode.YoutubeClient youtubeClient, MediaStreamInfo mediaStreamInfo, string filePath, IProgress<DownloadProgress> progress, CancellationToken cancellationToken)
            => await DownloadMediaStream(youtubeClient, mediaStreamInfo, filePath, progress, cancellationToken, 4096).ConfigureAwait(false);

        public async Task DownloadMediaStream(YoutubeExplode.YoutubeClient youtubeClient, MediaStreamInfo mediaStreamInfo, string filePath, IProgress<DownloadProgress> progress, CancellationToken cancellationToken, int bufferSize)
        {
            using (var mediaStream = await youtubeClient.GetMediaStreamAsync(mediaStreamInfo))
            {
                using (var output = File.Create(filePath, bufferSize))
                {
                    var buffer = new byte[bufferSize];
                    int bytesRead;
                    long totalBytesRead = 0;
                    long totalBytesReadNextSecond = 0;
                    long totalBytesReadInLastSecond = 0;

                    var timer = new System.Timers.Timer(TimeSpan.FromSeconds(1).TotalMilliseconds);
                    timer.AutoReset = true;
                    timer.Elapsed += (s, e) =>
                    {
                        totalBytesReadInLastSecond = totalBytesRead - totalBytesReadNextSecond;
                        totalBytesReadNextSecond = totalBytesRead;

                    };
                    timer.Start();

                    do
                    {
                        // Read
                        totalBytesRead += bytesRead = await mediaStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false);

                        // Write
                        await output.WriteAsync(buffer, 0, bytesRead, cancellationToken).ConfigureAwait(false);

                        progress?.Report(new DownloadProgress(totalBytesRead, mediaStream.Length, totalBytesReadInLastSecond));
                    }
                    while (bytesRead > 0);

                    timer.Stop();
                }
            }
        }
    }
}
