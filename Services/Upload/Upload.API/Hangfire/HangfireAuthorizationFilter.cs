using Hangfire.Dashboard;

namespace Upload.API.Hangfire;

/// <summary>
/// 
/// </summary>
public class HangfireDevelopmentAuthorizationFilter : IDashboardAuthorizationFilter
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public bool Authorize(DashboardContext context)
    {
        return true;
    }
}
