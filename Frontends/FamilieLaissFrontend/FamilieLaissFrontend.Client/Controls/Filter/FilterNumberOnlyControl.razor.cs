using FamilieLaissFrontend.Client.ViewModels.Controls.Filter;
using FamilieLaissSharedUI.Components;
using Microsoft.AspNetCore.Components;

namespace FamilieLaissFrontend.Client.Controls.Filter;

public partial class FilterNumberOnlyControl(FilterNumberOnlyControlViewModel viewModel, 
    NavigationManager navigationManager): FlComponentBase<FilterNumberOnlyControlViewModel>(viewModel, navigationManager)
{
    
}