namespace endproject.Pages.Main;

public partial class Profile : ContentPage {
    public Profile(IServiceProvider provider) {
        InitializeComponent();
        BindingContext = provider.GetService<ViewModels.Profile>();
    }
}