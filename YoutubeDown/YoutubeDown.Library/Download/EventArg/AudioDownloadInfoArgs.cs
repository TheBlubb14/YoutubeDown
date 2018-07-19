using System;

namespace YoutubeDown.Library.Download.EventArg
{
    public class AudioDownloadInfoArgs : EventArgs, IDownloadInfoArgs
    {
        public string Titel { get; set; }
        public string FullFileName { get; set; }
        public FileSize AudioSize { get; private set; }
        public FileSize TotalSize { get; private set; }

        public AudioDownloadInfoArgs(string titel, string fullFileName, long audioSize)
        {
            Titel = titel;
            FullFileName = fullFileName;
            AudioSize = new FileSize(audioSize);
            TotalSize = new FileSize(audioSize);
        }
    }
}
