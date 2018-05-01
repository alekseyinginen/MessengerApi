using AutoMapper;

namespace MessengerApi.Util
{
    public static class AutomapperConfiguration {
        public static void Configure() {
            Mapper.Initialize(cfg => {
                cfg.AddProfile(new AutoMapperProfile());
            });
        }
    }
}
