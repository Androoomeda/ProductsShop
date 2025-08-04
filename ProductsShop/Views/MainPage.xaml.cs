using ProductsShop.ViewModels;
using ProductsShop.Views;
using Windows.UI.Xaml.Controls;

namespace ProductsShop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a <see cref="Frame">.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public ShopViewModel ShopVM { get; } = new ();
        public CartViewModel CartVM { get; } = new();

        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += async (_, __) =>
            {
                await ShopVM.LoadProductsAsync();
                NavView.SelectedItem = NavView.MenuItems[0];
                ContentFrame.Navigate(typeof(ShopPage), this);
            };
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected) return;

            var selectedItem = args.SelectedItemContainer as NavigationViewItem;

            if(selectedItem != null)
            {
                if (selectedItem.Tag.ToString() == "Shop")
                    ContentFrame.Navigate(typeof(ShopPage), this);
                else if (selectedItem.Tag.ToString() == "Cart")
                    ContentFrame.Navigate(typeof(CartPage), this);
            }
        }
    }
}
