using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voting.Core.Model;
using Voting.Core.Model.Dto;

namespace Voting.UnitTests.MockData
{
    public class VotingMockData
    {
        public static IEnumerable<VoterViewModel> GetVotersViewModel()
        {
            return new List<VoterViewModel>
            {
                new VoterViewModel { Id = 1, Name = "A"},
                new VoterViewModel { Id = 2, Name = "B"},
                new VoterViewModel { Id = 3, Name = "C", HasVoted = true }
            };
        }

        public static IEnumerable<Voter> GetVoters()
        {
            return new List<Voter>
            {
                new Voter { Id = 1, Name = "A"},
                new Voter { Id = 2, Name = "B"},
                new Voter { Id = 3, Name = "C" }
            };
        }
    }
}
