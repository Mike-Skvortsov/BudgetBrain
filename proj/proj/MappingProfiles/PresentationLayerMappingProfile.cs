using AutoMapper;
using Entities.Entities;
using proj.Models;

namespace proj.MappingProfiles
{
    public class PresentationLayerMappingProfile : Profile
    {
        public PresentationLayerMappingProfile()
        {
            this.CreateMap<Card, CardModel>();
            this.CreateMap<User, UserModel>();
            this.CreateMap<Operation, OperationModel>();
            this.CreateMap<CardModel, Card>();
            this.CreateMap<UserModel, User>();
            this.CreateMap<OperationModel, Operation>();
        }
    }
}
