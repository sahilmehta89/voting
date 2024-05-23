using Voting.Core.Model.Dto;
using Voting.Core.Model;

namespace Voting.Core.Services
{
    public interface IVoterService
    {
        /// <summary>
        /// Add New Voter
        /// </summary>
        /// <param name="voterCreateModel">Voter Request Model</param>
        /// <returns>VoterServiceResponse</returns>
        Task<VoterServiceResponse> AddVoter(VoterCreateModel voterCreateModel);

        /// <summary>
        /// Update existing voter
        /// </summary>
        /// <param name="voterUpdateModel">Voter Update Request Model</param>
        /// <returns>VoterServiceResponse</returns>
        Task<VoterServiceResponse> UpdateVoter(VoterUpdateModel voterUpdateModel);

        /// <summary>
        /// To vote for a candidate
        /// </summary>
        /// <param name="voteCandateCreateModel">VoteCandateCreateModel</param>
        /// <returns>VoterServiceResponse</returns>
        Task<VoterServiceResponse> VoteCandidate(VoteCandidateCreateModel voteCandateCreateModel);

        /// <summary>
        /// Get all voters
        /// </summary>
        /// <returns>List of Voters</returns>
        Task<IEnumerable<VoterViewModel>> GetVoters();

        /// <summary>
        /// Get Voter by Id
        /// </summary>
        /// <param name="id">VoterId</param>
        /// <returns>Voter record</returns>
        Task<VoterViewModel> GetVoter(int id);
    }
}
