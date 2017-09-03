namespace YoutubeDown.Library
{
    public class FileSize
    {
        public long Size { get; private set; }
        public string SizeLabel => Size.NormalizeFileSize();

        public FileSize(long size)
        {
            Size = size;
        }
    }
}
