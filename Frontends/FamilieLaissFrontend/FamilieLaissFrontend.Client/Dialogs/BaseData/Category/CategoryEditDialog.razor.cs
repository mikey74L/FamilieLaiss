using FamilieLaissFrontend.Client.ViewModels.Dialogs.BaseData.Category;
using FamilieLaissSharedUI.Components;
using Microsoft.AspNetCore.Components;

namespace FamilieLaissFrontend.Client.Dialogs.BaseData.Category;

public partial class CategoryEditDialog(CategoryEditDialogViewModel viewModel, 
    NavigationManager navigationManager) : FlComponentBase<CategoryEditDialogViewModel>(viewModel, navigationManager)
{

}
