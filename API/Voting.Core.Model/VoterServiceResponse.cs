using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voting.Core.Model.Dto;

namespace Voting.Core.Model
{
    public class VoterServiceResponse : BaseServiceResponse
    {
        public VoterViewModel VoterViewModel { get; set; }

        public VoterServiceResponse()
        {
        }

        public VoterServiceResponse(int httpStatusCode, int returnCode, string message, VoterViewModel voterViewModel) : base(
            httpStatusCode, returnCode, message)
        {
            VoterViewModel = voterViewModel;
        }

        public VoterServiceResponse(int httpStatusCode, int returnCode, string message) : base(httpStatusCode,
            returnCode, message)
        {
        }

        public VoterServiceResponse(int httpStatusCode, string message) : base(httpStatusCode, message)
        {
            ReturnCode = httpStatusCode;
        }

        public VoterServiceResponse(VoterViewModel voterViewModel)
        {
            VoterViewModel = voterViewModel;
        }
    }
}
