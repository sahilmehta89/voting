using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voting.Core.Model
{
    public class VotingDetail
    {
        public int CandidateId { get; set; }
        public int VoterId { get; set; }
        public DateTime CreatedOn { get; set; }
        public virtual Candidate Candidate { get; set; }
        public virtual Voter Voter { get; set; }
    }
}
