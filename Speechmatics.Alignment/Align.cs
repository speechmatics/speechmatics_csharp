using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Speechmatics.API;

namespace Speechmatics.Alignment
{
    public partial class Align : Form
    {
        private class AlignmentJobInputData
        {
            internal string AudioFile { get; set; }
            internal string TextFile { get; set; }
        }

        private SpeechmaticsClient _smClient;
        private readonly List<Job> _jobs;
        private readonly List<AlignmentJobInputData> _inputData;

        public Align()
        {
            InitializeComponent();
            gbJob.Enabled = false;
            _jobs = new List<Job>();
            _inputData = new List<AlignmentJobInputData>();
        }

        private void btnUploadFile_MouseClick(object sender, MouseEventArgs e)
        {
            ofdUploadFile.Title = "Select audio file to be aligned";
            var drAudio = ofdUploadFile.ShowDialog();
            if (drAudio == DialogResult.OK)
            {
                var audioFilePath = Path.GetFullPath(ofdUploadFile.FileName);
                ofdUploadFile.Title = "Select text file to be aligned";
                var drText = ofdUploadFile.ShowDialog();
                if (drText == DialogResult.OK)
                {
                    var textFilePath = Path.GetFullPath(ofdUploadFile.FileName);
                    _inputData.Add(new AlignmentJobInputData
                    {
                        AudioFile = audioFilePath,
                        TextFile = textFilePath
                    });

                    Cursor = Cursors.WaitCursor;
                    try
                    {
                        StartJobs();
                    }
                    finally
                    {
                        Cursor = Cursors.Default;
                    }
                }
            }

        }

        private void btnUploadDir_MouseClick(object sender, MouseEventArgs e)
        {
            var dr = fbdUploadDir.ShowDialog();
            if (dr != DialogResult.OK) return;

            Cursor = Cursors.WaitCursor;
            try
            {
                var allFiles = Directory.GetFiles(fbdUploadDir.SelectedPath);
                var validAudioFiles = allFiles.Where(IsAudio);

                foreach (var filename in validAudioFiles)
                {
                    var shortName = Path.GetFileNameWithoutExtension(filename);
                    var matchingTextFile = allFiles.FirstOrDefault(c =>
                        c != filename && Path.GetFileNameWithoutExtension(c) == shortName);
                    if (matchingTextFile != null)
                    {
                        _inputData.Add(new AlignmentJobInputData
                        {
                            AudioFile = filename,
                            TextFile = matchingTextFile
                        });
                    }
                }

                lblDirName.Text = fbdUploadDir.SelectedPath;
                StartJobs();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private static bool IsAudio(string filename)
        {
            var ext = Path.GetExtension(filename).ToLowerInvariant();
            var allowedExt = new[] {".wav", ".mp3", ".mp4", ".wma", ".ogg"};
            return allowedExt.Any(t => ext == t);
        }

        private void StartJobs()
        {
            langComboBox.Enabled = false;
            tabType.Enabled = false;
            gbUser.Enabled = false;
            rtbOutput.Text =
                "Your job is currently being aligned.\r\nPlease wait - when it is finished your alignment(s) will automatically be displayed here.\n";
            lblJobStatus.Text = "Job status: in progress.";
            lblJobStatus.ForeColor = Color.Teal;
            foreach (var data in _inputData)
            {
                var resp = _smClient.CreateAlignmentJob(data.AudioFile, data.TextFile, langComboBox.Text);
                if (resp != null)
                {
                    _jobs.Add(resp.Job);
                }
                else
                {
                    _jobs.Add(new Job(0, 0)
                    {
                        Status = "done",
                        Name = data.AudioFile
                    });
                    rtbOutput.Text += "Error uploading file " + data.AudioFile + "\n";
                }
            }
            CheckCurrentJobs();
            tmrStatus.Interval = 10000;
            tmrStatus.Enabled = true;
        }

        private void InvalidCredentials()
        {
            connectStatusLabel.Text = "Connection status: Invalid User ID";
            lblBalanceValue.Text = "Not Connected";
            lblEmailValue.Text = "Not Connected";
            gbJob.Enabled = false;
        }

        private void RefreshClientCredentials()
        {
            if (string.IsNullOrEmpty(tbUserId.Text) ||
                string.IsNullOrEmpty(tbAuthToken.Text) ||
                !int.TryParse(tbUserId.Text, out var userId))
            {
                InvalidCredentials();
                return;
            }

            _smClient = new SpeechmaticsClient(userId, tbAuthToken.Text);
            connectStatusLabel.Text = "Connection status: Not Connected";
            connectStatusLabel.ForeColor = Color.DarkOrange;
            gbJob.Enabled = false;
            User user;
            Cursor = Cursors.WaitCursor;
            try
            {
                user = _smClient.GetUser();
                if (user != null)
                {
                    gbJob.Enabled = true;
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }
            if (null == user)
            {
                connectStatusLabel.Text = "Connection status: Invalid Auth Token";
                lblBalanceValue.Text = "Not Connected";
                lblEmailValue.Text = "Not Connected";
                gbJob.Enabled = false;
                return;
            }
            connectStatusLabel.Text = "Connection status: Connected";
            lblBalanceValue.Text = FormatBalance(user.Balance);
            lblEmailValue.Text = user.Email;
            connectStatusLabel.ForeColor = Color.Green;
        }

        private void CheckCurrentJobs()
        {
            var outputText = new StringBuilder();

            foreach (var job in _jobs)
            {
                if (job.Status != "done")
                {
                    _smClient.UpdateJobStatus(job);
                }

                outputText.AppendLine($"Job {job.Name} status {job.Status}");

                if (job.Status == "done")
                {
                    outputText.AppendLine("Output:");
                    outputText.AppendLine(_smClient.GetAlignment(job, true));

                    using (var sw = new StreamWriter(Path.ChangeExtension(job.Name, "word-timings.txt")))
                    {
                        sw.Write(_smClient.GetAlignment(job, false));
                    }
                    using (var sw = new StreamWriter(Path.ChangeExtension(job.Name, "line-timings.txt")))
                    {
                        sw.Write(_smClient.GetAlignment(job, true));
                    }
                }

                outputText.AppendLine("---");
            }

            if (_jobs.All(job => job.Status == "done"))
            {
                UnlockClient();
                lblJobStatus.Text = "Job status: All jobs complete.";
                lblJobStatus.ForeColor = Color.Green;
            }

            rtbOutput.Text = outputText.ToString();
        }

        private void UnlockClient()
        {
            langComboBox.Enabled = true;
            tabType.Enabled = true;
            gbUser.Enabled = true;
            tmrStatus.Enabled = false;
        }

        private static string FormatBalance(int balance)
        {
            return $"{balance} credits";
        }


        private void tmrStatus_Tick(object sender, EventArgs e)
        {
            CheckCurrentJobs();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            RefreshClientCredentials();
        }
    }
}
