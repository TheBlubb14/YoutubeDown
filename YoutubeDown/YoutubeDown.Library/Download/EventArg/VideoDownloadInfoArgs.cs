using System;

namespace YoutubeDown.Library.Download.EventArg
{
    public class VideoDownloadInfoArgs : EventArgs, IDownloadInfoArgs
    {
        public string Titel { get; set; }
        public string FullFileName { get; set; }
        public FileSize AudioSize { get; private set; }
        public FileSize VideoSize { get; private set; }
        public FileSize TotalSize { get; private set; }

        public VideoDownloadInfoArgs(string titel, string fullFileName, long audioSize, long videoSize)
        {
            Titel = titel;
            FullFileName = fullFileName;
            AudioSize = new FileSize(audioSize);
            VideoSize = new FileSize(videoSize);
            TotalSize = new FileSize(audioSize + videoSize);
        }
    }
}
