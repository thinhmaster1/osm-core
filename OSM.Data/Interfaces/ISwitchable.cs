using OSM.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OSM.Data.Interfaces
{
    public interface ISwitchable
    {
        Status Status { set; get; }
    }
}
