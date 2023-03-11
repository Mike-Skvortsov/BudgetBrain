using Autofac;
using DataAccess.Repositories;

namespace DataAccess
{
    public class DataAccessRegistrationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DataBaseContext>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<UserRepository>().As<IUserRepository>();
            builder.RegisterType<CardRepository>().As<ICardRepository>();
            builder.RegisterType<ColorRepository>().As<IColorRepository>();
            builder.RegisterType<CategoryRepository>().As<ICategoryRepository>();
            builder.RegisterType<OperationRepository>().As<IOperationRepository>();
        }
    }
}
