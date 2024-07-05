namespace FamilieLaissSharedUI.Interfaces;

public interface IView<out TViewModel> : IView
    where TViewModel : IViewModelBase
{
    // Skip
}

public interface IView
{
    // Skip
}