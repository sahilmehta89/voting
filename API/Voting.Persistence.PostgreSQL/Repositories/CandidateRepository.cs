using Microsoft.EntityFrameworkCore;
using Voting.Core.Model;
using Voting.Core.Repositories;

namespace Voting.Persistence.PostgreSQL.Repositories
{
    public class CandidateRepository : Repository<Candidate>, ICandidateRepository
    {
        public CandidateRepository(VotingDbContext context)
            : base(context)
        {

        }

        private VotingDbContext DbContext => Context as VotingDbContext;

        public async Task AddCandidate(Candidate candidate)
        {
            await AddAsync(candidate).ConfigureAwait(false);
        }

        public async Task UpdateCandidate(Candidate candidate)
        {
            await UpdateAsync(candidate).ConfigureAwait(false);
        }

        public async Task<Candidate?> GetCandidateById(int id)
        {
            return await DbContext.Candidate.SingleOrDefaultAsync(m => m.Id == id && m.IsDeleted == false).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Candidate>> GetCandidates()
        {
            return await DbContext.Candidate.Where(x => x.IsDeleted == false).Include(x=> x.VotesReceived).AsNoTracking().ToListAsync().ConfigureAwait(false);
        }

        public async Task<bool> IsCandidateExists(string title)
        {
            return await DbContext.Candidate.AnyAsync(m => m.IsDeleted == false && m.Name.ToLower() == title.ToLower()).ConfigureAwait(false);
        }
    }
}
