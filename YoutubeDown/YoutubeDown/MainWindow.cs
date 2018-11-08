using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoutubeDown.Library;
using YoutubeDown.Library.Download;
using YoutubeExplode.Models;
using System.Collections.Async;

namespace YoutubeDown
{
    public partial class MainWindow : Form
    {
        public Settings Settings { get; set; }
        public YoutubeClient YoutubeClient { get; set; }
        public List<VideoDownload> Downloads { get; set; }

        private CancellationTokenSource cancellationTokenSource;
        private bool isRunning;
        private Dictionary<int, (Func<VideoDownload, IComparable> Property, SortMode SortMode)> GridDictionary;


        public MainWindow()
        {
            InitializeComponent();
            Downloads = new List<VideoDownload>();
            cancellationTokenSource = new CancellationTokenSource();
            GridDictionary = new Dictionary<int, (Func<VideoDownload, IComparable> Property, SortMode SortMode)>();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            LoadSettings();
            LoadYoutubeClient();
            PrepareGrid();
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
                YoutubeClient = new YoutubeClient(Settings.FFmpegLocation, Settings.DownloadLocation, Settings.OverwriteFiles);
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
            textBoxVideoId.Clear();
        }

        private void buttonDownload_Click(object sender, EventArgs e)
        {
            //Work();
        }

        //private async void Work()
        //{
        //    if (isRunning)
        //    {
        //        if (!cancellationTokenSource.IsCancellationRequested)
        //            if (MessageBox.Show("Do u want to cancel the download?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
        //                cancellationTokenSource.Cancel();

        //        return;
        //    }

        //    if (string.IsNullOrEmpty(textBoxVideoId.Text))
        //        return;

        //    if (!ValidateSettings())
        //        return;

        //    buttonDownload.Text = "cancel";

        //    var progress = new Progress<double>(x =>
        //    {
        //        if (this.IsDisposed)
        //            return;

        //        toolStripProgressBar.Value = (int)x;
        //        toolStripStatusLabel.Text = $"downloading {x.ToString("00.00")}%";
        //    });

        //    try
        //    {
        //        isRunning = true;
        //        YoutubeClient = new YoutubeClient(Settings.FFmpegLocation, Settings.DownloadLocation, Settings.OverwriteFiles, cancellationTokenSource.Token);
        //        YoutubeClient.VideoDownloadInfo += this.YoutubeClient_VideoDownloadInfo;
        //        YoutubeClient.MuxingStarted += (s, e) => this.Invoke(() => toolStripStatusLabel.Text = "muxing started");
        //        YoutubeClient.MuxingFinished += (s, e) => this.Invoke(() => toolStripStatusLabel.Text = "muxing finished");

        //        await YoutubeClient.DownloadHighestVideo(textBoxVideoId.Text, progress);
        //    }
        //    catch (ArgumentException ex) when (ex.ParamName == "videoId")
        //    {
        //        MessageBox.Show("Invalid video id");
        //    }
        //    catch (TaskCanceledException) when (cancellationTokenSource.IsCancellationRequested)
        //    { }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "An error ocurred");
        //    }
        //    finally
        //    {
        //        isRunning = false;
        //        YoutubeClient = null;
        //        cancellationTokenSource = new CancellationTokenSource();
        //        if (!this.IsDisposed)
        //        {
        //            buttonDownload.Text = "download";
        //            toolStripProgressBar.Value = 0;
        //            toolStripStatusLabel.Text = "";
        //            labelVideoTitelValue.Text = "";
        //            labelAudioSizeValue.Text = "";
        //            labelVideoSizeValue.Text = "";
        //            labelTotalSizeValue.Text = "";
        //        }
        //    }

        //}

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

            if (Settings.MaxDegreeOfParalellism < 1)
            {
                MessageBox.Show("invalid MaxDegreeOfParalellism setting");
                return false;
            }

            return true;
        }

        //private void YoutubeClient_VideoDownloadInfo(object sender, VideoDownloadInfoArgs e)
        //{
        //    this.Invoke(() =>
        //    {
        //        toolTipTitle.SetToolTip(labelVideoTitelValue, e.Titel);
        //        labelVideoTitelValue.Text = e.Titel;
        //    });
        //    this.Invoke(() => labelAudioSizeValue.Text = e.AudioSize.SizeLabel);
        //    this.Invoke(() => labelVideoSizeValue.Text = e.VideoSize.SizeLabel);
        //    this.Invoke(() => labelTotalSizeValue.Text = e.TotalSize.SizeLabel);
        //}

        private void buttonAddDownload_Click(object sender, EventArgs e)
        {
            AddDownload(textBoxVideoId.Text);
            textBoxVideoId.Clear();
        }

        private async void AddDownload(string url)
        {
            foreach (var video in await YoutubeClient.GetVideosAsync(url))
            {
                var videoDownload = new VideoDownload(video, YoutubeClient, CancellationTokenSource.CreateLinkedTokenSource(cancellationTokenSource.Token));
                videoDownload.PropertyChanged += (s, e) =>
                {
                    var column = dataGrid.Columns[e.PropertyName];

                    if (column == null)
                        return;

                    var columnIndex = dataGrid.Columns.IndexOf(column);
                    var rowIndex = Downloads.IndexOf(videoDownload);
                    var cell = dataGrid[columnIndex, rowIndex];

                    // CellValueNeeded will update the cell then
                    if (!cell.Visible)
                        return;

                    dataGrid.InvalidateCell(cell);
                };

                Downloads.Add(videoDownload);
            }

            // Has to be set after adding the rows to the grid
            // Otherwise there will be an empty column 
            // https://msdn.microsoft.com/de-de/library/system.windows.forms.datagridview.rowcount(v=vs.110).aspx
            // "... If you set the RowCount property to a value greater than 0 for a DataGridView control without columns, a DataGridViewTextBoxColumn is added automatically."
            dataGrid.RowCount = Downloads.Count;
        }

        private void PrepareGrid()
        {
            dataGrid.Columns.Clear();
            dataGrid.AutoGenerateColumns = false;
            dataGrid.AllowUserToAddRows = false;
            dataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            GridDictionary.Clear();

            AddColumn(nameof(Video.Id), "Id", x => x.Video.Id);
            AddColumn(nameof(Video.Author), "Author", x => x.Video.Author);
            AddColumn(nameof(Video.Title), "Title", x => x.Video.Title);
            AddColumn(nameof(Video.Description), "Description", x => x.Video.Description);
            AddColumn(nameof(Statistics.ViewCount), "Views", x => x.Video.Statistics.ViewCount, useDecimalSeparator: true);
            AddColumn(nameof(Statistics.LikeCount), "Likes", x => x.Video.Statistics.LikeCount, useDecimalSeparator: true);
            AddColumn(nameof(Statistics.DislikeCount), "Dislikes", x => x.Video.Statistics.DislikeCount, useDecimalSeparator: true);
            AddColumn(nameof(Video.UploadDate), "Date", x => x.Video.UploadDate);
            AddColumn(nameof(VideoDownload.Status), "Status", x => x.Status);
            AddColumn(nameof(VideoDownload.DownloadPercentage), "%", x => x.DownloadPercentage);
            AddColumn(nameof(VideoDownload.AudioSize), "Audiosize", x => x.AudioSize?.SizeLabel);
            AddColumn(nameof(VideoDownload.VideoSize), "Videosize", x => x.VideoSize?.SizeLabel);
            AddColumn(nameof(VideoDownload.TotalSize), "Totalsize", x => x.TotalSize?.SizeLabel);
        }

        private void AddColumn(string propertyName, string title, Func<VideoDownload, IComparable> func, SortMode sortMode = SortMode.None, bool useDecimalSeparator = false)
        {
            dataGrid.Columns.Add(propertyName, title);

            if (useDecimalSeparator)
                dataGrid.Columns[propertyName].DefaultCellStyle.Format = "N0";

            GridDictionary.Add(dataGrid.Columns.Count - 1, (func, sortMode));
        }

        private void dataGrid_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            e.Value = GridDictionary[e.ColumnIndex].Property.Invoke(Downloads[e.RowIndex]);
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // TODO: better name
            var grid = GridDictionary[e.ColumnIndex];
            switch (grid.SortMode)
            {
                case SortMode.None:
                case SortMode.Descending:
                    Downloads.Sort((x, y) => x.CompareTo(y, grid.Property));
                    GridDictionary[e.ColumnIndex] = (grid.Property, SortMode.Ascending);
                    break;

                case SortMode.Ascending:
                    Downloads.Sort((x, y) => y.CompareTo(x, grid.Property));
                    GridDictionary[e.ColumnIndex] = (grid.Property, SortMode.Descending);
                    break;
            }

            dataGrid.Refresh();
        }

        private void buttonSelectedVideosDownload_Click(object sender, EventArgs e)
        {
            Work(GetSelectedVideos());
        }

        private async void Work(IEnumerable<VideoDownload> videoDownloads)
        {
            if (isRunning)
                return;

            isRunning = true;

            await videoDownloads.ParallelForEachAsync(async video =>
            {
                await video.DownloadAsync();
            },
            maxDegreeOfParalellism: Settings.MaxDegreeOfParalellism,
            cancellationToken: cancellationTokenSource.Token);

            isRunning = false;
        }

        private IEnumerable<VideoDownload> GetSelectedVideos()
        {
            foreach (DataGridViewRow row in dataGrid.SelectedRows)
            {
                var videoId = row.Cells[nameof(Video.Id)].Value.ToString();
                var video = Downloads.FirstOrDefault(x => x.Video.Id == videoId);

                if (video == null)
                    continue;

                yield return video;
            }
        }

        private IEnumerable<VideoDownload> GetAllVideos()
        {
            foreach (DataGridViewRow row in dataGrid.Rows)
            {
                var videoId = row.Cells[nameof(Video.Id)].Value.ToString();
                var video = Downloads.FirstOrDefault(x => x.Video.Id == videoId);

                if (video == null)
                    continue;

                yield return video;
            }
        }

        private void dataGrid_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            var download = Downloads[e.RowIndex];

            if (download.Status == DownloadStatus.Downloading || download.Status == DownloadStatus.Muxing)
            {
                // TODO: ask to cancel the download
            }

            Downloads.Remove(download);
        }

        private void checkBoxClipboard_CheckedChanged(object sender, EventArgs e)
        {
            timerClipboard.Enabled = checkBoxClipboard.Checked;
        }

        private string lastUrl = "";
        private void timerClipboard_Tick(object sender, EventArgs e)
        {
            var text = Clipboard.GetText();

            if (!text.StartsWith("https://www.youtube.com"))
                return;

            if (lastUrl != text)
            {
                AddDownload(text);
                lastUrl = Clipboard.GetText();
            }
        }
    }
}
