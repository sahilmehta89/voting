using AutoMapper;

namespace Voting.Services.Maps
{
    public static class MappingInitializer
    {
        public static IMapper Intialize()
        {
            // Auto Mapper Configurations  
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new CandidateProfile());
                mc.AddProfile(new VoterProfile());
            });
            var mapper = mappingConfig.CreateMapper();
            return mapper;
        }
    }
}
