using Hangfire.Dashboard;

namespace API.Application.Filters
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            //var httpContext = context.GetHttpContext();

            //return httpContext.User.Claims.Any(e=>e.Value == "gerenciartarefasemsegundoplano");
            return true;
        }
    }
}
