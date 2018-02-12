using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeDown.Library.Download
{
    public enum DownloadStatus
    {
        Queued,
        Downloading,
        Finished,
        Canceled,
        Muxing,
        VideoNotAvaible,
        VideoNotFound,
        HttpError,
    }
}
