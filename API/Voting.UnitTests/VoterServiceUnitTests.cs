using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Voting.Persistence.PostgreSQL;
using Voting.Services;
using Voting.Services.Maps;
using Voting.UnitTests.MockData;

namespace Voting.UnitTests
{
    public class VoterServiceUnitTests : IDisposable
    {
        protected readonly VotingDbContext _context;

        public VoterServiceUnitTests()
        {
            var options = new DbContextOptionsBuilder<VotingDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            _context = new VotingDbContext(options);

            _context.Database.EnsureCreated();
        }

        [Fact]
        public async Task GetVoters()
        {           
            var logggerMock = new Mock<ILogger<VoterService>>();

            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new VoterProfile()));
            IMapper mapper = new Mapper(configuration);
            var voterService = new VoterService(logggerMock.Object, mapper, unitOfWork);

            /// Act
            var result = await voterService.GetVoters();

            /// Assert
            result.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task AddVoters()
        {
            var logggerMock = new Mock<ILogger<VoterService>>();

            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new VoterProfile()));
            IMapper mapper = new Mapper(configuration);
            var voterService = new VoterService(logggerMock.Object, mapper, unitOfWork);

            /// Act
            _context.Voter.AddRange(new Core.Model.Voter()
            {
                Name = "Test"
            });
            await _context.SaveChangesAsync();

            var afterInsert = await voterService.GetVoters();

            /// Assert
            Assert.Contains(afterInsert, x => x.Name == "Test");
        }

        [Fact]
        public async Task CheckVoterHasVotedAfterVote()
        {
            var logggerMock = new Mock<ILogger<VoterService>>();

            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new VoterProfile()));
            IMapper mapper = new Mapper(configuration);
            var voterService = new VoterService(logggerMock.Object, mapper, unitOfWork);

            /// Act
            _context.VotingDetail.AddRange(new Core.Model.VotingDetail()
            {
                CandidateId = 2,
                VoterId = 2
            });
            await _context.SaveChangesAsync();

            var afterInsert = await voterService.GetVoters();

            /// Assert
            Assert.Contains(afterInsert, x => x.Id == 2 && x.HasVoted);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}