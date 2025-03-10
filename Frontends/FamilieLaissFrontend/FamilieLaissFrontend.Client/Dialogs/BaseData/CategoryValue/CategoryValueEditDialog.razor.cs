using FamilieLaissFrontend.Client.ViewModels.Dialogs.BaseData.CategoryValue;
using FamilieLaissSharedUI.Components;
using Microsoft.AspNetCore.Components;

namespace FamilieLaissFrontend.Client.Dialogs.BaseData.CategoryValue;

public partial class CategoryValueEditDialog(CategoryValueEditDialogViewModel viewModel, 
    NavigationManager navigationManager) : FlComponentBase<CategoryValueEditDialogViewModel>(viewModel, navigationManager)
{
    
}