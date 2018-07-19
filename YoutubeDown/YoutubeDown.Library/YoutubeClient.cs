using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YoutubeDown.Library.Download.EventArg;
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
        public bool OverwriteFiles { get; set; }

        public YoutubeClient(string ffmpegPath, string downloadPath, bool overwriteFiles)
        {
            if (string.IsNullOrEmpty(ffmpegPath))
                throw new ArgumentNullException(nameof(ffmpegPath));

            if (string.IsNullOrEmpty(downloadPath))
                throw new ArgumentNullException(nameof(downloadPath));

            FFmpegPath = ffmpegPath;
            DownloadPath = downloadPath;

            OverwriteFiles = overwriteFiles;
            client = new YoutubeExplode.YoutubeClient();
        }

        /// <summary>
        /// Downloads the best adaptive video from Youtube
        /// </summary>
        /// <param name="videoId">The video id</param>
        /// <param name="progress">Downloadprogress in double from 0 to 1</param>
        /// <param name="videoDownloadInfo">Infos about the current download</param>
        /// <param name="muxingStarted">Invoked when muxing starts</param>
        /// <param name="muxingFinished">Invoked when muxing finished</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns></returns>   
        public async Task DownloadHighestVideo(string videoId, IProgress<double> progress, EventHandler<VideoDownloadInfoArgs> videoDownloadInfo, EventHandler muxingStarted, EventHandler muxingFinished, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(videoId))
                return;

            videoId = videoId.NormalizeYoutubeVideoId();
            var video = await client.GetVideoAsync(videoId);

            await DownloadHighestVideo(video, progress, videoDownloadInfo, muxingStarted, muxingFinished, cancellationToken);
        }

        /// <summary>
        /// Downloads the best adaptive video from Youtube
        /// </summary>
        /// <param name="video">The Video to be downloaded</param>
        /// <param name="progress">Downloadprogress in double from 0 to 1</param>
        /// <param name="videoDownloadInfo">Infos about the current download</param>
        /// <param name="muxingStarted">Invoked when muxing starts</param>
        /// <param name="muxingFinished">Invoked when muxing finished</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns></returns>        
        public async Task DownloadHighestVideo(Video video, IProgress<double> progress, EventHandler<VideoDownloadInfoArgs> videoDownloadInfo, EventHandler muxingStarted, EventHandler muxingFinished, CancellationToken cancellationToken)
        {
            var videoStreamInfos = await client.GetVideoMediaStreamInfosAsync(video.Id);

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

            videoDownloadInfo?.Invoke(this, new VideoDownloadInfoArgs(video.Title, fileName, audioStreamInfo.Size, videoStreamInfo.Size));

            // download progress calculation
            double audioDownloadProgress = 0;
            double videoDownloadProgress = 0;

            var totalFileSize = audioStreamInfo.Size + videoStreamInfo.Size;
            var audioWeight = (double)Math.Round((100m / totalFileSize) * audioStreamInfo.Size, 2);
            var videoWeight = (double)Math.Round((100m / totalFileSize) * videoStreamInfo.Size, 2);

            var audioProgress = new Progress<double>(x =>
            {
                audioDownloadProgress = (audioWeight * x);
                progress?.Report(audioDownloadProgress + videoDownloadProgress);
            });

            var videoProgress = new Progress<double>(x =>
            {
                videoDownloadProgress = (videoWeight * x);
                progress?.Report(audioDownloadProgress + videoDownloadProgress);
            });

            try
            {
                // downloading adaptive streams
                await client.DownloadMediaStreamAsync(audioStreamInfo, tmpAudioFileName, audioProgress, cancellationToken);
                await client.DownloadMediaStreamAsync(videoStreamInfo, tmpVideoFileName, videoProgress, cancellationToken);

                // muxing videofile with audiofile
                muxingStarted?.Invoke(this, EventArgs.Empty);
                using (var muxer = new Muxer(OverwriteFiles, FFmpegPath))
                {
                    // TODO: Log
                    muxer.DataReceived += (s, e) => { Debug.WriteLine(e.Data?.ToString()); };
                    muxer.Mux(tmpVideoFileName, tmpAudioFileName, fileName, LogLevel.error);
                }
                muxingFinished?.Invoke(this, EventArgs.Empty);
            }
            finally
            {
                if (File.Exists(tmpAudioFileName))
                    File.Delete(tmpAudioFileName);
                if (File.Exists(tmpVideoFileName))
                    File.Delete(tmpVideoFileName);
            }
        }

        /// <summary>
        /// Downloads the beste audio as MP3 from the given Youtube video
        /// </summary>
        /// <param name="videoId">The video from which you want to download the audio</param>
        /// <param name="progress">Downloadprogress in double from 0 to 1</param>
        /// <param name="audioDownloadInfo">Infos about the current download</param>
        /// <param name="muxingStarted">Invoked when muxing starts</param>
        /// <param name="muxingFinished">Invoked when muxing finished</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns></returns>
        public async Task DownloadAsMP3(string videoId, IProgress<double> progress, EventHandler<AudioDownloadInfoArgs> audioDownloadInfo, EventHandler muxingStarted, EventHandler muxingFinished, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(videoId))
                return;

            videoId = videoId.NormalizeYoutubeVideoId();
            var video = await client.GetVideoAsync(videoId);

            await DownloadAsMP3(video, progress, audioDownloadInfo, muxingStarted, muxingFinished, cancellationToken);
        }

        /// <summary>
        /// Downloads the beste audio as MP3 from the given Youtube video
        /// </summary>
        /// <param name="video">The video id</param>
        /// <param name="progress">Downloadprogress in double from 0 to 1</param>
        /// <param name="audioDownloadInfo">Infos about the current download</param>
        /// <param name="muxingStarted">Invoked when muxing starts</param>
        /// <param name="muxingFinished">Invoked when muxing finished</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns></returns>
        public async Task DownloadAsMP3(Video video, IProgress<double> progress, EventHandler<AudioDownloadInfoArgs> audioDownloadInfo, EventHandler muxingStarted, EventHandler muxingFinished, CancellationToken cancellationToken)
        {
            var videoStreamInfos = await client.GetVideoMediaStreamInfosAsync(video.Id);

            var audioStreamInfo = videoStreamInfos.Audio
                .Where(x => x.AudioEncoding == AudioEncoding.Aac || x.AudioEncoding == AudioEncoding.Mp3)
                .OrderBy(x => x.Bitrate)
                .LastOrDefault();

            var tmpAudioFileName = Path.Combine(DownloadPath, $"{video.Title}__mp3".GetValidFileName());
            var fileName = Path.Combine(DownloadPath, $"{video.Title}.mp3".GetValidFileName());

            if (File.Exists(fileName))
            {
                // delete file if already exists, otherwise File.Move throws an exception
                if (OverwriteFiles)
                    File.Delete(fileName);
                else
                    return;
            }

            audioDownloadInfo?.Invoke(this, new AudioDownloadInfoArgs(video.Title, fileName, audioStreamInfo.Size));

            // download progress calculation
            double audioDownloadProgress = 0;

            var totalFileSize = audioStreamInfo.Size;
            var audioWeight = 1;

            var audioProgress = new Progress<double>(x =>
            {
                audioDownloadProgress = (audioWeight * x);
                progress?.Report(audioDownloadProgress);
            });

            try
            {
                // downloading audio stream
                await client.DownloadMediaStreamAsync(audioStreamInfo, tmpAudioFileName, audioProgress, cancellationToken);

                muxingStarted?.Invoke(this, EventArgs.Empty);

                if (audioStreamInfo.AudioEncoding == AudioEncoding.Mp3)
                {
                    File.Move(tmpAudioFileName, fileName);
                }
                else
                {
                    // convert audio to MP3
                    using (var reader = new MediaFoundationReader(tmpAudioFileName))
                        MediaFoundationEncoder.EncodeToMp3(reader, fileName);
                }

                muxingFinished?.Invoke(this, EventArgs.Empty);
            }
            finally
            {
                if (File.Exists(tmpAudioFileName))
                    File.Delete(tmpAudioFileName);
            }
        }

        public async Task<IEnumerable<Video>> GetVideosAsync(string url)
        {
            if (YoutubeExplode.YoutubeClient.TryParsePlaylistId(url, out string playlistId))
                return (await client.GetPlaylistAsync(playlistId)).Videos;
            else
                if (YoutubeExplode.YoutubeClient.TryParseVideoId(url, out string videoId))
                return new[] { await client.GetVideoAsync(videoId) };
            else
                if (YoutubeExplode.YoutubeClient.TryParseChannelId(url, out string channelId))
                return await client.GetChannelUploadsAsync(channelId);
            else
                return Enumerable.Empty<Video>();
        }
    }
}
