using System.IO;
using System.Linq;

namespace YoutubeDown.Library
{
    public static class Extensions
    {
        // https://stackoverflow.com/a/7393722
        public static string GetValidFileName(this string fileName)
        {
            return Path.GetInvalidFileNameChars()
                .Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty));
        }

        public static string NormalizeYoutubeVideoId(this string videoId)
        {
            if (YoutubeExplode.YoutubeClient.TryParseVideoId(videoId, out string id))
                videoId = id;

            return videoId;
        }

        public static string NormalizeFileSize(this long fileSize)
        {
            string[] units = { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
            double size = fileSize;
            var unit = 0;

            while (size >= 1024)
            {
                size /= 1024;
                ++unit;
            }

            return $"{size:0.#} {units[unit]}";
        }
    }
}
