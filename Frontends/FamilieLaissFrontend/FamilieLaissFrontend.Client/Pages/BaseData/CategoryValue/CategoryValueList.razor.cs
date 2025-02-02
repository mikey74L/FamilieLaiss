using FamilieLaissFrontend.Client.ViewModels.Pages.BaseData.CategoryValue;
using FamilieLaissSharedUI.Components;
using Microsoft.AspNetCore.Components;

namespace FamilieLaissFrontend.Client.Pages.BaseData.CategoryValue;

public partial class CategoryValueList(CategoryValueListViewModel viewModel, 
    NavigationManager navigationManager): FlComponentBase<CategoryValueListViewModel>(viewModel, navigationManager)
{
    
}