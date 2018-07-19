using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace YoutubeDown.Library
{
    public struct Settings : IEquatable<Settings>
    {
        [JsonIgnore]
        public const string SettingsLocation = "settings.json";

        public string FFmpegLocation { get; set; }
        public string DownloadLocation { get; set; }
        public bool OverwriteFiles { get; set; }
        public int MaxDegreeOfParalellism { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Settings settings && settings == this;
        }

        public bool Equals(Settings other)
        {
            return this.FFmpegLocation == other.FFmpegLocation &&
                   this.DownloadLocation == other.DownloadLocation &&
                   this.OverwriteFiles == other.OverwriteFiles &&
                   this.MaxDegreeOfParalellism == other.MaxDegreeOfParalellism;
        }

        // automaticly generated from vs
        public override int GetHashCode()
        {
            var hashCode = 491232962;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.FFmpegLocation);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.DownloadLocation);
            hashCode = hashCode * -1521134295 + this.OverwriteFiles.GetHashCode();
            hashCode = hashCode * -1521134295 + this.MaxDegreeOfParalellism.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Settings x, Settings y)
        {
            return string.Equals(x.FFmpegLocation, y.FFmpegLocation, System.StringComparison.OrdinalIgnoreCase) &&
                string.Equals(x.DownloadLocation, y.DownloadLocation, System.StringComparison.OrdinalIgnoreCase) &&
                x.OverwriteFiles == y.OverwriteFiles &&
                x.MaxDegreeOfParalellism == y.MaxDegreeOfParalellism;
        }

        public static bool operator !=(Settings x, Settings y)
        {
            return !(x == y);
        }
    }
}
