using FamilieLaissFrontend.Client.ViewModels.Controls.Filter;
using FamilieLaissSharedUI.Components;
using Microsoft.AspNetCore.Components;

namespace FamilieLaissFrontend.Client.Controls.Filter;

public partial class FilterNumberListControl(FilterNumberListControlViewModel viewModel, 
    NavigationManager navigationManager): FlComponentBase<FilterNumberListControlViewModel>(viewModel, navigationManager)
{
    
}