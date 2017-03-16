namespace Speechmatics.Transcription
{
    partial class Align
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Align));
            this.ofdUploadFile = new System.Windows.Forms.OpenFileDialog();
            this.fbdUploadDir = new System.Windows.Forms.FolderBrowserDialog();
            this.btnUploadFile = new System.Windows.Forms.Button();
            this.lblUserId = new System.Windows.Forms.Label();
            this.lblAuthToken = new System.Windows.Forms.Label();
            this.tbUserId = new System.Windows.Forms.TextBox();
            this.tbAuthToken = new System.Windows.Forms.TextBox();
            this.lblBalance = new System.Windows.Forms.Label();
            this.lblBalanceValue = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblEmailValue = new System.Windows.Forms.Label();
            this.lblCurrentJob = new System.Windows.Forms.Label();
            this.lblJobName = new System.Windows.Forms.Label();
            this.rtbOutput = new System.Windows.Forms.RichTextBox();
            this.tmrStatus = new System.Windows.Forms.Timer(this.components);
            this.btnConnect = new System.Windows.Forms.Button();
            this.lblTranscription = new System.Windows.Forms.Label();
            this.gbUser = new System.Windows.Forms.GroupBox();
            this.connectStatusLabel = new System.Windows.Forms.Label();
            this.loginLabel = new System.Windows.Forms.Label();
            this.gbJob = new System.Windows.Forms.GroupBox();
            this.tabType = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnUploadDir = new System.Windows.Forms.Button();
            this.lblCurrentDir = new System.Windows.Forms.Label();
            this.lblDirName = new System.Windows.Forms.Label();
            this.lblJobStatus = new System.Windows.Forms.Label();
            this.langComboBox = new System.Windows.Forms.ComboBox();
            this.langLabel = new System.Windows.Forms.Label();
            this.gbUser.SuspendLayout();
            this.gbJob.SuspendLayout();
            this.tabType.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnUploadFile
            // 
            this.btnUploadFile.Location = new System.Drawing.Point(351, 6);
            this.btnUploadFile.Name = "btnUploadFile";
            this.btnUploadFile.Size = new System.Drawing.Size(107, 45);
            this.btnUploadFile.TabIndex = 3;
            this.btnUploadFile.Text = "Select Files";
            this.btnUploadFile.UseVisualStyleBackColor = true;
            this.btnUploadFile.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnUploadFile_MouseClick);
            // 
            // lblUserId
            // 
            this.lblUserId.AutoSize = true;
            this.lblUserId.Location = new System.Drawing.Point(45, 25);
            this.lblUserId.Name = "lblUserId";
            this.lblUserId.Size = new System.Drawing.Size(47, 13);
            this.lblUserId.TabIndex = 4;
            this.lblUserId.Text = "User ID:";
            // 
            // lblAuthToken
            // 
            this.lblAuthToken.AutoSize = true;
            this.lblAuthToken.Location = new System.Drawing.Point(6, 57);
            this.lblAuthToken.Name = "lblAuthToken";
            this.lblAuthToken.Size = new System.Drawing.Size(86, 13);
            this.lblAuthToken.TabIndex = 5;
            this.lblAuthToken.Text = "API Auth Token:";
            // 
            // tbUserId
            // 
            this.tbUserId.Location = new System.Drawing.Point(94, 22);
            this.tbUserId.Name = "tbUserId";
            this.tbUserId.Size = new System.Drawing.Size(140, 21);
            this.tbUserId.TabIndex = 1;
            // 
            // tbAuthToken
            // 
            this.tbAuthToken.Location = new System.Drawing.Point(94, 54);
            this.tbAuthToken.Name = "tbAuthToken";
            this.tbAuthToken.Size = new System.Drawing.Size(140, 21);
            this.tbAuthToken.TabIndex = 2;
            // 
            // lblBalance
            // 
            this.lblBalance.AutoSize = true;
            this.lblBalance.Location = new System.Drawing.Point(251, 57);
            this.lblBalance.Name = "lblBalance";
            this.lblBalance.Size = new System.Drawing.Size(48, 13);
            this.lblBalance.TabIndex = 8;
            this.lblBalance.Text = "Balance:";
            // 
            // lblBalanceValue
            // 
            this.lblBalanceValue.AutoSize = true;
            this.lblBalanceValue.Location = new System.Drawing.Point(312, 57);
            this.lblBalanceValue.Name = "lblBalanceValue";
            this.lblBalanceValue.Size = new System.Drawing.Size(79, 13);
            this.lblBalanceValue.TabIndex = 9;
            this.lblBalanceValue.Text = "Not Connected";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(251, 25);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(35, 13);
            this.lblEmail.TabIndex = 10;
            this.lblEmail.Text = "Email:";
            // 
            // lblEmailValue
            // 
            this.lblEmailValue.AutoSize = true;
            this.lblEmailValue.Location = new System.Drawing.Point(312, 25);
            this.lblEmailValue.Name = "lblEmailValue";
            this.lblEmailValue.Size = new System.Drawing.Size(79, 13);
            this.lblEmailValue.TabIndex = 11;
            this.lblEmailValue.Text = "Not Connected";
            // 
            // lblCurrentJob
            // 
            this.lblCurrentJob.AutoSize = true;
            this.lblCurrentJob.Location = new System.Drawing.Point(6, 22);
            this.lblCurrentJob.Name = "lblCurrentJob";
            this.lblCurrentJob.Size = new System.Drawing.Size(57, 13);
            this.lblCurrentJob.TabIndex = 12;
            this.lblCurrentJob.Text = "File Name:";
            // 
            // lblJobName
            // 
            this.lblJobName.AutoSize = true;
            this.lblJobName.Location = new System.Drawing.Point(69, 22);
            this.lblJobName.Name = "lblJobName";
            this.lblJobName.Size = new System.Drawing.Size(32, 13);
            this.lblJobName.TabIndex = 13;
            this.lblJobName.Text = "None";
            // 
            // rtbOutput
            // 
            this.rtbOutput.Location = new System.Drawing.Point(21, 149);
            this.rtbOutput.Name = "rtbOutput";
            this.rtbOutput.ReadOnly = true;
            this.rtbOutput.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.rtbOutput.Size = new System.Drawing.Size(467, 135);
            this.rtbOutput.TabIndex = 15;
            this.rtbOutput.Text = "";
            // 
            // tmrStatus
            // 
            this.tmrStatus.Interval = 60000;
            this.tmrStatus.Tick += new System.EventHandler(this.tmrStatus_Tick);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(6, 116);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(488, 23);
            this.btnConnect.TabIndex = 16;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // lblTranscription
            // 
            this.lblTranscription.AutoSize = true;
            this.lblTranscription.Location = new System.Drawing.Point(22, 130);
            this.lblTranscription.Name = "lblTranscription";
            this.lblTranscription.Size = new System.Drawing.Size(41, 13);
            this.lblTranscription.TabIndex = 17;
            this.lblTranscription.Text = "Output";
            // 
            // gbUser
            // 
            this.gbUser.Controls.Add(this.connectStatusLabel);
            this.gbUser.Controls.Add(this.loginLabel);
            this.gbUser.Controls.Add(this.lblAuthToken);
            this.gbUser.Controls.Add(this.btnConnect);
            this.gbUser.Controls.Add(this.lblUserId);
            this.gbUser.Controls.Add(this.tbUserId);
            this.gbUser.Controls.Add(this.lblBalanceValue);
            this.gbUser.Controls.Add(this.lblEmail);
            this.gbUser.Controls.Add(this.lblBalance);
            this.gbUser.Controls.Add(this.lblEmailValue);
            this.gbUser.Controls.Add(this.tbAuthToken);
            this.gbUser.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbUser.Location = new System.Drawing.Point(12, 12);
            this.gbUser.Name = "gbUser";
            this.gbUser.Size = new System.Drawing.Size(500, 180);
            this.gbUser.TabIndex = 18;
            this.gbUser.TabStop = false;
            this.gbUser.Text = "USER DETAILS";
            // 
            // connectStatusLabel
            // 
            this.connectStatusLabel.AutoSize = true;
            this.connectStatusLabel.ForeColor = System.Drawing.Color.DarkOrange;
            this.connectStatusLabel.Location = new System.Drawing.Point(26, 151);
            this.connectStatusLabel.Name = "connectStatusLabel";
            this.connectStatusLabel.Size = new System.Drawing.Size(173, 13);
            this.connectStatusLabel.TabIndex = 18;
            this.connectStatusLabel.Text = "Connection status: Not Connected";
            // 
            // loginLabel
            // 
            this.loginLabel.AutoSize = true;
            this.loginLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loginLabel.Location = new System.Drawing.Point(26, 90);
            this.loginLabel.Name = "loginLabel";
            this.loginLabel.Size = new System.Drawing.Size(447, 13);
            this.loginLabel.TabIndex = 17;
            this.loginLabel.Text = "Login to https://www.speechmatics.com/account to find your \'User ID\' and \'API Aut" +
    "h Token\'";
            // 
            // gbJob
            // 
            this.gbJob.Controls.Add(this.tabType);
            this.gbJob.Controls.Add(this.lblJobStatus);
            this.gbJob.Controls.Add(this.langComboBox);
            this.gbJob.Controls.Add(this.langLabel);
            this.gbJob.Controls.Add(this.lblTranscription);
            this.gbJob.Controls.Add(this.rtbOutput);
            this.gbJob.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbJob.Location = new System.Drawing.Point(12, 198);
            this.gbJob.Name = "gbJob";
            this.gbJob.Size = new System.Drawing.Size(500, 303);
            this.gbJob.TabIndex = 19;
            this.gbJob.TabStop = false;
            this.gbJob.Text = "JOB DETAILS";
            // 
            // tabType
            // 
            this.tabType.Controls.Add(this.tabPage1);
            this.tabType.Controls.Add(this.tabPage2);
            this.tabType.Location = new System.Drawing.Point(21, 20);
            this.tabType.Name = "tabType";
            this.tabType.SelectedIndex = 0;
            this.tabType.Size = new System.Drawing.Size(469, 83);
            this.tabType.TabIndex = 23;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnUploadFile);
            this.tabPage1.Controls.Add(this.lblCurrentJob);
            this.tabPage1.Controls.Add(this.lblJobName);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(461, 57);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Single File";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnUploadDir);
            this.tabPage2.Controls.Add(this.lblCurrentDir);
            this.tabPage2.Controls.Add(this.lblDirName);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(461, 57);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Directory";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnUploadDir
            // 
            this.btnUploadDir.Location = new System.Drawing.Point(348, 6);
            this.btnUploadDir.Name = "btnUploadDir";
            this.btnUploadDir.Size = new System.Drawing.Size(107, 45);
            this.btnUploadDir.TabIndex = 14;
            this.btnUploadDir.Text = "Select Directory";
            this.btnUploadDir.UseVisualStyleBackColor = true;
            this.btnUploadDir.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnUploadDir_MouseClick);
            // 
            // lblCurrentDir
            // 
            this.lblCurrentDir.AutoSize = true;
            this.lblCurrentDir.Location = new System.Drawing.Point(6, 22);
            this.lblCurrentDir.Name = "lblCurrentDir";
            this.lblCurrentDir.Size = new System.Drawing.Size(85, 13);
            this.lblCurrentDir.TabIndex = 15;
            this.lblCurrentDir.Text = "Directory Name:";
            // 
            // lblDirName
            // 
            this.lblDirName.AutoSize = true;
            this.lblDirName.Location = new System.Drawing.Point(97, 22);
            this.lblDirName.Name = "lblDirName";
            this.lblDirName.Size = new System.Drawing.Size(32, 13);
            this.lblDirName.TabIndex = 16;
            this.lblDirName.Text = "None";
            // 
            // lblJobStatus
            // 
            this.lblJobStatus.AutoSize = true;
            this.lblJobStatus.ForeColor = System.Drawing.Color.DarkOrange;
            this.lblJobStatus.Location = new System.Drawing.Point(22, 287);
            this.lblJobStatus.Name = "lblJobStatus";
            this.lblJobStatus.Size = new System.Drawing.Size(139, 13);
            this.lblJobStatus.TabIndex = 21;
            this.lblJobStatus.Text = "Job status: None submitted";
            // 
            // langComboBox
            // 
            this.langComboBox.FormattingEnabled = true;
            this.langComboBox.Items.AddRange(new object[] {
            "en-GB",
            "en-US"});
            this.langComboBox.Location = new System.Drawing.Point(86, 103);
            this.langComboBox.Name = "langComboBox";
            this.langComboBox.Size = new System.Drawing.Size(56, 21);
            this.langComboBox.TabIndex = 20;
            this.langComboBox.Text = "en-US";
            // 
            // langLabel
            // 
            this.langLabel.AutoSize = true;
            this.langLabel.Location = new System.Drawing.Point(22, 106);
            this.langLabel.Name = "langLabel";
            this.langLabel.Size = new System.Drawing.Size(58, 13);
            this.langLabel.TabIndex = 19;
            this.langLabel.Text = "Language:";
            // 
            // Align
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(247)))), ((int)(((byte)(253)))));
            this.ClientSize = new System.Drawing.Size(523, 511);
            this.Controls.Add(this.gbUser);
            this.Controls.Add(this.gbJob);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Align";
            this.Text = "Speechmatics Alignment";
            this.gbUser.ResumeLayout(false);
            this.gbUser.PerformLayout();
            this.gbJob.ResumeLayout(false);
            this.gbJob.PerformLayout();
            this.tabType.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog ofdUploadFile;
        private System.Windows.Forms.FolderBrowserDialog fbdUploadDir;
        private System.Windows.Forms.Button btnUploadFile;
        private System.Windows.Forms.Label lblUserId;
        private System.Windows.Forms.Label lblAuthToken;
        private System.Windows.Forms.TextBox tbUserId;
        private System.Windows.Forms.TextBox tbAuthToken;
        private System.Windows.Forms.Label lblBalance;
        private System.Windows.Forms.Label lblBalanceValue;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblEmailValue;
        private System.Windows.Forms.Label lblCurrentJob;
        private System.Windows.Forms.Label lblJobName;
        private System.Windows.Forms.RichTextBox rtbOutput;
        private System.Windows.Forms.Timer tmrStatus;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label lblTranscription;
        private System.Windows.Forms.GroupBox gbUser;
        private System.Windows.Forms.GroupBox gbJob;
        private System.Windows.Forms.Label langLabel;
        private System.Windows.Forms.ComboBox langComboBox;
        private System.Windows.Forms.Label loginLabel;
        private System.Windows.Forms.Label connectStatusLabel;
        private System.Windows.Forms.Label lblJobStatus;
        private System.Windows.Forms.TabControl tabType;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnUploadDir;
        private System.Windows.Forms.Label lblCurrentDir;
        private System.Windows.Forms.Label lblDirName;
    }
}

