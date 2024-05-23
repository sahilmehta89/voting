using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Voting.Core.Model;

namespace Voting.Persistence.PostgreSQL.Configurations
{
    public class VotingConfiguration : IEntityTypeConfiguration<VotingDetail>
    {
        public void Configure(EntityTypeBuilder<VotingDetail> builder)
        {
            builder
            .HasKey(v => new { v.VoterId, v.CandidateId });

            builder
            .Property(m => m.CreatedOn)
            .HasDefaultValueSql("timezone('utc', now())");

            builder
                .HasOne(v => v.Voter)
                .WithMany(v => v.VotesCast)
                .HasForeignKey(v => v.VoterId);

            builder
                .HasOne(v => v.Candidate)
                .WithMany(c => c.VotesReceived)
                .HasForeignKey(v => v.CandidateId);

            builder
                .ToTable("VotingDetails");
        }
    }
}
