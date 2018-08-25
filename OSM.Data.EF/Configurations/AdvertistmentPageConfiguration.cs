using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OSM.Data.EF.Extensions;
using OSM.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OSM.Data.EF.Configurations
{
    public class AdvertistmentPageConfiguration : DbEntityConfiguration<AdvertistmentPage>
    {
        public override void Configure(EntityTypeBuilder<AdvertistmentPage> entity)
        {
            entity.Property(c => c.Id).HasMaxLength(20).IsRequired();
        }
    }
}
