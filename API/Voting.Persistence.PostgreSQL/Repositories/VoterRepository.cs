using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using Voting.Core.Model;
using Voting.Core.Repositories;

namespace Voting.Persistence.PostgreSQL.Repositories
{
    public class VoterRepository : Repository<Voter>, IVoterRepository
    {
        public VoterRepository(VotingDbContext context)
            : base(context)
        {

        }

        private VotingDbContext DbContext => Context as VotingDbContext;

        public async Task AddVoter(Voter voter)
        {
            await AddAsync(voter).ConfigureAwait(false);
        }

        public async Task UpdateVoter(Voter voter)
        {
            await UpdateAsync(voter).ConfigureAwait(false);
        }

        public async Task<Voter?> GetVoterById(int id)
        {
            return await DbContext.Voter.SingleOrDefaultAsync(m => m.Id == id && m.IsDeleted == false).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Voter>> GetVoters()
        {
            return await DbContext.Voter.Where(x => x.IsDeleted == false).Include(x=> x.VotesCast).AsNoTracking().ToListAsync().ConfigureAwait(false);
        }

        public async Task<bool> IsVoterExists(string title)
        {
            return await DbContext.Voter.AnyAsync(m => m.IsDeleted == false && m.Name.ToLower() == title.ToLower()).ConfigureAwait(false);
        }
    }
}
