using FamilieLaissFrontend.Client.ViewModels.Controls.Filter;
using FamilieLaissSharedUI.Components;
using Microsoft.AspNetCore.Components;

namespace FamilieLaissFrontend.Client.Controls.Filter;

public partial class FilterStringOnlyControl(FilterStringOnlyControlViewModel viewModel, 
    NavigationManager navigationManager): FlComponentBase<FilterStringOnlyControlViewModel>(viewModel, navigationManager)
{
    
}