using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows.Forms;
using YoutubeDown.Library;

namespace YoutubeDown
{
    public partial class SettingsWindow : Form
    {
        public Settings Settings { get; set; }

        public SettingsWindow(Settings settings)
        {
            InitializeComponent();
            Settings = settings;
        }

        private void SettingsWindow_Load(object sender, EventArgs e)
        {
            textBoxFFmpegLocation.Text = Settings.FFmpegLocation;
            textBoxDownloadLocation.Text = Settings.DownloadLocation;
            checkBoxOverwriteFiles.Checked = Settings.OverwriteFiles;
            numericUpDownMaxDegreeOfParalellism.Value = Settings.MaxDegreeOfParalellism;
        }

        private void buttonFFmpegLocation_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "ffmpeg|ffmpeg.exe|all files|*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                textBoxFFmpegLocation.Text = openFileDialog.FileName;
        }

        private void buttonDownloadLocation_Click(object sender, EventArgs e)
        {
            var folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                textBoxDownloadLocation.Text = folderBrowserDialog.SelectedPath;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SaveSettings()
        {
            var settings = new Settings
            {
                FFmpegLocation = textBoxFFmpegLocation.Text,
                DownloadLocation = textBoxDownloadLocation.Text,
                OverwriteFiles = checkBoxOverwriteFiles.Checked,
                MaxDegreeOfParalellism = Convert.ToInt32(numericUpDownMaxDegreeOfParalellism.Value)
            };

            // settings changed?
            if (settings != Settings)
            {
                try
                {
                    File.WriteAllText(Settings.SettingsLocation, JsonConvert.SerializeObject(settings, Formatting.Indented));
                    this.DialogResult = DialogResult.OK;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Couldnt save settings");
                }
            }

            this.Close();
        }
    }
}
