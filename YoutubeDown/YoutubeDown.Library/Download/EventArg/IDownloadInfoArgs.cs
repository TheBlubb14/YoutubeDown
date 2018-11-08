namespace YoutubeDown.Library.Download.EventArg
{
    public interface IDownloadInfoArgs
    {
        string FullFileName { get; set; }
        string Titel { get; set; }
        FileSize TotalSize { get; }
    }
}