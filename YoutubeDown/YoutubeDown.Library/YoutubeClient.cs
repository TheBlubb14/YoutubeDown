using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YoutubeDown.Library.Downloader;
using YoutubeDown.Library.ffmpeg;
using YoutubeExplode.Models;
using YoutubeExplode.Models.MediaStreams;

namespace YoutubeDown.Library
{
    public class YoutubeClient
    {
        private YoutubeExplode.YoutubeClient client;

        public string FFmpegPath { get; set; }
        public string DownloadPath { get; set; }
        public IProgress<double> Progress { get; set; }
        public bool OverwriteFiles { get; set; }
        public CancellationToken CancellationToken { get; set; }

        public event EventHandler<VideoDownloadInfoArgs> VideoDownloadInfo;
        public event EventHandler MuxingStarted;
        public event EventHandler MuxingFinished;

        public YoutubeClient(string ffmpegPath, string downloadPath, Progress<double> progress, bool overwriteFiles, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(ffmpegPath))
                throw new ArgumentNullException(nameof(ffmpegPath));

            if (string.IsNullOrEmpty(downloadPath))
                throw new ArgumentNullException(nameof(downloadPath));

            FFmpegPath = ffmpegPath;
            DownloadPath = downloadPath;
            Progress = progress;
            OverwriteFiles = overwriteFiles;
            CancellationToken = cancellationToken;

            client = new YoutubeExplode.YoutubeClient();
        }

        /// <summary>
        /// Downloads the best adaptive video from Youtube
        /// </summary>
        /// <param name="videoId">The video id</param>
        /// <returns></returns>
        public async Task DownloadHighestVideo(string videoId)
        {
            videoId = videoId.NormalizeYoutubeVideoId();

            var video = await client.GetVideoAsync(videoId);
            var videoStreamInfos = await client.GetVideoMediaStreamInfosAsync(videoId);

            // get adaptive videostream with best videoquality
            var videoStreamInfo = videoStreamInfos.Video
                .OrderBy(x => x.VideoQuality)
                .ThenBy(x => x.Bitrate)
                .LastOrDefault();

            AudioStreamInfo audioStreamInfo = null;

            // get adaptive audiostream with highest bitrate
            if (videoStreamInfo.Container == Container.WebM)
            {
                audioStreamInfo = videoStreamInfos.Audio
                    .Where(x => x.AudioEncoding == AudioEncoding.Vorbis || x.AudioEncoding == AudioEncoding.Opus)
                    .OrderBy(x => x.Bitrate)
                    .LastOrDefault();
            }
            else
            {
                audioStreamInfo = videoStreamInfos.Audio
                    .Where(x => x.AudioEncoding == AudioEncoding.Aac)
                    .OrderBy(x => x.Bitrate)
                    .LastOrDefault();
            }

            // temporary filenames for downloading adaptive files
            var tmpAudioFileName = Path.Combine(DownloadPath, $"{video.Title}__audio".GetValidFileName());
            var tmpVideoFileName = Path.Combine(DownloadPath, $"{video.Title}__video".GetValidFileName());

            // get the final filename
            var fileExtension = videoStreamInfo.Container.GetFileExtension();
            var fileName = Path.Combine(DownloadPath, $"{video.Title}.{fileExtension}".GetValidFileName());

            VideoDownloadInfo?.Invoke(this, new VideoDownloadInfoArgs(video.Title, fileName, audioStreamInfo.Size, videoStreamInfo.Size));

            // download progress calculation
            double audioDownloadProgress = 0;
            double videoDownloadProgress = 0;

            var totalFileSize = audioStreamInfo.Size + videoStreamInfo.Size;
            var audioWeight = (double)Math.Round((100m / totalFileSize) * audioStreamInfo.Size, 2);
            var videoWeight = (double)Math.Round((100m / totalFileSize) * videoStreamInfo.Size, 2);

            var audioProgress = new Progress<double>(x =>
            {
                audioDownloadProgress = (audioWeight * x);
                Progress?.Report(audioDownloadProgress + videoDownloadProgress);
            });

            var videoProgress = new Progress<double>(x =>
            {
                videoDownloadProgress = (videoWeight * x);
                Progress?.Report(audioDownloadProgress + videoDownloadProgress);
            });

            //var audioProgress = new Progress<DownloadProgress>(x =>
            //{
            //    audioDownloadProgress = (audioWeight * x.Percentage);
            //    Debug.WriteLine("SPEED: " + x.Speed);
            //    Progress?.Report(audioDownloadProgress + videoDownloadProgress);
            //});

            //var videoProgress = new Progress<DownloadProgress>(x =>
            //{
            //    videoDownloadProgress = (videoWeight * x.Percentage);
            //    Debug.WriteLine("SPEED: " + x.Speed);
            //    Progress?.Report(audioDownloadProgress + videoDownloadProgress);
            //});

            try
            {
                //var downloader = new MediaStreamDownloader();
                //await downloader.DownloadMediaStream(client, audioStreamInfo, tmpAudioFileName, audioProgress, CancellationToken);
                //await downloader.DownloadMediaStream(client, videoStreamInfo, tmpVideoFileName, videoProgress, CancellationToken);

                // downloading adaptive streams
                await client.DownloadMediaStreamAsync(audioStreamInfo, tmpAudioFileName, audioProgress, CancellationToken);
                await client.DownloadMediaStreamAsync(videoStreamInfo, tmpVideoFileName, videoProgress, CancellationToken);

                // muxing videofile with audiofile
                MuxingStarted?.Invoke(this, EventArgs.Empty);
                using (var muxer = new Muxer(OverwriteFiles, FFmpegPath))
                {
                    // TODO: Log
                    muxer.DataReceived += (s, e) => { Debug.WriteLine(e.Data?.ToString()); };
                    muxer.Mux(tmpVideoFileName, tmpAudioFileName, fileName, LogLevel.error);
                }
                MuxingFinished?.Invoke(this, EventArgs.Empty);
            }
            finally
            {
                if (File.Exists(tmpAudioFileName))
                    File.Delete(tmpAudioFileName);
                if (File.Exists(tmpVideoFileName))
                    File.Delete(tmpVideoFileName);
            }
        }
    }
}
