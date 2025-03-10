using FamilieLaissFrontend.Client.ViewModels.Controls.Filter;
using FamilieLaissSharedUI.Components;
using Microsoft.AspNetCore.Components;

namespace FamilieLaissFrontend.Client.Controls.Filter;

public partial class FilterGroupControl(FilterGroupControlViewModel viewModel, 
    NavigationManager navigationManager): FlComponentBase<FilterGroupControlViewModel>(viewModel, navigationManager)
{
    
}