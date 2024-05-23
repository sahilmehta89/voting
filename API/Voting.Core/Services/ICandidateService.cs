using Voting.Core.Model;
using Voting.Core.Model.Dto;

namespace Voting.Core.Services
{
    public interface ICandidateService
    {
        /// <summary>
        /// Add candidate
        /// </summary>
        /// <param name="candidateCreateModel">CandidateCreateModel</param>
        /// <returns>CandidateServiceResponse</returns>
        Task<CandidateServiceResponse> AddCandidate(CandidateCreateModel candidateCreateModel);

        /// <summary>
        /// Update existing candidate
        /// </summary>
        /// <param name="candidateUpdateModel">CandidateUpdateModel</param>
        /// <returns>CandidateServiceResponse</returns>
        Task<CandidateServiceResponse> UpdateCandidate(CandidateUpdateModel candidateUpdateModel);

        /// <summary>
        /// Get all candidates
        /// </summary>
        /// <returns>List of candidates</returns>
        Task<IEnumerable<CandidateViewModel>> GetCandidates();

        /// <summary>
        /// Get candidate by Id
        /// </summary>
        /// <param name="id">Candidate Id</param>
        /// <returns>Candidate</returns>
        Task<CandidateViewModel> GetCandidate(int id);
    }
}
