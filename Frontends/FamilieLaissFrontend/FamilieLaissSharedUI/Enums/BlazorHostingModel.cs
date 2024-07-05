using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilieLaissSharedUI.Enums;

/// <summary>
/// The ASP.NET Core Blazor hosing models
/// </summary>
public enum BlazorHostingModel
{
    /// <summary>
    /// No hosting model is specified
    /// </summary>
    NotSpecified,

    /// <summary>
    /// Blazor Server 
    /// </summary>
    Server,

    /// <summary>
    /// Blazor WebAssembly
    /// </summary>
    WebAssembly,

    /// <summary>
    /// Blazor Hybrid
    /// </summary>
    Hybrid,

    /// <summary>
    /// Interactive WebApp
    /// </summary>
    WebApp
}