using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using ProductsShop.Models;

namespace ProductsShop.Views
{
    public sealed partial class CartPage : Page
    {
        public MainPage Main;

        public CartPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Main = e.Parameter as MainPage;
        }

        private void RemoveCart_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var cartItem = button.DataContext as CartItem;
                if (cartItem != null)
                {
                    Main.CartVM.RemoveCartItem(cartItem);
                }
            }
        }
    }
}
