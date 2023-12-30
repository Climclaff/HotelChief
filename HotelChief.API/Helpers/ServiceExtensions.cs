namespace HotelChief.API.Helpers
{
    using AutoMapper;
    using HotelChief.Infrastructure.Mappers;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceExtensions
    {
        public static void AddAutoMapper(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new DomainToInfrastructureMappingProfile());
                mc.AddProfile(new InfrastructureToDomainMappingProfile());
                mc.AddProfile(new CoreToViewModelMappingProfile());
                mc.AddProfile(new ViewModelToCoreMappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
