using System;
using System.Diagnostics;
using System.IO;

namespace YoutubeDown.Library.ffmpeg
{
    public static class Muxer
    {
        public static bool OverwriteFiles { get; set; }
        public static string FFmpegPath { get; set; }

        private static Process ffmpegProcess = null;

        public static void Mux(string videoFile, string audioFile, string destinationFile, LogLevel logLevel)
        {
            try
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

                ffmpegProcess.ErrorDataReceived += FfmpegProcess_DataReceived;
                ffmpegProcess.OutputDataReceived += FfmpegProcess_DataReceived;

                ffmpegProcess.Start();
                ffmpegProcess.BeginErrorReadLine();
                ffmpegProcess.BeginOutputReadLine();

                ffmpegProcess.WaitForExit();
            }
            catch (Exception ex)
            {
                ffmpegProcess?.Dispose();
                throw ex;
            }
        }

        private static void FfmpegProcess_DataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                ffmpegProcess?.Dispose();
                throw new Exception(e.Data);
            }
        }
    }
}
