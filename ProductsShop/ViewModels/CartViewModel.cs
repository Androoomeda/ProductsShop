using Newtonsoft.Json;
using ProductsShop.Models;
using ProductsShop.Utilities;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace ProductsShop.ViewModels
{
    public class CartViewModel : INotifyPropertyChanged
    {

        private const string CartFileName = "cart.json";
        private StorageFolder localFolder = ApplicationData.Current.LocalFolder;

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

        public async Task LoadCartItemsAsync()
        {
            try
            {
                var file = await localFolder.GetFileAsync(CartFileName);
                var json = await FileIO.ReadTextAsync(file);
                var items = JsonConvert.DeserializeObject<ObservableCollection<CartItem>>(json);
                if (items != null)
                {
                    CartItems.Clear();
                    foreach (var item in items)
                        CartItems.Add(item);

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalPrice)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalQuantity)));
                }
            }
            catch (FileNotFoundException)
            {
                await localFolder.CreateFileAsync(CartFileName, CreationCollisionOption.ReplaceExisting);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public async Task SaveCartAsync()
        {
            try
            {
                var settings = new JsonSerializerSettings();
                settings.Converters.Add(new DecimalJsonConverter());

                var json = JsonConvert.SerializeObject(CartItems, Formatting.Indented, settings);

                var file = await localFolder.CreateFileAsync(CartFileName, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(file, json);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
