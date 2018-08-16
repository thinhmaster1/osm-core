using System;
using System.Collections.Generic;
using System.Text;

namespace OSM.Data.Interfaces
{
    public interface ISortable
    {
        int SortOrder { set; get; }
    }
}
