using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voting.Core.Model;

namespace Voting.Persistence.PostgreSQL.Configurations
{
    public class VoterConfiguration : IEntityTypeConfiguration<Voter>
    {
        public VoterConfiguration() { }

        public void Configure(EntityTypeBuilder<Voter> builder)
        {
            builder
                .HasKey(m => m.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder
                .Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .Property(m => m.CreatedOn)
                .HasDefaultValueSql("timezone('utc', now())");

            builder
                .Property(m => m.IsDeleted)
                .HasDefaultValue(0);

            builder
                .ToTable("Voter");
        }
    }
}
