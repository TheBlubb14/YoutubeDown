using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
        private Dictionary<int, (Func<Video, IComparable> Property, SortMode SortMode)> GridDictionary;


        public MainWindow()
        {
            InitializeComponent();
            cancellationTokenSource = new CancellationTokenSource();
            GridDictionary = new Dictionary<int, (Func<Video, IComparable> Property, SortMode SortMode)>();
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

            GridDictionary.Clear();

            AddColumn(nameof(Video.Id), "Id", x => x.Id);
            AddColumn(nameof(Video.Author), "Author", x => x.Author);
            AddColumn(nameof(Video.Title), "Title", x => x.Title);
            AddColumn(nameof(Video.Description), "Description", x => x.Description);
            AddColumn(nameof(Statistics.ViewCount), "Views", x => x.Statistics.ViewCount, useDecimalSeparator: true);
            AddColumn(nameof(Statistics.LikeCount), "Likes", x => x.Statistics.LikeCount, useDecimalSeparator: true);
            AddColumn(nameof(Statistics.DislikeCount), "Dislikes", x => x.Statistics.DislikeCount, useDecimalSeparator: true);
            AddColumn(nameof(Video.UploadDate), "Date", x => x.UploadDate);

            // Has to be set after adding the rows to the grid
            // Otherwise there will be an empty column 
            // https://msdn.microsoft.com/de-de/library/system.windows.forms.datagridview.rowcount(v=vs.110).aspx
            // "... If you set the RowCount property to a value greater than 0 for a DataGridView control without columns, a DataGridViewTextBoxColumn is added automatically."
            dataGridView1.RowCount = Downloads.Count;
        }

        private void AddColumn(string columnName, string title, Func<Video, IComparable> func, SortMode sortMode = SortMode.None, bool useDecimalSeparator = false)
        {
            dataGridView1.Columns.Add(columnName, title);

            if (useDecimalSeparator)
                dataGridView1.Columns[columnName].DefaultCellStyle.Format = "N0";

            GridDictionary.Add(dataGridView1.Columns.Count - 1, (func, sortMode));
        }

        private void dataGridView1_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
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

            dataGridView1.Refresh();
        }
    }
}
