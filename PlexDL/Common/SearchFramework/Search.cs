﻿using PlexDL.Common.Globals.Providers;
using PlexDL.Common.Logging;
using PlexDL.Common.Renderers.Forms.GridView;
using PlexDL.UI.Forms;
using PlexDL.WaitWindow;
using System;
using System.Data;
using System.Windows.Forms;
using UIHelpers;

namespace PlexDL.Common.SearchFramework
{
    public static class Search
    {
        private static void RenderWithNoStruct(object sender, WaitWindowEventArgs e)
        {
            var dataToRender = (DataTable)e.Arguments[1];
            var gridToRender = (DataGridView)e.Arguments[0];
            if (gridToRender.InvokeRequired)
                gridToRender.BeginInvoke((MethodInvoker)delegate { gridToRender.DataSource = dataToRender; });
            else
                gridToRender.DataSource = dataToRender;
            e.Result = dataToRender;
        }

        private static void RenderWithStruct(object sender, WaitWindowEventArgs e)
        {
            var gridToRender = (DataGridView)e.Arguments[0];
            var renderInfo = (GenericRenderStruct)e.Arguments[1];
            e.Result = GenericViewRenderer.RenderView(renderInfo, gridToRender);
        }

        private static DataTable RenderResult(DataGridView dgvTarget, GenericRenderStruct info)
        {
            var message = "Preparing results";
            if (dgvTarget != null && info != null)
                if (info.Data != null)
                    return (DataTable)WaitWindow.WaitWindow.Show(RenderWithStruct, message, dgvTarget, info);
            return null;
        }

        private static DataTable RenderResult(DataGridView dgvTarget, DataTable data)
        {
            var message = "Preparing results";
            if (dgvTarget != null && data != null)
                return (DataTable)WaitWindow.WaitWindow.Show(RenderWithNoStruct, message, dgvTarget, data);
            return null;
        }

        public static bool RunTitleSearch(DataGridView dgvTarget, GenericRenderStruct dgvRenderInfo, SearchData searchContext,
            bool copyToGlobalTables = false)
        {
            try
            {
                //UIMessages.Info(data.SearchRule.ToString());

                var filteredTable = Workers.GetFilteredTable(searchContext, false);
                DataTable filteredView = null;
                //UIMessages.Info(filteredTable.Rows.Count.ToString());

                if (filteredTable == null)
                    return false;
                if (dgvRenderInfo == null)
                {
                    filteredView = RenderResult(dgvTarget, filteredTable);
                }
                else if (dgvRenderInfo.Data != null)
                {
                    dgvRenderInfo.Data = filteredTable;
                    filteredView = RenderResult(dgvTarget, dgvRenderInfo);
                }

                if (!copyToGlobalTables) return true;

                DataProvider.FilteredProvider.SetRawTable(filteredTable);
                DataProvider.FilteredProvider.SetViewTable(filteredView);

                return true;
            }
            catch (Exception ex)
            {
                LoggingHelpers.RecordException(ex.Message, "SearchError");
                UIMessages.Error(ex.ToString());
                return false;
            }
        }

        public static bool RunTitleSearch(DataGridView dgvTarget, SearchData searchContext,
            bool copyToGlobalTables = false)
        {
            var renderContext = new GenericRenderStruct
            {
                Data = searchContext.SearchTable
            };

            return RunTitleSearch(dgvTarget, renderContext, searchContext, copyToGlobalTables);
        }

        public static bool RunTitleSearch(DataGridView dgvTarget, GenericRenderStruct dgvRenderInfo,
            bool copyToGlobalTables = false)
        {
            try
            {
                if (dgvTarget.Rows.Count > 0)
                {
                    var start = new SearchOptions
                    {
                        SearchCollection = dgvRenderInfo.Data,
                        ColumnCollection = dgvRenderInfo.WantedColumns
                    };
                    var result = SearchForm.ShowSearch(start, dgvRenderInfo.WantedColumns);
                    if (!string.IsNullOrEmpty(result.SearchTerm) && !string.IsNullOrEmpty(result.SearchColumn))
                    {
                        var data = new SearchData
                        {
                            SearchColumn = result.SearchColumn,
                            SearchTerm = result.SearchTerm,
                            SearchRule = result.SearchRule,
                            SearchTable = dgvRenderInfo.Data
                        };

                        return RunTitleSearch(dgvTarget, dgvRenderInfo, data, copyToGlobalTables);
                    }

                    return false;
                }

                return false;
            }
            catch (Exception ex)
            {
                LoggingHelpers.RecordException(ex.Message, "SearchError");
                UIMessages.Error(ex.ToString());
                return false;
            }
        }

        public static bool RunTitleSearch(DataGridView dgvTarget, DataTable tableToSearch,
            bool copyToGlobalTables = false)
        {
            return RunTitleSearch(dgvTarget, new GenericRenderStruct { Data = tableToSearch }, copyToGlobalTables);
        }
    }
}