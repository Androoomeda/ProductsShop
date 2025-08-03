using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ProductsShop.Views
{
    public sealed partial class ShopPage : Page
    {
        public MainPage Main;

        public ShopPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Main = e.Parameter as MainPage;
        }
    }
}
