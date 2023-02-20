using Auth.Common;
using Autofac;
using BL.Services;
using DataAccess;
using Microsoft.Extensions.Options;

namespace BL
{
    public class BlRegistrationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<DataAccessRegistrationModule>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<CardService>().As<ICardService>();
            builder.RegisterType<OperationService>().As<IOperationService>();
		}
	}
}
