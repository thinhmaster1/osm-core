using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OSM.Data.EF.Extensions;
using OSM.Data.Entities;

namespace OSM.Data.EF.Configurations
{
    public class AnnouncementConfiguration : DbEntityConfiguration<Announcement>
    {
        public override void Configure(EntityTypeBuilder<Announcement> entity)
        {
            entity.Property(c => c.Id).HasMaxLength(128).IsRequired();
        }
    }
}