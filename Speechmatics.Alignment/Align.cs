using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Speechmatics.API;

namespace Speechmatics.Transcription
{
    public partial class Align : Form
    {
        public Align()
        {
            InitializeComponent();
            _sc = null;
            gbJob.Enabled = false;
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
                if (drAudio == DialogResult.OK)
                {
                    var textFilePath = Path.GetFullPath(ofdUploadFile.FileName);
                    _audioFiles = new string[1];
                    _textFiles = new string[1];
                    _jobs = new Job[1];
                    _outputs = new string[1];
                    _audioFiles[0] = audioFilePath;
                    _textFiles[0] = textFilePath;
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
            if (dr == DialogResult.OK)
            {
                Cursor = Cursors.WaitCursor;
                try
                {
                    _files = Directory.GetFiles(fbdUploadDir.SelectedPath);
                    var audioCount = 0;
                    for (var i = 0; i < _files.Length; i++ )
                    {
                        if (IsAudio(_files[i]) && GetText(_files[i]) != null)
                        {
                            audioCount++;
                        }
                    }
                    _audioFiles = new string[audioCount];
                    _textFiles = new string[audioCount];
                    audioCount = 0;
                    for (var i = 0; i < _files.Length; i++)
                    {
                        if (IsAudio(_files[i]) && GetText(_files[i]) != null)
                        {
                            _audioFiles[audioCount] = _files[i];
                            _textFiles[audioCount] = GetText(_files[i]);
                            audioCount++;
                        }
                    }
                    _jobs = new Job[audioCount];
                    _outputs = new string[audioCount];
                    lblDirName.Text = fbdUploadDir.SelectedPath;
                    StartJobs();
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }

        }

        private static bool IsAudio(string filename)
        {
            var ext = Path.GetExtension(filename);
            var allowedExt = new String[5] { ".wav", ".mp3", ".mp4", ".wma", ".ogg" };
            for (var j=0; j<allowedExt.Length; j++)
            {
                if(ext.Equals(allowedExt[j], StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        private string GetText(string filename)
        {
            var basename = Path.GetFileNameWithoutExtension(filename);
            for (var i=0;i<_files.Length;i++)
            {
                if((! _files[i].Equals(filename)) && (basename.Equals(Path.GetFileNameWithoutExtension(_files[i]))))
                {
                    return _files[i];
                }
            }
                return null;
        }

        private void StartJobs()
        {
            langComboBox.Enabled = false;
            tabType.Enabled = false;
            gbUser.Enabled = false;
            lblJobStatus.ForeColor = Color.Teal;
            rtbOutput.Text = "Your job is currently being aligned.\r\nPlease wait - when it is finished your alignment(s) will automatically be displayed here.\n";
            lblJobStatus.Text = "Job status: in progress.";
            lblJobStatus.ForeColor = Color.Teal;
            for (var i = 0; i < _audioFiles.Length; i++)
            {
                if (_audioFiles[i].Equals(_textFiles[i]))
                {
                    _jobs[i] = new Job(0, 0);
                    _jobs[i].Status = "done";
                    _jobs[i].Name = _audioFiles[i];
                    rtbOutput.Text += "Error uploading file " + _audioFiles[i] + "\n";
                    _outputs[i] = "Text and audio are the same file - cannot align\n";
                }
                else
                {
                    var resp = _sc.CreateAlignmentJob(_audioFiles[i], _textFiles[i], langComboBox.Text);
                    if (resp != null)
                    {
                        _jobs[i] = resp.Job;
                    }
                    else
                    {
                        _jobs[i] = new Job(0, 0);
                        _jobs[i].Status = "done";
                        _jobs[i].Name = _audioFiles[i];
                        rtbOutput.Text += "Error uploading file " + _audioFiles[i] + "\n";
                        _outputs[i] = "Error uploading file " + _audioFiles[i] + "\n";
                    }
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
        private string[] _audioFiles;
        private string[] _textFiles;
        
        private void RefreshClientCredentials()
        {
            if (!string.IsNullOrEmpty(tbUserId.Text) && !string.IsNullOrEmpty(tbAuthToken.Text))
            {
                var userId = -1;
                if (Int32.TryParse(tbUserId.Text, out userId))
                {
                    _sc = new SpeechmaticsClient(userId, tbAuthToken.Text);
                    connectStatusLabel.Text = "Connection status: Not Connected";
                    connectStatusLabel.ForeColor = Color.DarkOrange;
                    gbJob.Enabled = false;  
                    User user = null;
                    Cursor = Cursors.WaitCursor;
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
                        Cursor = Cursors.Default;
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
                        connectStatusLabel.ForeColor = Color.Green;
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
            rtbOutput.Text = "";
            var completeCount = 0;
            for (var i = 0; i < _jobs.Length; i++)
            {
                if (_jobs[i].Status != "done")
                {
                    _sc.UpdateJobStatus(_jobs[i]);
                    if (_jobs[i].Status == "done")
                    {
                        _outputs[i] = _sc.GetAlignment(_jobs[i], true);
                        var ext = Path.GetExtension(_textFiles[i]);
                        var sw = new StreamWriter(Path.ChangeExtension(_textFiles[i], "word-timings" + ext));
                        sw.Write(_sc.GetAlignment(_jobs[i], false));
                        sw.Close();
                        sw = new StreamWriter(Path.ChangeExtension(_audioFiles[i], "line-timings" + ext));
                        sw.Write(_sc.GetAlignment(_jobs[i], true));
                        sw.Close();
                    }
                }

                rtbOutput.Text += "Job " + _jobs[i].Name + " status " + _jobs[i].Status;
                if (_jobs[i].Status == "done")
                {
                    completeCount++;
                    rtbOutput.Text += "\nOutput:\n";
                    rtbOutput.Text += _outputs[i];
                }
                rtbOutput.Text += "\n---\n";
            }
            if (completeCount == _jobs.Length)
            {
                UnlockClient();
                lblJobStatus.Text = "Job status: All jobs complete.";
                lblJobStatus.ForeColor = Color.Green;
            }

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
