using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Speechmatics.API;

namespace Speechmatics.Transcription
{
    public partial class Transcribe : Form
    {
        public Transcribe()
        {
            InitializeComponent();
            _sc = null;
            gbJob.Enabled = false;
        }

        private void btnUploadFile_MouseClick(object sender, MouseEventArgs e)
        {
            ofdUploadFile.Title = "Select audio file to be transcribed";
            var dr = ofdUploadFile.ShowDialog();
            if (dr == DialogResult.OK)
            {
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    _files = new string[1];
                    _jobs = new Job[1];
                    _outputs = new string[1];
                    _files[0] = Path.GetFullPath(ofdUploadFile.FileName);
                    lblJobName.Text = _files[0];
                    StartJobs();
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void btnUploadDirectory_MouseClick(object sender, MouseEventArgs e)
        {
            var dr = fbdUploadDir.ShowDialog();
            if (dr == DialogResult.OK)
            {
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    _files = Directory.GetFiles(fbdUploadDir.SelectedPath);
                    _jobs = new Job[_files.Length];
                    _outputs = new string[_files.Length];
                    lblDirName.Text = fbdUploadDir.SelectedPath;
                    StartJobs();
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }

        }

        private void StartJobs(){           
            langComboBox.Enabled = false;
            tabType.Enabled = false;
            gbUser.Enabled = false;
            cbxDiarise.Enabled = false;
            rtbTranscript.Text = "Your job(s) are currently being transcribed.\r\nPlease wait - when finished your transcription(s) will automatically be displayed here.\n";
            lblJobStatus.Text = "Job status: in progress.";
            lblJobStatus.ForeColor = System.Drawing.Color.Teal;
            for(var i=0; i < _files.Length ; i++)
            {
                var resp = _sc.CreateTranscriptionJob(_files[i], langComboBox.Text, cbxDiarise.Checked);
                if (resp != null)
                {                       
                    _jobs[i] = resp.Job;                       
                }
                else
                {
                    _jobs[i] = new Job(0, 0);
                    _jobs[i].Status = "done";
                    _jobs[i].Name = _files[i];
                    rtbTranscript.Text += "Error uploading file " + _files[i] + "\n";
                    _outputs[i] = "Error uploading file " + _files[i] + "\n";
                }
            }
            CheckCurrentJobs();
            tmrStatus.Interval = 10000;
            tmrStatus.Enabled = true;
        }

        private SpeechmaticsClient _sc;
        private Job[] _jobs;
        private string[] _outputs;       
        private string[] _files;
        
        private void RefreshClientCredentials()
        {
            if (!string.IsNullOrEmpty(tbUserId.Text) && !string.IsNullOrEmpty(tbAuthToken.Text))
            {
                var userId = -1;
                if (Int32.TryParse(tbUserId.Text, out userId))
                {
                    _sc = new SpeechmaticsClient(userId, tbAuthToken.Text);
                    connectStatusLabel.Text = "Connection status: Not Connected";
                    this.connectStatusLabel.ForeColor = System.Drawing.Color.DarkOrange;
                    gbJob.Enabled = false;  
                    User user = null;
                    this.Cursor = Cursors.WaitCursor;
                    try
                    {
                        user = _sc.GetUser();
                        if (user != null)
                        {
                            gbJob.Enabled = true;
                        }
                    }
                    finally
                    {
                        this.Cursor = Cursors.Default;
                    }
                    if (null == user)
                    {
                        connectStatusLabel.Text = "Connection status: Invalid Auth Token";
                        lblBalanceValue.Text = "Not Connected";
                        lblEmailValue.Text = "Not Connected";
                        gbJob.Enabled = false;
                    }
                    else
                    {
                        connectStatusLabel.Text = "Connection status: Connected";
                        lblBalanceValue.Text = FormatBalance(user.Balance);
                        lblEmailValue.Text = user.Email;
                        connectStatusLabel.ForeColor = System.Drawing.Color.Green;
                    }
                }
                else
                {
                    connectStatusLabel.Text = "Connection status: Invalid User ID";
                    lblBalanceValue.Text = "Not Connected";
                    lblEmailValue.Text = "Not Connected";
                    gbJob.Enabled = false;
                }
            }
        }

        private void CheckCurrentJobs()
        {
            rtbTranscript.Text = "";
            var completeCount = 0;
            for (var i = 0; i < _jobs.Length; i++)
            {               
                if (_jobs[i].Status != "done")
                { 
                    _sc.UpdateJobStatus(_jobs[i]);
                    if (_jobs[i].Status == "done")
                    {
                        _outputs[i] = _sc.GetTranscript(_jobs[i], "txt");
                        var sw = new System.IO.StreamWriter(Path.ChangeExtension(_files[i], ".txt"));
                        sw.Write(_outputs[i]);
                        sw.Close();
                        sw = new System.IO.StreamWriter(Path.ChangeExtension(_files[i], ".json"));
                        sw.Write(_sc.GetTranscript(_jobs[i], "json"));
                        sw.Close();
                    }
                }
                
                rtbTranscript.Text += "Job " + _jobs[i].Name + " status " + _jobs[i].Status;
                if (_jobs[i].Status == "done")
                {
                    completeCount++;
                    rtbTranscript.Text += "\nOutput:\n";
                    rtbTranscript.Text += _outputs[i];
                }
                rtbTranscript.Text += "\n---\n";
             }
            if (completeCount == _jobs.Length)
            {
                UnlockClient();
                lblJobStatus.Text = "Job status: All jobs complete.";
                lblJobStatus.ForeColor = System.Drawing.Color.Green;
            }

        }

        private void UnlockClient()
        {
            tmrStatus.Enabled = false; 
            langComboBox.Enabled = true;
            tabType.Enabled = true;
            gbUser.Enabled = true;
            cbxDiarise.Enabled = true;
        }

        private string FormatBalance(int balance)
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
