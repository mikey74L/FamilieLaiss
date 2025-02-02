using FamilieLaissFrontend.Client.ViewModels.Controls.Filter;
using FamilieLaissSharedUI.Components;
using Microsoft.AspNetCore.Components;

namespace FamilieLaissFrontend.Client.Controls.Filter;

public partial class FilterControl(FilterControlViewModel viewModel, 
    NavigationManager navigationManager): FlComponentBase<FilterControlViewModel>(viewModel, navigationManager)
{
    
}