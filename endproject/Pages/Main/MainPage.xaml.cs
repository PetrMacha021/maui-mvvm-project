namespace endproject.Pages.Main;

public partial class MainPage : ContentPage
{
    public MainPage(IServiceProvider provider)
    {
        InitializeComponent();
        BindingContext = provider.GetService<ViewModels.Main>();
    }
}
