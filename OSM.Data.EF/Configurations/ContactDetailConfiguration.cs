using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OSM.Data.EF.Extensions;
using OSM.Data.Entities;

namespace OSM.Data.EF.Configurations
{
    public class ContactDetailConfiguration : DbEntityConfiguration<Contact>
    {
        public override void Configure(EntityTypeBuilder<Contact> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).HasMaxLength(255).IsRequired();
        }
    }
}