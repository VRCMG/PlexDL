﻿using PlexDL.Common;
using PlexDL.Common.API;
using PlexDL.Common.Caching;
using PlexDL.Common.Logging;
using PlexDL.Common.Renderers.DGVRenderers;
using PlexDL.Common.Structures;
using PlexDL.PlexAPI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PlexDL.UI
{
    public partial class ServerManager : Form
    {
        public Server SelectedServer { get; set; } = null;

        public ServerManager()
        {
            InitializeComponent();
        }

        private void itmAuthenticate_Click(object sender, EventArgs e)
        {
            try
            {
                using (var frm = new Authenticate())
                {
                    var existingInfo = new ConnectionInfo
                    {
                        PlexAccountToken = GlobalStaticVars.Settings.ConnectionInfo.PlexAccountToken
                    };
                    frm.ConnectionInfo = existingInfo;
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        GlobalStaticVars.User.authenticationToken = frm.ConnectionInfo.PlexAccountToken;
                        GlobalStaticVars.Settings.ConnectionInfo.PlexAccountToken = frm.ConnectionInfo.PlexAccountToken;
                        itmLoad.Enabled = true;
                        dgvServers.DataSource = null;
                        MessageBox.Show(@"Token applied successfully. You can now load servers and relays from Plex.tv", @"Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingHelpers.RecordException(ex.Message, "ConnectionError");
                MessageBox.Show("Connection Error:\n\n" + ex, "Connection Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void RenderServersView(List<Server> servers)
        {
            PlexDL.WaitWindow.WaitWindow.Show(this.WorkerRenderServersView, "Updating Servers", new object[] { servers });
        }

        private void WorkerRenderServersView(object sender, PlexDL.WaitWindow.WaitWindowEventArgs e)
        {
            List<Server> servers = (List<Server>)e.Arguments[0];
            ServerViewRenderer.RenderView(servers, dgvServers);
        }

        private void GetServerListWorker(object sender, PlexDL.WaitWindow.WaitWindowEventArgs e)
        {
            Helpers.CacheStructureBuilder();
            if (ServerCaching.ServerInCache(GlobalStaticVars.Settings.ConnectionInfo.PlexAccountToken) && GlobalStaticVars.Settings.CacheSettings.Mode.EnableServerCaching)
            {
                e.Result = ServerCaching.ServerFromCache(GlobalStaticVars.Settings.ConnectionInfo.PlexAccountToken);
            }
            else
            {
                List<Server> result = GlobalStaticVars.Plex.GetServers(GlobalStaticVars.User);
                if (GlobalStaticVars.Settings.CacheSettings.Mode.EnableServerCaching)
                    ServerCaching.ServerToCache(result, GlobalStaticVars.User.authenticationToken);
                e.Result = result;
            }
        }

        private void GetRelaysListWorker(object sender, PlexDL.WaitWindow.WaitWindowEventArgs e)
        {
            List<Server> result = Relays.GetServerRelays(GlobalStaticVars.User.authenticationToken);
            e.Result = result;
        }

        private void itmServers_Click(object sender, EventArgs e)
        {
            try
            {
                if (GlobalStaticVars.User.authenticationToken != "")
                {
                    object serversResult = PlexDL.WaitWindow.WaitWindow.Show(GetServerListWorker, "Fetching Servers");
                    List<Server> servers = (List<Server>)serversResult;
                    if (servers.Count == 0)
                    {
                        DialogResult msg = MessageBox.Show("No servers found for current account token. Please update your token or try a direct connection.", "Authentication Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        GlobalStaticVars.PlexServers = servers;
                        RenderServersView(servers);
                        itmConnect.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Server retrieval error\n\n" + ex.ToString(), "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoggingHelpers.RecordException(ex.Message, "ServerGetError");
                return;
            }
        }

        private void itmRelays_Click(object sender, EventArgs e)
        {
            try
            {
                if (GlobalStaticVars.User.authenticationToken != "")
                {
                    object serversResult = PlexDL.WaitWindow.WaitWindow.Show(GetRelaysListWorker, "Fetching Relays");
                    List<Server> servers = (List<Server>)serversResult;
                    if (servers.Count == 0)
                    {
                        DialogResult msg = MessageBox.Show("No relays found for current account token. Please update your token or try a direct connection.", "Authentication Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        GlobalStaticVars.PlexServers = servers;
                        RenderServersView(servers);
                        itmConnect.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Relay retrieval error\n\n" + ex.ToString(), "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoggingHelpers.RecordException(ex.Message, "RelayGetError");
                return;
            }
        }

        private void itmConnect_Click(object sender, EventArgs e)
        {
            if (dgvServers.SelectedRows.Count == 1)
            {
                try
                {
                    int index = dgvServers.CurrentCell.RowIndex;
                    Server s = GlobalStaticVars.PlexServers[index];
                    SelectedServer = s;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    LoggingHelpers.RecordException(ex.Message, "ConnectionError");
                    MessageBox.Show("Server connection attempt failed\n\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void RunDirectConnect()
        {
            var servers = new List<Server>();
            using (var frmDir = new DirectConnect())
            {
                frmDir.ConnectionInfo.PlexAccountToken = GlobalStaticVars.User.authenticationToken;
                if (frmDir.ShowDialog() == DialogResult.OK)
                {
                    GlobalStaticVars.Settings.ConnectionInfo = frmDir.ConnectionInfo;
                    GlobalStaticVars.User.authenticationToken = frmDir.ConnectionInfo.PlexAccountToken;
                    var s = new Server
                    {
                        accessToken = GlobalStaticVars.User.authenticationToken,
                        address = GlobalStaticVars.Settings.ConnectionInfo.PlexAddress,
                        port = GlobalStaticVars.Settings.ConnectionInfo.PlexPort,
                        name = "DirectConnect"
                    };
                    servers.Add(s);
                    GlobalStaticVars.PlexServers = servers;
                    SelectedServer = s;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }

        private void dgvServers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvServers.SelectedRows.Count == 1)
                itmConnect.Enabled = true;
            else
                itmConnect.Enabled = false;
        }

        private void ServerManager_Load(object sender, EventArgs e)
        {
            if (GlobalStaticVars.PlexServers != null)
            {
                if (GlobalStaticVars.PlexServers.Count > 0)
                {
                    RenderServersView(GlobalStaticVars.PlexServers);
                }
            }
        }
    }
}