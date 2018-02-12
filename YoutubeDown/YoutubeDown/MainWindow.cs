using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoutubeDown.Library;
using YoutubeExplode.Models;

namespace YoutubeDown
{
    public partial class MainWindow : Form
    {
        public Settings Settings { get; set; }
        public YoutubeClient YoutubeClient { get; set; }

        private CancellationTokenSource cancellationTokenSource;
        private bool isRunning;
        private Dictionary<int, (Func<Video, object> Property, SortMode SortMode, Func<Video, Video, int> SortFunctAsc, Func<Video, Video, int> SortFunctDesc)> GridDictionary;


        public MainWindow()
        {
            InitializeComponent();
            cancellationTokenSource = new CancellationTokenSource();
            GridDictionary = new Dictionary<int, (Func<Video, object> Property, SortMode SortMode, Func<Video, Video, int> SortFunctAsc, Func<Video, Video, int> SortFunctDesc)>();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            LoadSettings();
            LoadYoutubeClient();
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

        private void LoadYoutubeClient()
        {
            try
            {
                YoutubeClient = new YoutubeClient(Settings.FFmpegLocation, Settings.DownloadLocation, Settings.OverwriteFiles, cancellationTokenSource.Token);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void textBoxVideoId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Enter)
                return;

            e.Handled = true;

            AddDownload(textBoxVideoId.Text);
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
                YoutubeClient = new YoutubeClient(Settings.FFmpegLocation, Settings.DownloadLocation, Settings.OverwriteFiles, cancellationTokenSource.Token);
                YoutubeClient.VideoDownloadInfo += this.YoutubeClient_VideoDownloadInfo;
                YoutubeClient.MuxingStarted += (s, e) => this.Invoke(() => toolStripStatusLabel.Text = "muxing started");
                YoutubeClient.MuxingFinished += (s, e) => this.Invoke(() => toolStripStatusLabel.Text = "muxing finished");

                await YoutubeClient.DownloadHighestVideo(textBoxVideoId.Text, progress);
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

        private void buttonAddDownload_Click(object sender, EventArgs e)
        {
            AddDownload(textBoxVideoId.Text);
        }

        public List<Video> Downloads { get; set; }

        private async void AddDownload(string url)
        {
            Downloads = (await YoutubeClient.GetVideosAsync(url)).ToList();
            PrepareGrid();
        }

        private void PrepareGrid()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.Columns.Add(nameof(Video.Id), "Id");
            dataGridView1.Columns.Add(nameof(Video.Author), "Author");
            dataGridView1.Columns.Add(nameof(Video.Title), "Title");
            dataGridView1.Columns.Add(nameof(Video.Description), "Description");
            dataGridView1.Columns.Add(nameof(Statistics.ViewCount), "Views");
            dataGridView1.Columns.Add(nameof(Statistics.LikeCount), "Likes");
            dataGridView1.Columns.Add(nameof(Statistics.DislikeCount), "Dislikes");
            dataGridView1.Columns.Add(nameof(Video.UploadDate), "Date");

            dataGridView1.RowCount = Downloads.Count;

            GridDictionary.Clear();
            GridDictionary.Add(0,
                (x => x.Id, SortMode.None,
                (x, y) => x.Id.CompareTo(y.Id),
                (x, y) => y.Id.CompareTo(x.Id)));

            GridDictionary.Add(1,
                (x => x.Author, SortMode.None,
                (x, y) => x.Author.CompareTo(y.Author),
                (x, y) => y.Author.CompareTo(x.Author)));

            GridDictionary.Add(2,
                (x => x.Title, SortMode.None,
                (x, y) => x.Title.CompareTo(y.Title),
                (x, y) => y.Title.CompareTo(x.Title)));

            GridDictionary.Add(3,
                (x => x.Description, SortMode.None,
                (x, y) => x.Description.CompareTo(y.Description),
                (x, y) => y.Description.CompareTo(x.Description)));

            GridDictionary.Add(4,
                (x => x.Statistics.ViewCount, SortMode.None,
                (x, y) => x.Statistics.ViewCount.CompareTo(y.Statistics.ViewCount),
                (x, y) => y.Statistics.ViewCount.CompareTo(x.Statistics.ViewCount)));

            GridDictionary.Add(5,
                (x => x.Statistics.LikeCount, SortMode.None,
                (x, y) => x.Statistics.LikeCount.CompareTo(y.Statistics.LikeCount),
                (x, y) => y.Statistics.LikeCount.CompareTo(x.Statistics.LikeCount)));

            GridDictionary.Add(6,
                (x => x.Statistics.DislikeCount, SortMode.None,
                (x, y) => x.Statistics.DislikeCount.CompareTo(y.Statistics.DislikeCount),
                (x, y) => y.Statistics.DislikeCount.CompareTo(x.Statistics.DislikeCount)));

            GridDictionary.Add(7,
                (x => x.UploadDate, SortMode.None,
                (x, y) => x.UploadDate.CompareTo(y.UploadDate),
                (x, y) => y.UploadDate.CompareTo(x.UploadDate)));
        }

        private void dataGridView1_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            e.Value = GridDictionary[e.ColumnIndex].Property.Invoke(Downloads[e.RowIndex]);
        }

        public enum SortMode
        {
            None,
            Ascending,
            Descending
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // TODO: better name
            var grid = GridDictionary[e.ColumnIndex];
            switch (grid.SortMode)
            {
                case SortMode.None:
                case SortMode.Descending:
                    Downloads.Sort((x, y) => grid.SortFunctAsc(x, y));
                    GridDictionary[e.ColumnIndex] = (grid.Property, SortMode.Ascending, grid.SortFunctAsc, grid.SortFunctDesc);
                    break;

                case SortMode.Ascending:
                    Downloads.Sort((x, y) => grid.SortFunctDesc(x, y));
                    GridDictionary[e.ColumnIndex] = (grid.Property, SortMode.Descending, grid.SortFunctAsc, grid.SortFunctDesc);
                    break;
            }

            dataGridView1.Refresh();
        }
    }
}
