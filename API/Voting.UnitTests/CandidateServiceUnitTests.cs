using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voting.Core.Services;
using Voting.Persistence.PostgreSQL;
using Voting.Services;
using Voting.Services.Maps;

namespace Voting.UnitTests
{
    public class CandidateServiceUnitTests : IDisposable
    {
        protected readonly VotingDbContext _context;

        public CandidateServiceUnitTests()
        {
            var options = new DbContextOptionsBuilder<VotingDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            _context = new VotingDbContext(options);

            _context.Database.EnsureCreated();
        }

        [Fact]
        public async Task GetCandidates()
        {
            var logggerMock = new Mock<ILogger<CandidateService>>();

            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new CandidateProfile()));
            IMapper mapper = new Mapper(configuration);
            var candidateService = new CandidateService(logggerMock.Object, mapper, unitOfWork);

            /// Act
            var result = await candidateService.GetCandidates();

            /// Assert
            result.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task AddCandidates()
        {
            var logggerMock = new Mock<ILogger<CandidateService>>();

            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new CandidateProfile()));
            IMapper mapper = new Mapper(configuration);
            var candidateService = new CandidateService(logggerMock.Object, mapper, unitOfWork);

            /// Act
            _context.Candidate.AddRange(new Core.Model.Candidate()
            {
                Name = "Test"
            });
            await _context.SaveChangesAsync();

            var afterInsert = await candidateService.GetCandidates();

            /// Assert
            Assert.Contains(afterInsert, x => x.Name == "Test");
        }

        [Fact]
        public async Task CheckCandidateVoteCountAfterVote()
        {
            var logggerMock = new Mock<ILogger<CandidateService>>();

            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new CandidateProfile()));
            IMapper mapper = new Mapper(configuration);
            var candidateService = new CandidateService(logggerMock.Object, mapper, unitOfWork);

            /// Act
            _context.VotingDetail.AddRange(new Core.Model.VotingDetail()
            {
                CandidateId = 2,
                VoterId = 2
            });
            await _context.SaveChangesAsync();

            var afterInsert = await candidateService.GetCandidates();

            /// Assert
            Assert.True(afterInsert.All(x=> x.Votes > 0));
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
