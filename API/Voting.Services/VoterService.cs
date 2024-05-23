using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Voting.Core;
using Voting.Core.Model;
using Voting.Core.Model.Dto;
using Voting.Core.Services;

namespace Voting.Services
{
    public class VoterService : IVoterService
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public VoterService(ILogger<VoterService> logger, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<VoterServiceResponse> AddVoter(VoterCreateModel voterCreateModel)
        {
            VoterServiceResponse voterServiceResponse;

            try
            {
                var voterExists = await IsVoterExists(voterCreateModel.Name).ConfigureAwait(false);

                if (!voterExists)
                {
                    var voter = _mapper.Map<Voter>(voterCreateModel);
                    await _unitOfWork.Voter.AddVoter(voter).ConfigureAwait(false);
                    var voterViewModel = _mapper.Map<VoterViewModel>(voter);
                    voterServiceResponse = new VoterServiceResponse(voterViewModel);
                }
                else
                {
                    voterServiceResponse = new VoterServiceResponse(StatusCodes.Status400BadRequest, "Voter with this name already exists");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddVoter");
                voterServiceResponse =
                    new VoterServiceResponse(StatusCodes.Status500InternalServerError, "Some error occurred");
            }

            return voterServiceResponse;
        }

        /// <inheritdoc/>
        public async Task<VoterViewModel> GetVoter(int id)
        {
            var existing = await _unitOfWork.Voter.GetVoterById(id).ConfigureAwait(false);
            return _mapper.Map<VoterViewModel>(existing);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<VoterViewModel>> GetVoters()
        {
            var voters = await _unitOfWork.Voter.GetVoters().ConfigureAwait(false);
            var voterViewModels = _mapper.Map<IEnumerable<VoterViewModel>>(voters);
            return voterViewModels;
        }

        /// <inheritdoc/>
        public async Task<VoterServiceResponse> UpdateVoter(VoterUpdateModel voterUpdateModel)
        {
            VoterServiceResponse voterServiceResponse;

            var existing = await _unitOfWork.Voter.GetVoterById(voterUpdateModel.Id).ConfigureAwait(false);

            if (existing != null)
            {
                existing.Name = voterUpdateModel.Name;
                await _unitOfWork.Voter.UpdateVoter(existing).ConfigureAwait(false);
                voterServiceResponse = new VoterServiceResponse(_mapper.Map<VoterViewModel>(existing));
            }
            else
            {
                voterServiceResponse = new VoterServiceResponse(StatusCodes.Status400BadRequest, "Record does not exists");
            }

            return voterServiceResponse;
        }

        /// <inheritdoc/>
        public async Task<VoterServiceResponse> VoteCandidate(VoteCandidateCreateModel voteCandateCreateModel)
        {
            var voter = await _unitOfWork.Voter.GetVoterById(voteCandateCreateModel.VoterId);
            if (voter == null)
            {
                return new VoterServiceResponse(StatusCodes.Status400BadRequest, "Invalid voter or already voted");
            }

            var candidate = await _unitOfWork.Candidate.GetCandidateById(voteCandateCreateModel.CandiateId);
            if (candidate == null)
            {
                return new VoterServiceResponse(StatusCodes.Status404NotFound, "Candidate not found");
            }

            VotingDetail votingDetail = new VotingDetail()
            {
                CandidateId = voteCandateCreateModel.CandiateId,
                VoterId = voteCandateCreateModel.VoterId
            };

            var existing = await _unitOfWork.VotingDetail.ValidateVote(votingDetail).ConfigureAwait(false);

            if (!existing)
            {
                await _unitOfWork.VotingDetail.VoteCandidate(votingDetail).ConfigureAwait(false);
                return new VoterServiceResponse(StatusCodes.Status201Created, "Record Added");
            }

            return new VoterServiceResponse(StatusCodes.Status400BadRequest, "Vote already added");
        }

        /// <summary>
        /// Check whether voter already exists by name
        /// </summary>
        /// <param name="name">Voter Name</param>
        /// <returns>True or False</returns>
        private async Task<bool> IsVoterExists(string name)
        {
            try
            {
                name = string.IsNullOrWhiteSpace(name) ? string.Empty : name.Trim();
                return await _unitOfWork.Voter.IsVoterExists(name).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting IsVoterExists");
                throw;
            }
        }
    }
}
