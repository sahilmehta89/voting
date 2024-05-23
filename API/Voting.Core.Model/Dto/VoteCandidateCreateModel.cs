using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voting.Core.Model.Dto
{
    public class VoteCandidateCreateModel
    {
        public int CandiateId { get; set; }
        public int VoterId { get; set;}
    }
}
