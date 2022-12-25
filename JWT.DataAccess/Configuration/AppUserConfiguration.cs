using JWT.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace JWT.DataAccess.Configuration
{
    public class AppUserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("AspNetUsers");
            builder.Property<string>(x => x.FirstName).IsRequired();
            builder.Property<string>(x => x.LastName).IsRequired();
            builder.Property<string>(x => x.CreatedBy);
            builder.Property<string>(x => x.Region).IsRequired();
            builder.Property<DateTime>(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.IsActive).IsRequired().HasDefaultValue(true);
        }
    }
}
