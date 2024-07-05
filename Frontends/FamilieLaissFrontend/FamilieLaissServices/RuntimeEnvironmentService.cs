using FamilieLaissInterfaces.Services;
using Microsoft.JSInterop;

namespace FamilieLaissServices;

public class RuntimeEnvironmentService(IJSRuntime jsRuntime) : IRuntimeEnvironmentService
{
    public bool IsWebAssembly()
    {
        return jsRuntime is IJSInProcessRuntime;
    }
}
