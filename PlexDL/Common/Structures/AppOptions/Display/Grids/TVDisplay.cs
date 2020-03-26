﻿using System.Collections.Generic;

namespace PlexDL.Common.Structures.AppOptions.Display.Grids
{
    public class TvDisplay : ColumnAdapter
    {
        public TvDisplay()
        {
            DisplayColumns = new List<string>
            {
                "title", "studio", "year", "contentRating"
            };

            DisplayCaptions = new List<string>
            {
                "Title", "Studio", "Year", "Rating"
            };
        }
    }
}