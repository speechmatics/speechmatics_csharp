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
            sc = null;
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
                    files = new string[1];
                    jobs = new Job[1];
                    outputs = new string[1];
                    files[0] = Path.GetFullPath(ofdUploadFile.FileName);
                    lblJobName.Text = files[0];
                    startJobs();
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
                    files = Directory.GetFiles(fbdUploadDir.SelectedPath);
                    jobs = new Job[files.Length];
                    outputs = new string[files.Length];
                    lblDirName.Text = fbdUploadDir.SelectedPath;
                    startJobs();
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }

        }

        private void startJobs(){           
            langComboBox.Enabled = false;
            tabType.Enabled = false;
            gbUser.Enabled = false;
            cbxDiarise.Enabled = false;
            rtbTranscript.Text = "Your job(s) are currently being transcribed.\r\nPlease wait - when finished your transcription(s) will automatically be displayed here.\n";
            lblJobStatus.Text = "Job status: in progress.";
            lblJobStatus.ForeColor = System.Drawing.Color.Teal;
            for(var i=0; i < files.Length ; i++)
            {
                var resp = sc.CreateTranscriptionJob(files[i], langComboBox.Text, cbxDiarise.Checked);
                if (resp != null)
                {                       
                    jobs[i] = resp.Job;                       
                }
                else
                {
                    jobs[i] = new Job(0, 0);
                    jobs[i].Status = "done";
                    jobs[i].Name = files[i];
                    rtbTranscript.Text += "Error uploading file " + files[i] + "\n";
                    outputs[i] = "Error uploading file " + files[i] + "\n";
                }
            }
            checkCurrentJobs();
            tmrStatus.Interval = 10000;
            tmrStatus.Enabled = true;
        }

        private SpeechmaticsClient sc;
        private Job[] jobs;
        private string[] outputs;       
        private string[] files;
        
        private void refreshClientCredentials()
        {
            if (!string.IsNullOrEmpty(tbUserId.Text) && !string.IsNullOrEmpty(tbAuthToken.Text))
            {
                var userId = -1;
                if (Int32.TryParse(tbUserId.Text, out userId))
                {
                    sc = new SpeechmaticsClient(userId, tbAuthToken.Text);
                    connectStatusLabel.Text = "Connection status: Not Connected";
                    this.connectStatusLabel.ForeColor = System.Drawing.Color.DarkOrange;
                    gbJob.Enabled = false;  
                    User user = null;
                    this.Cursor = Cursors.WaitCursor;
                    try
                    {
                        user = sc.GetUser();
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
                        lblBalanceValue.Text = formatBalance(user.Balance);
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

        private void checkCurrentJobs()
        {
            rtbTranscript.Text = "";
            var completeCount = 0;
            for (var i = 0; i < jobs.Length; i++)
            {               
                if (jobs[i].Status != "done")
                { 
                    sc.UpdateJobStatus(jobs[i]);
                    if (jobs[i].Status == "done")
                    {
                        outputs[i] = sc.getTranscript(jobs[i], "txt");
                        var sw = new System.IO.StreamWriter(Path.ChangeExtension(files[i], ".txt"));
                        sw.Write(outputs[i]);
                        sw.Close();
                        sw = new System.IO.StreamWriter(Path.ChangeExtension(files[i], ".json"));
                        sw.Write(sc.getTranscript(jobs[i], "json"));
                        sw.Close();
                    }
                }
                
                rtbTranscript.Text += "Job " + jobs[i].Name + " status " + jobs[i].Status;
                if (jobs[i].Status == "done")
                {
                    completeCount++;
                    rtbTranscript.Text += "\nOutput:\n";
                    rtbTranscript.Text += outputs[i];
                }
                rtbTranscript.Text += "\n---\n";
             }
            if (completeCount == jobs.Length)
            {
                unlockClient();
                lblJobStatus.Text = "Job status: All jobs complete.";
                lblJobStatus.ForeColor = System.Drawing.Color.Green;
            }

        }

        private void unlockClient()
        {
            tmrStatus.Enabled = false; 
            langComboBox.Enabled = true;
            tabType.Enabled = true;
            gbUser.Enabled = true;
            cbxDiarise.Enabled = true;
        }

        private string formatBalance(int balance)
        {
            return $"{balance} credits";
        }


        private void tmrStatus_Tick(object sender, EventArgs e)
        {
            checkCurrentJobs();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            refreshClientCredentials();
        }
    }
}
