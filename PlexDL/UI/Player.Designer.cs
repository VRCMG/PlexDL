﻿using System.ComponentModel;
using System.Windows.Forms;

namespace PlexDL.UI
{
    partial class Player
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Player));
            this.tmrCopied = new System.Windows.Forms.Timer(this.components);
            this.tmrRefreshUI = new System.Windows.Forms.Timer(this.components);
            this.btnSkipBack = new System.Windows.Forms.Button();
            this.btnPrevTitle = new System.Windows.Forms.Button();
            this.btnSkipForward = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnNextTitle = new System.Windows.Forms.Button();
            this.lblTimeSoFar = new System.Windows.Forms.Label();
            this.trkDuration = new System.Windows.Forms.TrackBar();
            this.lblTotalDuration = new System.Windows.Forms.Label();
            this.btnPlayPause = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.tlpPlayerControls = new System.Windows.Forms.TableLayoutPanel();
            this.btnFullScreen = new System.Windows.Forms.Button();
            this.tlpMaster = new System.Windows.Forms.TableLayoutPanel();
            this.pnlPlayer = new System.Windows.Forms.Panel();
            this.wmpMain = new AxWMPLib.AxWindowsMediaPlayer();
            ((System.ComponentModel.ISupportInitialize)(this.trkDuration)).BeginInit();
            this.tlpPlayerControls.SuspendLayout();
            this.tlpMaster.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.wmpMain)).BeginInit();
            this.SuspendLayout();
            // 
            // tmrCopied
            // 
            this.tmrCopied.Interval = 1500;
            this.tmrCopied.Tick += new System.EventHandler(this.TmrCopied_Tick);
            // 
            // btnSkipBack
            // 
            this.btnSkipBack.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSkipBack.AutoSize = true;
            this.btnSkipBack.BackgroundImage = global::PlexDL.Properties.Resources.baseline_fast_rewind_black_18dp;
            this.btnSkipBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSkipBack.Location = new System.Drawing.Point(117, 6);
            this.btnSkipBack.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.btnSkipBack.Name = "btnSkipBack";
            this.btnSkipBack.Size = new System.Drawing.Size(32, 32);
            this.btnSkipBack.TabIndex = 13;
            this.btnSkipBack.UseVisualStyleBackColor = true;
            this.btnSkipBack.Click += new System.EventHandler(this.BtnSkipBack_Click);
            // 
            // btnPrevTitle
            // 
            this.btnPrevTitle.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPrevTitle.AutoSize = true;
            this.btnPrevTitle.BackgroundImage = global::PlexDL.Properties.Resources.baseline_skip_previous_black_18dp;
            this.btnPrevTitle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnPrevTitle.Location = new System.Drawing.Point(79, 6);
            this.btnPrevTitle.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.btnPrevTitle.Name = "btnPrevTitle";
            this.btnPrevTitle.Size = new System.Drawing.Size(32, 32);
            this.btnPrevTitle.TabIndex = 12;
            this.btnPrevTitle.UseVisualStyleBackColor = true;
            this.btnPrevTitle.Click += new System.EventHandler(this.BtnPrevTitle_Click);
            // 
            // btnSkipForward
            // 
            this.btnSkipForward.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSkipForward.AutoSize = true;
            this.btnSkipForward.BackgroundImage = global::PlexDL.Properties.Resources.baseline_fast_forward_black_18dp;
            this.btnSkipForward.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSkipForward.Location = new System.Drawing.Point(155, 6);
            this.btnSkipForward.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.btnSkipForward.Name = "btnSkipForward";
            this.btnSkipForward.Size = new System.Drawing.Size(32, 32);
            this.btnSkipForward.TabIndex = 14;
            this.btnSkipForward.UseVisualStyleBackColor = true;
            this.btnSkipForward.Click += new System.EventHandler(this.BtnSkipForward_Click);
            // 
            // btnStop
            // 
            this.btnStop.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnStop.AutoSize = true;
            this.btnStop.BackgroundImage = global::PlexDL.Properties.Resources.baseline_stop_black_18dp;
            this.btnStop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnStop.Location = new System.Drawing.Point(41, 6);
            this.btnStop.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(32, 32);
            this.btnStop.TabIndex = 11;
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // btnNextTitle
            // 
            this.btnNextTitle.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnNextTitle.AutoSize = true;
            this.btnNextTitle.BackgroundImage = global::PlexDL.Properties.Resources.baseline_skip_next_black_18dp;
            this.btnNextTitle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnNextTitle.Location = new System.Drawing.Point(193, 6);
            this.btnNextTitle.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.btnNextTitle.Name = "btnNextTitle";
            this.btnNextTitle.Size = new System.Drawing.Size(32, 32);
            this.btnNextTitle.TabIndex = 15;
            this.btnNextTitle.UseVisualStyleBackColor = true;
            this.btnNextTitle.Click += new System.EventHandler(this.BtnNextTitle_Click);
            // 
            // lblTimeSoFar
            // 
            this.lblTimeSoFar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblTimeSoFar.AutoSize = true;
            this.lblTimeSoFar.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.lblTimeSoFar.ForeColor = System.Drawing.Color.Black;
            this.lblTimeSoFar.Location = new System.Drawing.Point(231, 13);
            this.lblTimeSoFar.Name = "lblTimeSoFar";
            this.lblTimeSoFar.Size = new System.Drawing.Size(64, 18);
            this.lblTimeSoFar.TabIndex = 6;
            this.lblTimeSoFar.Text = "00:00:00";
            // 
            // trkDuration
            // 
            this.trkDuration.BackColor = this.BackColor;
            this.trkDuration.Cursor = System.Windows.Forms.Cursors.Hand;
            this.trkDuration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trkDuration.Location = new System.Drawing.Point(298, 0);
            this.trkDuration.Margin = new System.Windows.Forms.Padding(0);
            this.trkDuration.Maximum = 100;
            this.trkDuration.Name = "trkDuration";
            this.trkDuration.Size = new System.Drawing.Size(836, 45);
            this.trkDuration.TabIndex = 7;
            this.trkDuration.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            // 
            // lblTotalDuration
            // 
            this.lblTotalDuration.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblTotalDuration.AutoSize = true;
            this.lblTotalDuration.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.lblTotalDuration.ForeColor = System.Drawing.Color.Black;
            this.lblTotalDuration.Location = new System.Drawing.Point(1137, 13);
            this.lblTotalDuration.Name = "lblTotalDuration";
            this.lblTotalDuration.Size = new System.Drawing.Size(64, 18);
            this.lblTotalDuration.TabIndex = 8;
            this.lblTotalDuration.Text = "00:00:00";
            // 
            // btnPlayPause
            // 
            this.btnPlayPause.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPlayPause.AutoSize = true;
            this.btnPlayPause.BackgroundImage = global::PlexDL.Properties.Resources.baseline_play_arrow_black_18dp;
            this.btnPlayPause.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnPlayPause.Location = new System.Drawing.Point(3, 6);
            this.btnPlayPause.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.btnPlayPause.Name = "btnPlayPause";
            this.btnPlayPause.Size = new System.Drawing.Size(32, 32);
            this.btnPlayPause.TabIndex = 10;
            this.btnPlayPause.UseVisualStyleBackColor = true;
            this.btnPlayPause.Click += new System.EventHandler(this.BtnPlayPause_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnExit.AutoSize = true;
            this.btnExit.BackgroundImage = global::PlexDL.Properties.Resources.baseline_cancel_black_18dp;
            this.btnExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnExit.Location = new System.Drawing.Point(1245, 6);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(32, 32);
            this.btnExit.TabIndex = 11;
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // tlpPlayerControls
            // 
            this.tlpPlayerControls.AutoSize = true;
            this.tlpPlayerControls.ColumnCount = 11;
            this.tlpPlayerControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpPlayerControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpPlayerControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpPlayerControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpPlayerControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpPlayerControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpPlayerControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpPlayerControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPlayerControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpPlayerControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpPlayerControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpPlayerControls.Controls.Add(this.btnFullScreen, 9, 0);
            this.tlpPlayerControls.Controls.Add(this.btnExit, 10, 0);
            this.tlpPlayerControls.Controls.Add(this.btnPlayPause, 0, 0);
            this.tlpPlayerControls.Controls.Add(this.lblTotalDuration, 8, 0);
            this.tlpPlayerControls.Controls.Add(this.trkDuration, 7, 0);
            this.tlpPlayerControls.Controls.Add(this.lblTimeSoFar, 6, 0);
            this.tlpPlayerControls.Controls.Add(this.btnNextTitle, 5, 0);
            this.tlpPlayerControls.Controls.Add(this.btnStop, 1, 0);
            this.tlpPlayerControls.Controls.Add(this.btnSkipForward, 4, 0);
            this.tlpPlayerControls.Controls.Add(this.btnPrevTitle, 2, 0);
            this.tlpPlayerControls.Controls.Add(this.btnSkipBack, 3, 0);
            this.tlpPlayerControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPlayerControls.Location = new System.Drawing.Point(0, 726);
            this.tlpPlayerControls.Margin = new System.Windows.Forms.Padding(0);
            this.tlpPlayerControls.Name = "tlpPlayerControls";
            this.tlpPlayerControls.RowCount = 1;
            this.tlpPlayerControls.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpPlayerControls.Size = new System.Drawing.Size(1280, 45);
            this.tlpPlayerControls.TabIndex = 11;
            // 
            // btnFullScreen
            // 
            this.btnFullScreen.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnFullScreen.AutoSize = true;
            this.btnFullScreen.BackgroundImage = global::PlexDL.Properties.Resources.baseline_fullscreen_black_18dp;
            this.btnFullScreen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnFullScreen.Location = new System.Drawing.Point(1207, 6);
            this.btnFullScreen.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.btnFullScreen.Name = "btnFullScreen";
            this.btnFullScreen.Size = new System.Drawing.Size(32, 32);
            this.btnFullScreen.TabIndex = 12;
            this.btnFullScreen.UseVisualStyleBackColor = true;
            this.btnFullScreen.Click += new System.EventHandler(this.BtnFullScreen_Click);
            // 
            // tlpMaster
            // 
            this.tlpMaster.ColumnCount = 1;
            this.tlpMaster.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMaster.Controls.Add(this.pnlPlayer, 0, 0);
            this.tlpMaster.Controls.Add(this.tlpPlayerControls, 0, 1);
            this.tlpMaster.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMaster.Location = new System.Drawing.Point(0, 0);
            this.tlpMaster.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMaster.Name = "tlpMaster";
            this.tlpMaster.RowCount = 2;
            this.tlpMaster.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMaster.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMaster.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMaster.Size = new System.Drawing.Size(1280, 771);
            this.tlpMaster.TabIndex = 12;
            // 
            // pnlPlayer
            // 
            this.pnlPlayer.BackColor = System.Drawing.Color.Black;
            this.pnlPlayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPlayer.Location = new System.Drawing.Point(0, 0);
            this.pnlPlayer.Margin = new System.Windows.Forms.Padding(0);
            this.pnlPlayer.Name = "pnlPlayer";
            this.pnlPlayer.Size = new System.Drawing.Size(1280, 726);
            this.pnlPlayer.TabIndex = 12;
            // 
            // wmpMain
            // 
            this.wmpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wmpMain.Enabled = true;
            this.wmpMain.Location = new System.Drawing.Point(0, 0);
            this.wmpMain.Name = "wmpMain";
            this.wmpMain.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("wmpMain.OcxState")));
            this.wmpMain.Size = new System.Drawing.Size(1280, 771);
            this.wmpMain.TabIndex = 0;
            // 
            // Player
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1280, 771);
            this.Controls.Add(this.tlpMaster);
            this.Controls.Add(this.wmpMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Player";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Unknown Title";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmPlayer_FormClosing);
            this.Load += new System.EventHandler(this.FrmPlayer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trkDuration)).EndInit();
            this.tlpPlayerControls.ResumeLayout(false);
            this.tlpPlayerControls.PerformLayout();
            this.tlpMaster.ResumeLayout(false);
            this.tlpMaster.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.wmpMain)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Timer tmrCopied;
        private Timer tmrRefreshUI;
        private Button btnSkipBack;
        private Button btnPrevTitle;
        private Button btnSkipForward;
        private Button btnStop;
        private Button btnNextTitle;
        private Label lblTimeSoFar;
        private TrackBar trkDuration;
        private TableLayoutPanel tlpPlayerControls;
        private Button btnExit;
        private Button btnPlayPause;
        private Label lblTotalDuration;
        private TableLayoutPanel tlpMaster;
        private Button btnFullScreen;
        private AxWMPLib.AxWindowsMediaPlayer wmpMain;
        private Panel pnlPlayer;
    }
}