﻿namespace PlexDL.UI
{
    partial class ServerManager
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerManager));
            this.dgvServers = new PlexDL.Common.Components.FlatDataGridView();
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.itmAuthenticate = new System.Windows.Forms.ToolStripMenuItem();
            this.itmLoad = new System.Windows.Forms.ToolStripMenuItem();
            this.itmServers = new System.Windows.Forms.ToolStripMenuItem();
            this.itmRelays = new System.Windows.Forms.ToolStripMenuItem();
            this.itmDirectConnection = new System.Windows.Forms.ToolStripMenuItem();
            this.itmConnect = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dgvServers)).BeginInit();
            this.menuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvServers
            // 
            this.dgvServers.AllowUserToAddRows = false;
            this.dgvServers.AllowUserToDeleteRows = false;
            this.dgvServers.AllowUserToOrderColumns = true;
            this.dgvServers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvServers.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvServers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvServers.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Gray;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvServers.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvServers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvServers.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvServers.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            this.dgvServers.Location = new System.Drawing.Point(0, 24);
            this.dgvServers.Margin = new System.Windows.Forms.Padding(2);
            this.dgvServers.MultiSelect = false;
            this.dgvServers.Name = "dgvServers";
            this.dgvServers.RowHeadersVisible = false;
            this.dgvServers.RowsEmptyText = "No Servers Found";
            this.dgvServers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvServers.Size = new System.Drawing.Size(800, 426);
            this.dgvServers.TabIndex = 16;
            this.dgvServers.CurrentCellChanged += new System.EventHandler(this.dgvServers_SelectionChanged);
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itmAuthenticate,
            this.itmLoad,
            this.itmConnect});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(800, 24);
            this.menuMain.TabIndex = 17;
            this.menuMain.Text = "menuStrip1";
            // 
            // itmAuthenticate
            // 
            this.itmAuthenticate.Name = "itmAuthenticate";
            this.itmAuthenticate.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.itmAuthenticate.Size = new System.Drawing.Size(87, 20);
            this.itmAuthenticate.Text = "Authenticate";
            this.itmAuthenticate.Click += new System.EventHandler(this.itmAuthenticate_Click);
            // 
            // itmLoad
            // 
            this.itmLoad.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itmServers,
            this.itmRelays,
            this.itmDirectConnection});
            this.itmLoad.Enabled = false;
            this.itmLoad.Name = "itmLoad";
            this.itmLoad.Size = new System.Drawing.Size(45, 20);
            this.itmLoad.Text = "Load";
            // 
            // itmServers
            // 
            this.itmServers.Name = "itmServers";
            this.itmServers.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.itmServers.Size = new System.Drawing.Size(212, 22);
            this.itmServers.Text = "Servers";
            this.itmServers.Click += new System.EventHandler(this.itmServers_Click);
            // 
            // itmRelays
            // 
            this.itmRelays.Name = "itmRelays";
            this.itmRelays.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.itmRelays.Size = new System.Drawing.Size(212, 22);
            this.itmRelays.Text = "Relays";
            this.itmRelays.Click += new System.EventHandler(this.itmRelays_Click);
            // 
            // itmDirectConnection
            // 
            this.itmDirectConnection.Name = "itmDirectConnection";
            this.itmDirectConnection.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.itmDirectConnection.Size = new System.Drawing.Size(212, 22);
            this.itmDirectConnection.Text = "Direct Connection";
            // 
            // itmConnect
            // 
            this.itmConnect.Enabled = false;
            this.itmConnect.Name = "itmConnect";
            this.itmConnect.Size = new System.Drawing.Size(134, 20);
            this.itmConnect.Text = "Connect to this server";
            this.itmConnect.Click += new System.EventHandler(this.itmConnect_Click);
            // 
            // ServerManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dgvServers);
            this.Controls.Add(this.menuMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuMain;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ServerManager";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Server Manager";
            this.Load += new System.EventHandler(this.ServerManager_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvServers)).EndInit();
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Common.Components.FlatDataGridView dgvServers;
        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem itmAuthenticate;
        private System.Windows.Forms.ToolStripMenuItem itmLoad;
        private System.Windows.Forms.ToolStripMenuItem itmServers;
        private System.Windows.Forms.ToolStripMenuItem itmRelays;
        private System.Windows.Forms.ToolStripMenuItem itmDirectConnection;
        private System.Windows.Forms.ToolStripMenuItem itmConnect;
    }
}