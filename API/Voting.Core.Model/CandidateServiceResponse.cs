using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voting.Core.Model.Dto;

namespace Voting.Core.Model
{
    public class CandidateServiceResponse : BaseServiceResponse
    {
        public CandidateViewModel CandidateViewModel { get; set; }

        public CandidateServiceResponse()
        {
        }

        public CandidateServiceResponse(int httpStatusCode, int returnCode, string message, CandidateViewModel candidateViewModel) : base(
            httpStatusCode, returnCode, message)
        {
            CandidateViewModel = candidateViewModel;
        }

        public CandidateServiceResponse(int httpStatusCode, int returnCode, string message) : base(httpStatusCode,
            returnCode, message)
        {
        }

        public CandidateServiceResponse(int httpStatusCode, string message) : base(httpStatusCode, message)
        {
            ReturnCode = httpStatusCode;
        }

        public CandidateServiceResponse(CandidateViewModel candidateViewModel)
        {
            CandidateViewModel = candidateViewModel;
        }
    }
}
