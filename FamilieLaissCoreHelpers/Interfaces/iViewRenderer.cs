using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FamilieLaissCoreHelpers.Interfaces
{
    public interface iViewRenderer
    {
        Task<string> RenderToStringAsync(string viewName);
        Task<string> RenderToStringAsync<TModel>(string viewName, TModel model);
        string RenderToString<TModel>(string viewPath, TModel model);
        string RenderToString(string viewPath);
    }
}
