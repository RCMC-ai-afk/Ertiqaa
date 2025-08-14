using Orbits.GeneralProject.BLL.HangfireService;

namespace OrbitsProject.API
{
    public static class program
    {
        public static IApplicationBuilder RunHangfireJobs(this IApplicationBuilder builder)
        {
            using (var scope = builder.ApplicationServices.CreateScope())
            {
                var hangfireJobService = scope.ServiceProvider.GetService<IHangfireBLL>();
                if (hangfireJobService != null)
                {
                    hangfireJobService.Jobs();
                }
            }
            return builder;
        }
    }
}
