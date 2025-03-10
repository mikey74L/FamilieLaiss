using FamilieLaissFrontend.Client.ViewModels.Controls.Filter;
using FamilieLaissSharedUI.Components;
using Microsoft.AspNetCore.Components;

namespace FamilieLaissFrontend.Client.Controls.Filter;

public partial class FilterDateRangeControl(FilterDateRangeControlViewModel viewModel, 
    NavigationManager navigationManager): FlComponentBase<FilterDateRangeControlViewModel>(viewModel, navigationManager)
{
    
}