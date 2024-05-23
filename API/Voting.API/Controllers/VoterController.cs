using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Voting.Core.Model.Dto;
using Voting.Core.Services;
using Voting.Services;

namespace Voting.API.Controllers
{
    [ApiController]
    public class VoterController : BaseController
    {
        private ILogger<VoterController> _logger;
        private readonly IVoterService _voterService;

        public VoterController(ILogger<VoterController> logger, IVoterService voterService)
        {
            _logger = logger;
            _voterService = voterService;
        }

        [HttpGet]
        [Route("api/Voter/")]
        [ProducesResponseType(typeof(IEnumerable<VoterViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetVoters()
        {
            try
            {
                IEnumerable<VoterViewModel> voters = await _voterService.GetVoters().ConfigureAwait(false);
                return Ok(voters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while calling GetVoters in VoterController");
                return GetStatusCodeWithProblemDetails(StatusCodes.Status500InternalServerError, "Some error occurred");
            }
        }

        [HttpPost]
        [Route("api/Voter/")]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateVoter(VoterCreateModel voterCreateModel)
        {
            try
            {
                var response = await _voterService.AddVoter(voterCreateModel).ConfigureAwait(false);

                if (response.ReturnCode == 0)
                    return StatusCode(StatusCodes.Status201Created, response.VoterViewModel.Id);

                return GetStatusCodeWithProblemDetails(response.HttpStatusCode,
                    response.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while calling CreateVoter in VoterController. Data={JsonConvert.SerializeObject(voterCreateModel)}");
                return GetStatusCodeWithProblemDetails(StatusCodes.Status500InternalServerError, "Some error occurred");
            }
        }

        [HttpPost]
        [Route("api/Voter/VoteCandidate")]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(int), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> VoteCandidate(VoteCandidateCreateModel voteCandateCreateModel)
        {
            try
            {
                var response = await _voterService.VoteCandidate(voteCandateCreateModel).ConfigureAwait(false);
                return GetStatusCodeWithProblemDetails(response.HttpStatusCode,
                    response.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while calling VoteCandidate in VoterController. Data={JsonConvert.SerializeObject(voteCandateCreateModel)}");
                return GetStatusCodeWithProblemDetails(StatusCodes.Status500InternalServerError, "Some error occurred");
            }
        }
    }
}
