using Voting.Core.Model;

namespace Voting.Core.Repositories
{
    public interface IVotingDetailRepository
    {
        Task VoteCandidate(VotingDetail votingDetail);
        Task<bool> ValidateVote(VotingDetail votingDetail);
    }
}
