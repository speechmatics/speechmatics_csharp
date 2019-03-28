using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Speechmatics.API;

namespace Speechmatics.Transcription
{
    public partial class Transcribe : Form
    {
        private SpeechmaticsClient _smClient;
        private readonly List<Job> _jobs;
        private readonly List<string> _files;

        public Transcribe()
        {
            InitializeComponent();
            gbJob.Enabled = false;
            _files = new List<string>();
            _jobs = new List<Job>();
        }

        private void btnUploadFile_MouseClick(object sender, MouseEventArgs e)
        {
            ofdUploadFile.Title = "Select audio file to be transcribed";
            var dr = ofdUploadFile.ShowDialog();
            if (dr != DialogResult.OK) return;

            Cursor = Cursors.WaitCursor;
            try
            {
                var file = Path.GetFullPath(ofdUploadFile.FileName);
                _files.Add(file);
                lblJobName.Text = file;
                StartJobs();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void btnUploadDirectory_MouseClick(object sender, MouseEventArgs e)
        {
            var dr = fbdUploadDir.ShowDialog();
            if (dr != DialogResult.OK) return;

            Cursor = Cursors.WaitCursor;
            try
            {
                _files.AddRange(Directory.GetFiles(fbdUploadDir.SelectedPath));
                lblDirName.Text = fbdUploadDir.SelectedPath;
                StartJobs();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void StartJobs()
        {
            langComboBox.Enabled = false;
            tabType.Enabled = false;
            gbUser.Enabled = false;
            cbxDiarize.Enabled = false;
            rtbTranscript.Text = "Your job(s) are currently being transcribed.\r\nPlease wait - when finished your transcription(s) will automatically be displayed here.\n";
            lblJobStatus.Text = "Job status: in progress.";
            lblJobStatus.ForeColor = Color.Teal;

            foreach(var file in _files)
            { 
                var resp = _smClient.CreateTranscriptionJob(file, langComboBox.Text, cbxDiarize.Checked);
                if (resp != null)
                {
                    _jobs.Add(resp.Job);
                }
                else
                {
                    _jobs.Add(new Job(0, 0)
                    {
                        Status = "done",
                        Name = file
                    });
                    rtbTranscript.Text += $"Error uploading file ${file}\n";
                }
            }
            CheckCurrentJobs();
            tmrStatus.Interval = 10000;
            tmrStatus.Enabled = true;
        }

        private void InvalidCredentials(string reason)
        {
            connectStatusLabel.Text = reason;
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
                InvalidCredentials("Connection status: Invalid User ID");
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
                InvalidCredentials("Connection status: Invalid Auth Token");
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
                    outputText.AppendLine(_smClient.GetTranscript(job, "txt"));

                    using (var sw = new StreamWriter(Path.ChangeExtension(job.Name, ".json")))
                    {
                        sw.Write(_smClient.GetTranscript(job, "json"));
                    }
                    using (var sw = new StreamWriter(Path.ChangeExtension(job.Name, ".txt")))
                    {
                        sw.Write(_smClient.GetTranscript(job, "txt"));
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

            rtbTranscript.Text = outputText.ToString();
        }

        private void UnlockClient()
        {
            tmrStatus.Enabled = false;
            langComboBox.Enabled = true;
            tabType.Enabled = true;
            gbUser.Enabled = true;
            cbxDiarize.Enabled = true;
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
