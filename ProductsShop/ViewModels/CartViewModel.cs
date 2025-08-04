using Newtonsoft.Json;
using ProductsShop.Models;
using ProductsShop.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace ProductsShop.ViewModels
{
    public enum SortType
    {
        ByName,
        ByPriceAsc,
        ByPriceDesc
    }

    public class CartViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<CartItem> CartItems { get; set; } = new();

        public event PropertyChangedEventHandler PropertyChanged;

        public int TotalQuantity => CartItems.Sum(i => i.Quantity);
        public decimal TotalPrice => CartItems.Sum(i => i.TotalPrice);

        public SortType CurrentSortType
        {
            get => currentSortType;
            set
            {
                if(currentSortType != value)
                {
                    currentSortType = value;
                    SortCartItems();
                    SaveSortType();
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentSortType)));
                }
            }
        }

        private const string CartFileName = "cart.json";
        private const string SortTypeKey = "CartSortType";
        private readonly StorageFolder localFolder = ApplicationData.Current.LocalFolder;

        private SortType currentSortType = SortType.ByName;

        private void SortCartItems()
        {
            IEnumerable<CartItem> sorted = (currentSortType switch
            {
                SortType.ByName => CartItems.OrderBy(i => i.Product.Name),
                SortType.ByPriceAsc => CartItems.OrderBy(i => i.TotalPrice),
                SortType.ByPriceDesc => CartItems.OrderByDescending(i => i.TotalPrice),
                _ => CartItems.OrderBy(i => i.Product.Name),
            }).ToList();

            CartItems.Clear();
            foreach (var item in sorted)
                CartItems.Add(item);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalPrice)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalQuantity)));
        }

        private void SaveSortType()
        {
            ApplicationData.Current.LocalSettings.Values[SortTypeKey] = CurrentSortType.ToString();
        }

        private void LoadSortType()
        {
            if (ApplicationData.Current.LocalSettings.Values.TryGetValue(SortTypeKey, out var value))
            {
                if (Enum.TryParse(typeof(SortType), value.ToString(), out var result))
                {
                    currentSortType = (SortType)result;
                }
            }
            else
                currentSortType = SortType.ByName;
        }

        public async Task AddProduct(Product product)
        {
            var existingItem = CartItems.FirstOrDefault(p => p.Product.Id == product.Id);

            if (existingItem != null) 
                existingItem.Quantity++;
            else 
                CartItems.Add(new CartItem { Product = product, Quantity = 1 });

            SortCartItems();
            await SaveCartAsync();

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalPrice)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalQuantity)));
        }

        public async Task RemoveCartItem(CartItem cartItem)
        {
            CartItems.Remove(cartItem);

            SortCartItems();
            await SaveCartAsync();

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalPrice)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalQuantity)));
        }

        public async Task LoadCartAsync()
        {
            LoadSortType();

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
