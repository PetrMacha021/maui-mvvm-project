namespace endproject.Pages;

public partial class Login : ContentPage {
    public Login(IServiceProvider provider) {
        InitializeComponent();
        BindingContext = provider.GetService<ViewModels.Login>();
    }
}