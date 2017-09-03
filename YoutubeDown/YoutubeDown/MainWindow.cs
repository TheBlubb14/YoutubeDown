using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoutubeDown.Library;

namespace YoutubeDown
{
    public partial class MainWindow : Form
    {
        public Settings Settings { get; set; }
        public YoutubeClient YoutubeClient { get; set; }

        private CancellationTokenSource cancellationTokenSource;
        private bool isRunning;

        public MainWindow()
        {
            InitializeComponent();
            cancellationTokenSource = new CancellationTokenSource();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            LoadSettings();
        }

        private async void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isRunning)
            {
                if (MessageBox.Show("Do u want to quit?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    cancellationTokenSource.Cancel();
                    // wait for the task to finish
                    while (isRunning)
                        await Task.Delay(10);
                }
                else
                {
                    e.Cancel = true;
                }

            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (new SettingsWindow(Settings).ShowDialog() == DialogResult.OK)
                LoadSettings();
        }

        private void LoadSettings()
        {
            try
            {
                if (File.Exists(Settings.SettingsLocation))
                    Settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(Settings.SettingsLocation));
                else
                    Settings = default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Couldnt load settings");
            }
        }

        private void textBoxVideoId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Enter)
                return;

            e.Handled = true;
            Work();
        }

        private void buttonDownload_Click(object sender, EventArgs e)
        {
            Work();
        }

        private async void Work()
        {
            if (isRunning)
            {
                if (!cancellationTokenSource.IsCancellationRequested)
                    if (MessageBox.Show("Do u want to cancel the download?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        cancellationTokenSource.Cancel();

                return;
            }

            if (string.IsNullOrEmpty(textBoxVideoId.Text))
                return;

            if (!ValidateSettings())
                return;

            buttonDownload.Text = "cancel";

            var progress = new Progress<double>(x =>
            {
                if (this.IsDisposed)
                    return;

                toolStripProgressBar.Value = (int)x;
                toolStripStatusLabel.Text = $"downloading {x.ToString("00.00")}%";
            });

            try
            {
                isRunning = true;
                YoutubeClient = new YoutubeClient(Settings.FFmpegLocation, Settings.DownloadLocation, progress, Settings.OverwriteFiles, cancellationTokenSource.Token);
                YoutubeClient.VideoDownloadInfo += this.YoutubeClient_VideoDownloadInfo;
                YoutubeClient.MuxingStarted += (s, e) => this.Invoke(() => toolStripStatusLabel.Text = "muxing started");
                YoutubeClient.MuxingFinished += (s, e) => this.Invoke(() => toolStripStatusLabel.Text = "muxing finished");

                await YoutubeClient.DownloadHighestVideo(textBoxVideoId.Text);
            }
            catch (ArgumentException ex) when (ex.ParamName == "videoId")
            {
                MessageBox.Show("Invalid video id");
            }
            catch (TaskCanceledException) when (cancellationTokenSource.IsCancellationRequested)
            { }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "An error ocurred");
            }
            finally
            {
                isRunning = false;
                YoutubeClient = null;
                cancellationTokenSource = new CancellationTokenSource();
                if (!this.IsDisposed)
                {
                    buttonDownload.Text = "download";
                    toolStripProgressBar.Value = 0;
                    toolStripStatusLabel.Text = "";
                    labelVideoTitelValue.Text = "";
                    labelAudioSizeValue.Text = "";
                    labelVideoSizeValue.Text = "";
                    labelTotalSizeValue.Text = "";
                }
            }

        }

        private bool ValidateSettings()
        {
            if (Settings == default)
            {
                MessageBox.Show("You have to edit your settings first");
                return false;
            }

            if (string.IsNullOrEmpty(Settings.FFmpegLocation))
            {
                MessageBox.Show("ffmpeg location is not set");
                return false;
            }

            if (string.IsNullOrEmpty(Settings.DownloadLocation))
            {
                MessageBox.Show("download location is not set");
                return false;
            }

            return true;
        }

        private void YoutubeClient_VideoDownloadInfo(object sender, VideoDownloadInfoArgs e)
        {
            this.Invoke(() =>
            {
                toolTipTitle.SetToolTip(labelVideoTitelValue, e.Titel);
                labelVideoTitelValue.Text = e.Titel;
            });
            this.Invoke(() => labelAudioSizeValue.Text = e.AudioSize.SizeLabel);
            this.Invoke(() => labelVideoSizeValue.Text = e.VideoSize.SizeLabel);
            this.Invoke(() => labelTotalSizeValue.Text = e.TotalSize.SizeLabel);
        }

        private void labelVideoSizeValue_Click(object sender, EventArgs e)
        {

        }
    }
}
