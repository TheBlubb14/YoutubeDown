using System;
using System.Diagnostics;
using System.IO;

namespace YoutubeDown.Library.ffmpeg
{
    public class Muxer : IDisposable
    {
        public bool OverwriteFiles { get; set; }
        public string FFmpegPath { get; set; }

        public event EventHandler<DataReceivedEventArgs> DataReceived;

        private Process ffmpegProcess = null;

        public Muxer(bool overwriteFiles, string ffmpegPath)
        {
            OverwriteFiles = overwriteFiles;
            FFmpegPath = ffmpegPath;
        }

        public void Mux(string videoFile, string audioFile, string destinationFile, LogLevel logLevel)
        {
            var overwriteArgument = OverwriteFiles ? "-y" : "-n";
            var processStartInfo = new ProcessStartInfo(FFmpegPath, $"-v {logLevel} {overwriteArgument} -i \"{videoFile}\" -i \"{audioFile}\"  -c copy \"{destinationFile}\"")
            {
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false
            };

            ffmpegProcess = new Process
            {
                StartInfo = processStartInfo,
                EnableRaisingEvents = true
            };

            ffmpegProcess.ErrorDataReceived += (s, e) => { DataReceived?.Invoke(s, e); };
            ffmpegProcess.OutputDataReceived += (s, e) => { DataReceived?.Invoke(s, e); };

            ffmpegProcess.Start();
            ffmpegProcess.BeginErrorReadLine();
            ffmpegProcess.BeginOutputReadLine();

            ffmpegProcess.WaitForExit();
        }

        public void Dispose()
        {
            ffmpegProcess?.Dispose();
        }
    }
}
