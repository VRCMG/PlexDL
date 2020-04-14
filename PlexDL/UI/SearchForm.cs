﻿using PlexDL.Common.SearchFramework;
using PlexDL.Common.SearchFramework.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace PlexDL.UI
{
    public partial class SearchForm : Form
    {
        public SearchOptions SearchContext = new SearchOptions();

        public SearchForm()
        {
            InitializeComponent();
            cbxSearchRule.SelectedIndex = 0;
        }

        private void BtnStartSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtSearchTerm.Text) && cbxSearchColumn.SelectedItem != null && cbxSearchRule.SelectedIndex >= 0)
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show(@"Please enter required values or exit search", @"Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void PopulateAllColumns()
        {
            foreach (DataColumn column in SearchContext.SearchCollection.Columns)
                cbxSearchColumn.Items.Add(column.ColumnName);
        }

        private void PopulateFilteredColumns()
        {
            foreach (DataColumn column in SearchContext.SearchCollection.Columns)
                if (SearchContext.ColumnCollection.Contains(column.ColumnName))
                    cbxSearchColumn.Items.Add(column.ColumnName);
        }

        private void PopulateColumns()
        {
            cbxSearchColumn.Items.Clear();

            if (SearchContext.ColumnCollection != null)
            {
                if (SearchContext.ColumnCollection.Count > 0)
                {
                    PopulateFilteredColumns();
                }
                else
                {
                    PopulateAllColumns();
                }
            }
            else
            {
                PopulateAllColumns();
            }
            if (cbxSearchColumn.Items.Count > 0)
            {
                //"title" should always get first preference.
                //this checks to see if it's in the ComboBox,
                //then selects it if it is. If it isn't, we should always
                //select index 0 so that the box selection isn't immediately empty.
                if (cbxSearchColumn.Items.Contains("title"))
                    cbxSearchColumn.SelectedItem = "title";
                else
                    cbxSearchColumn.SelectedIndex = 0;
            }
        }

        private void FrmSearch_Load(object sender, EventArgs e)
        {
            PopulateColumns();
        }

        public static SearchResult ShowSearch(SearchOptions settings, List<string> wantedColumns)
        {
            var frm = new SearchForm
            {
                SearchContext = settings
            };
            var result = new SearchResult();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                result.SearchColumn = frm.cbxSearchColumn.SelectedItem.ToString();
                result.SearchTerm = frm.txtSearchTerm.Text;

                switch (frm.cbxSearchRule.SelectedIndex)
                {
                    case 0:
                        result.SearchRule = SearchRule.CONTAINS_KEY;
                        break;

                    case 1:
                        result.SearchRule = SearchRule.EQUALS_KEY;
                        break;

                    case 2:
                        result.SearchRule = SearchRule.BEGINS_WITH;
                        break;

                    case 3:
                        result.SearchRule = SearchRule.ENDS_WITH;
                        break;
                }
                return result;
            }

            return result;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}