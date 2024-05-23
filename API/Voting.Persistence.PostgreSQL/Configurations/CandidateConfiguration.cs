using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Voting.Core.Model;

namespace Voting.Persistence.PostgreSQL.Configurations
{
    public class CandidateConfiguration : IEntityTypeConfiguration<Candidate>
    {
        public CandidateConfiguration() { }

        public void Configure(EntityTypeBuilder<Candidate> builder)
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
                .ToTable("Candidate");
        }
    }
}
