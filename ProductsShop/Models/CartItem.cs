using System.ComponentModel;

namespace ProductsShop.Models
{
    public class CartItem : INotifyPropertyChanged
    {
        public Product Product { get; set; }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Quantity)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalPrice)));
            }
        }

        public decimal TotalPrice => Product.Price * Quantity;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
