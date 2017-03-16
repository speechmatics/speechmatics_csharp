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
    public partial class Align : Form
    {
        public Align()
        {
            InitializeComponent();
            sc = null;
            gbJob.Enabled = false;
        }

        private void btnUploadFile_MouseClick(object sender, MouseEventArgs e)
        {
            ofdUploadFile.Title = "Select audio file to be aligned";
            DialogResult drAudio = ofdUploadFile.ShowDialog();
            if (drAudio == DialogResult.OK)
            {
                string audioFilePath = Path.GetFullPath(ofdUploadFile.FileName);
                ofdUploadFile.Title = "Select text file to be aligned";
                DialogResult drText = ofdUploadFile.ShowDialog();
                if (drAudio == DialogResult.OK)
                {
                    string textFilePath = Path.GetFullPath(ofdUploadFile.FileName);
                    audio_files = new string[1];
                    text_files = new string[1];
                    jobs = new Job[1];
                    outputs = new string[1];
                    audio_files[0] = audioFilePath;
                    text_files[0] = textFilePath;
                    this.Cursor = Cursors.WaitCursor;
                    try
                    {
                        startJobs();
                    }
                    finally
                    {
                        this.Cursor = Cursors.Default;
                    }
                }
            }

        }

        private void btnUploadDir_MouseClick(object sender, MouseEventArgs e)
        {
            DialogResult dr = fbdUploadDir.ShowDialog();
            if (dr == DialogResult.OK)
            {
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    files = Directory.GetFiles(fbdUploadDir.SelectedPath);
                    int audio_count = 0;
                    for (int i = 0; i < files.Length; i++ )
                    {
                        if (isAudio(files[i]) && getText(files[i]) != null)
                        {
                            audio_count++;
                        }
                    }
                    audio_files = new string[audio_count];
                    text_files = new string[audio_count];
                    audio_count = 0;
                    for (int i = 0; i < files.Length; i++)
                    {
                        if (isAudio(files[i]) && getText(files[i]) != null)
                        {
                            audio_files[audio_count] = files[i];
                            text_files[audio_count] = getText(files[i]);
                            audio_count++;
                        }
                    }
                    jobs = new Job[audio_count];
                    outputs = new string[audio_count];
                    lblDirName.Text = fbdUploadDir.SelectedPath;
                    startJobs();
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }

        }

        private bool isAudio(string filename)
        {
            string ext = Path.GetExtension(filename);
            string[] allowedExt = new String[5] { ".wav", ".mp3", ".mp4", ".wma", ".ogg" };
            for (int j=0; j<allowedExt.Length; j++)
            {
                if(ext.Equals(allowedExt[j], StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        private string getText(string filename)
        {
            string basename = Path.GetFileNameWithoutExtension(filename);
            for (int i=0;i<files.Length;i++)
            {
                if((! files[i].Equals(filename)) && (basename.Equals(Path.GetFileNameWithoutExtension(files[i]))))
                {
                    return files[i];
                }
            }
                return null;
        }

        private void startJobs()
        {
            langComboBox.Enabled = false;
            tabType.Enabled = false;
            gbUser.Enabled = false;
            lblJobStatus.ForeColor = System.Drawing.Color.Teal;
            rtbOutput.Text = "Your job is currently being aligned.\r\nPlease wait - when it is finished your alignment(s) will automatically be displayed here.\n";
            lblJobStatus.Text = "Job status: in progress.";
            lblJobStatus.ForeColor = System.Drawing.Color.Teal;
            for (int i = 0; i < audio_files.Length; i++)
            {
                if (audio_files[i].Equals(text_files[i]))
                {
                    jobs[i] = new Job(0, 0);
                    jobs[i].Status = "done";
                    jobs[i].Name = audio_files[i];
                    rtbOutput.Text += "Error uploading file " + audio_files[i] + "\n";
                    outputs[i] = "Text and audio are the same file - cannot align\n";
                }
                else
                {
                    CreateJobResponse resp = sc.CreateAlignmentJob(audio_files[i], text_files[i], langComboBox.Text);
                    if (resp != null)
                    {
                        jobs[i] = resp.Job;
                    }
                    else
                    {
                        jobs[i] = new Job(0, 0);
                        jobs[i].Status = "done";
                        jobs[i].Name = audio_files[i];
                        rtbOutput.Text += "Error uploading file " + audio_files[i] + "\n";
                        outputs[i] = "Error uploading file " + audio_files[i] + "\n";
                    }
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
        private string[] audio_files;
        private string[] text_files;
        
        private void refreshClientCredentials()
        {
            if (!string.IsNullOrEmpty(tbUserId.Text) && !string.IsNullOrEmpty(tbAuthToken.Text))
            {
                int userId = -1;
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
            rtbOutput.Text = "";
            int completeCount = 0;
            for (int i = 0; i < jobs.Length; i++)
            {
                if (jobs[i].Status != "done")
                {
                    sc.UpdateJobStatus(jobs[i]);
                    if (jobs[i].Status == "done")
                    {
                        outputs[i] = sc.getAlignment(jobs[i], true);
                        string ext = Path.GetExtension(text_files[i]);
                        System.IO.StreamWriter sw = new System.IO.StreamWriter(Path.ChangeExtension(text_files[i], "word-timings" + ext));
                        sw.Write(sc.getAlignment(jobs[i], false));
                        sw.Close();
                        sw = new System.IO.StreamWriter(Path.ChangeExtension(audio_files[i], "line-timings" + ext));
                        sw.Write(sc.getAlignment(jobs[i], true));
                        sw.Close();
                    }
                }

                rtbOutput.Text += "Job " + jobs[i].Name + " status " + jobs[i].Status;
                if (jobs[i].Status == "done")
                {
                    completeCount++;
                    rtbOutput.Text += "\nOutput:\n";
                    rtbOutput.Text += outputs[i];
                }
                rtbOutput.Text += "\n---\n";
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
            langComboBox.Enabled = true;
            tabType.Enabled = true;
            gbUser.Enabled = true;
            tmrStatus.Enabled = false;
        }

        private string formatBalance(int balance)
        {
            return string.Format("{0} credits", balance);
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
