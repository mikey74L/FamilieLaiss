using FamilieLaissFrontend.Client.ViewModels.Pages.BaseData.Category;
using FamilieLaissSharedUI.Components;
using Microsoft.AspNetCore.Components;

namespace FamilieLaissFrontend.Client.Pages.BaseData.Category;

public partial class CategoryList(CategoryListViewModel viewModel, 
    NavigationManager navigationManager): FlComponentBase<CategoryListViewModel>(viewModel, navigationManager)
{
    
}