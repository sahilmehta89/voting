using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Voting.Core;
using Voting.Core.Model;
using Voting.Core.Model.Dto;
using Voting.Core.Services;

namespace Voting.Services
{
    public class CandidateService : ICandidateService
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CandidateService(ILogger<CandidateService> logger, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<CandidateServiceResponse> AddCandidate(CandidateCreateModel candidateCreateModel)
        {
            CandidateServiceResponse candidateServiceResponse;

            try
            {
                var candidateExists = await IsCandidateExists(candidateCreateModel.Name).ConfigureAwait(false);

                if (!candidateExists)
                {
                    var candidate = _mapper.Map<Candidate>(candidateCreateModel);
                    await _unitOfWork.Candidate.AddCandidate(candidate).ConfigureAwait(false);
                    var candidateViewModel = _mapper.Map<CandidateViewModel>(candidate);
                    candidateServiceResponse = new CandidateServiceResponse(candidateViewModel);
                }
                else
                {
                    candidateServiceResponse = new CandidateServiceResponse(StatusCodes.Status400BadRequest, "Candidate with this name already exists");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddCandidate");
                candidateServiceResponse =
                    new CandidateServiceResponse(StatusCodes.Status500InternalServerError, "Some error occurred");
            }

            return candidateServiceResponse;
        }

        /// <inheritdoc/>
        public async Task<CandidateViewModel> GetCandidate(int id)
        {
            var existing = await _unitOfWork.Candidate.GetCandidateById(id).ConfigureAwait(false);
            return _mapper.Map<CandidateViewModel>(existing);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CandidateViewModel>> GetCandidates()
        {
            var candidates = await _unitOfWork.Candidate.GetCandidates().ConfigureAwait(false);
            var candidateViewModels = _mapper.Map<IEnumerable<CandidateViewModel>>(candidates);
            return candidateViewModels;
        }

        /// <inheritdoc/>
        public async Task<CandidateServiceResponse> UpdateCandidate(CandidateUpdateModel candidateUpdateModel)
        {
            CandidateServiceResponse candidateServiceResponse;

            var existing = await _unitOfWork.Candidate.GetCandidateById(candidateUpdateModel.Id).ConfigureAwait(false);

            if (existing != null)
            {
                existing.Name = candidateUpdateModel.Name;
                await _unitOfWork.Candidate.UpdateCandidate(existing).ConfigureAwait(false);
                candidateServiceResponse = new CandidateServiceResponse(_mapper.Map<CandidateViewModel>(existing));
            }
            else
            {
                candidateServiceResponse = new CandidateServiceResponse(StatusCodes.Status400BadRequest, "Record does not exists");
            }

            return candidateServiceResponse;
        }

        /// <summary>
        /// Check whether Candidate exists by name
        /// </summary>
        /// <param name="name">Candidate Name</param>
        /// <returns>True or False</returns>
        private async Task<bool> IsCandidateExists(string name)
        {
            try
            {
                name = string.IsNullOrWhiteSpace(name) ? string.Empty : name.Trim();
                return await _unitOfWork.Candidate.IsCandidateExists(name).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting IsCandidateExists");
                throw;
            }
        }
    }
}
