using AutoMapper;
using Voting.Core.Model.Dto;
using Voting.Core.Model;

namespace Voting.Services.Maps
{
    public class VoterProfile : Profile
    {
        public VoterProfile() 
        {
            CreateMap<Voter, VoterViewModel>().ForMember(x=> x.HasVoted, o=> o.MapFrom(opt => opt.VotesCast.Any()));
            CreateMap<VoterUpdateModel, Voter>();
            CreateMap<VoterViewModel, VoterUpdateModel>();
            CreateMap<VoterCreateModel, Voter>();
        }
    }
}
