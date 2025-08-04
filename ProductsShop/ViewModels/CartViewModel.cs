using ProductsShop.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace ProductsShop.ViewModels
{
    public class CartViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<CartItem> CartItems { get; set; } = new();

        public event PropertyChangedEventHandler PropertyChanged;

        public int TotalQuantity => CartItems.Sum(i => i.Quantity);
        public decimal TotalPrice => CartItems.Sum(i => i.TotalPrice);

        public void AddProduct(Product product)
        {
            var existingItem = CartItems.FirstOrDefault(p => p.Product.Id == product.Id);

            if (existingItem != null) 
                existingItem.Quantity++;
            else 
                CartItems.Add(new CartItem { Product = product, Quantity = 1 });

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalPrice)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalQuantity)));
        }

        public void RemoveCartItem(CartItem cartItem)
        {
            CartItems.Remove(cartItem);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalPrice)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalQuantity)));
        }
    }
}
