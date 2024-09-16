using endproject.Data.Models;

namespace endproject.Pages.Main;

public partial class MainPage : ContentPage
{
    public MainPage(IServiceProvider provider)
    {
        InitializeComponent();
        BindingContext = provider.GetService<ViewModels.Main>();
    }

    private void ListView_OnItemSelected(object? sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is Item selectedItem)
        {
            foreach (var item in (BindingContext as ViewModels.Main).Items)
            {
                item.IsMessageVisible = false;
            }

            selectedItem.IsMessageVisible = true;
        }
    }

    private void VisualElement_OnUnfocused(object? sender, FocusEventArgs e)
    {
        foreach (var item in (BindingContext as ViewModels.Main).Items)
        {
            item.IsMessageVisible = false;
        }
    }
}
