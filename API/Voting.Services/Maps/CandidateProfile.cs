using AutoMapper;
using Voting.Core.Model;
using Voting.Core.Model.Dto;

namespace Voting.Services.Maps
{
    public class CandidateProfile : Profile
    {
        public CandidateProfile()
        {
            CreateMap<Candidate, CandidateViewModel>().ForMember(x => x.Votes, o => o.MapFrom(opt => opt.VotesReceived.Count));
            CreateMap<CandidateUpdateModel, Candidate>();
            CreateMap<CandidateViewModel, CandidateUpdateModel>();
            CreateMap<CandidateCreateModel, Candidate>();
        }
    }
}
