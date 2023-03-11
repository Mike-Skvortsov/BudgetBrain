using AutoMapper;
using Entities.Entities;
using proj.Models;
using proj.Models.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace proj.MappingProfiles
{
	public class PresentationLayerMappingProfile : Profile
	{
		private static readonly MapperConfiguration MapperConfiguration = new MapperConfiguration(cfg =>
		{
			cfg.AddProfile<PresentationLayerMappingProfile>();
		});
		private static readonly IMapper Mapper = MapperConfiguration.CreateMapper();
		public PresentationLayerMappingProfile()
		{
			CreateMap<Card, CardModel>();
			CreateMap<OperationDTO1, Operation>();
			CreateMap<User, UserModel>();
			CreateMap<User, UserUpdate>();
			CreateMap<Category, CategoryModel>();
			CreateMap<Operation, OperationDTOOneOperation>();
			CreateMap<CardModel, Card>();
			CreateMap<CardDTO1, Card>();
			CreateMap<CardDTOColorOperation, Card>();
			CreateMap<UserModel, User>();
			CreateMap<UserGetID, User>();
			CreateMap<User, UserGetID>();
			CreateMap<OperationModel, Operation>();
			CreateMap<OperationDTOOneOperation, Operation>();
			CreateMap<UserUpdate, User>();
			CreateMap<CategoryModel, Category>();
			CreateMap<ColorCard, ColorModel>();
			CreateMap<Operation, OperationDTO1>();
			CreateMap<Card, CardDTO1>()
	.ForMember(dest => dest.Color, opt => opt.MapFrom(src => Mapper.Map<ColorModel>(src.ColorCard)));
			CreateMap<Operation, OperationModel>()
	.ForMember(dest => dest.Category, opt => opt.MapFrom(src => Mapper.Map<CategoryModel>(src.Category)));
			CreateMap<Card, CardDTOColorOperation>()
	.ForMember(dest => dest.Color, opt => opt.MapFrom(src => Mapper.Map<ColorModel>(src.ColorCard)))
	.ForMember(dest => dest.Operations, opt => opt.MapFrom(src => Mapper.Map<ICollection<OperationModel>>(src.Operations)));
		}
	}
}

