using Voting.Core;
using Voting.Core.Repositories;
using Voting.Persistence.PostgreSQL.Repositories;

namespace Voting.Persistence.PostgreSQL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly VotingDbContext _context;
        private CandidateRepository _candidateRepository;
        private VoterRepository _voterRepository;
        private VotingDetailRepository _voterDetailRepository;

        public UnitOfWork(VotingDbContext context)
        {
            _context = context;
        }

        public ICandidateRepository Candidate => _candidateRepository = _candidateRepository ?? new CandidateRepository(_context);
        public IVoterRepository Voter => _voterRepository = _voterRepository ?? new VoterRepository(_context);
        public IVotingDetailRepository VotingDetail => _voterDetailRepository = _voterDetailRepository ?? new VotingDetailRepository(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
