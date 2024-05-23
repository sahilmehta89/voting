using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Voting.Core.Model.Dto;
using Voting.Core.Services;
using Voting.Services;

namespace Voting.API.Controllers
{
    [ApiController]
    public class CandidateController : BaseController
    {
        private ILogger<CandidateController> _logger;
        private readonly ICandidateService _candidateService;

        public CandidateController(ILogger<CandidateController> logger, ICandidateService candidateService)
        {
            _logger = logger;
            _candidateService = candidateService;
        }

        [HttpGet]
        [Route("api/Candidate/")]
        [ProducesResponseType(typeof(IEnumerable<CandidateViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCandidates()
        {
            try
            {
                IEnumerable<CandidateViewModel> candidates = await _candidateService.GetCandidates().ConfigureAwait(false);
                return Ok(candidates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while calling GetCandidates in CandidateController");
                return GetStatusCodeWithProblemDetails(StatusCodes.Status500InternalServerError, "Some error occurred");
            }
        }

        [HttpPost]
        [Route("api/Candidate/")]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCandidate(CandidateCreateModel candidateCreateModel)
        {
            try
            {
                var response = await _candidateService.AddCandidate(candidateCreateModel).ConfigureAwait(false);

                if (response.ReturnCode == 0)
                    return StatusCode(StatusCodes.Status201Created, response.CandidateViewModel.Id);

                return GetStatusCodeWithProblemDetails(response.HttpStatusCode,
                    response.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while calling CreateCandidate in CandidateController. Data={JsonConvert.SerializeObject(candidateCreateModel)}");
                return GetStatusCodeWithProblemDetails(StatusCodes.Status500InternalServerError, "Some error occurred");
            }
        }
    }
}
