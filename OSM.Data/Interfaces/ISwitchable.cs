using OSM.Data.Enums;

namespace OSM.Data.Interfaces
{
    public interface ISwitchable
    {
        Status Status { set; get; }
    }
}