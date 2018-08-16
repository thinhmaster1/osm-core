using System;
using System.Collections.Generic;
using System.Text;

namespace OSM.Data.Interfaces
{
    public interface IDateTracking
    {
        DateTime DateCreated { get; set; }
        DateTime DateModified { set; get; }

    }
}
