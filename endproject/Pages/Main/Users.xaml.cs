namespace endproject.Pages.Main;

public partial class Users : ContentPage {
    public Users(IServiceProvider provider) {
        InitializeComponent();
        BindingContext = provider.GetService<ViewModels.Users>();
    }
}