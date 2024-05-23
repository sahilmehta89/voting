using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voting.Core.Model;
using Voting.Persistence.PostgreSQL.Configurations;

namespace Voting.Persistence.PostgreSQL
{
    public class VotingDbContext : DbContext
    {
        public VotingDbContext(DbContextOptions<VotingDbContext> options)
            : base(options)
        {
        }

        public DbSet<Candidate> Candidate { get; set; }
        public DbSet<Voter> Voter { get; set; }
        public DbSet<VotingDetail> VotingDetail { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfiguration(new CandidateConfiguration())
                .ApplyConfiguration(new VoterConfiguration())
                .ApplyConfiguration(new VotingConfiguration());
            
            PopulateVoterSeedData(modelBuilder);
            PopulateCandidateSeedData(modelBuilder);
            PopulateVotingDetailSeedData(modelBuilder);
        }

        private void PopulateCandidateSeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Candidate>().HasData(new Candidate
            {
                Id = 1,
                Name = "Martin Johnston",
            });

            modelBuilder.Entity<Candidate>().HasData(new Candidate
            {
                Id = 2,
                Name = "Daniel Stevens",
            });
        }

        private void PopulateVoterSeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Voter>().HasData(new Voter
            {
                Id = 1,
                Name = "John Smith",
            });

            modelBuilder.Entity<Voter>().HasData(new Voter
            {
                Id = 2,
                Name = "Andrew Johnston",
            });
        }

        private void PopulateVotingDetailSeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VotingDetail>().HasData(new VotingDetail
            {
                CandidateId = 1,
                VoterId = 1
            });
        }
    }
}
