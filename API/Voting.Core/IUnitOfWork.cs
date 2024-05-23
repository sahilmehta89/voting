using Voting.Core.Repositories;

namespace Voting.Core
{
    public interface IUnitOfWork : IDisposable
    {
        ICandidateRepository Candidate { get; }
        IVoterRepository Voter { get; }
        IVotingDetailRepository VotingDetail { get; }
    }
}
