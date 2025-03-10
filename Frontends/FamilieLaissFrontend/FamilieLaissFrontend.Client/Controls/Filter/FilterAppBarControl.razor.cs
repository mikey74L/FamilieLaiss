using FamilieLaissFrontend.Client.ViewModels.Controls.Filter;
using FamilieLaissSharedUI.Components;
using Microsoft.AspNetCore.Components;

namespace FamilieLaissFrontend.Client.Controls.Filter;

public partial class FilterAppBarControl<TModel, TSortInput, TFilterInputType>(FilterAppBarControlViewModel<TModel,TSortInput,TFilterInputType> viewModel, 
    NavigationManager navigationManager): FlComponentBase<FilterAppBarControlViewModel<TModel,TSortInput,TFilterInputType>>(viewModel, navigationManager)
{
    
}