using Newtonsoft.Json;
using ProductsShop.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace ProductsShop.ViewModels
{
    public class ShopViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Product> Products { get; } = new();

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task LoadProductsAsync()
        {
            var file = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(@"Data\products.json");
            var json = await Windows.Storage.FileIO.ReadTextAsync(file);
            var products = JsonConvert.DeserializeObject<List<Product>>(json);
            Products.Clear();
            foreach (var p in products)
                Products.Add(p);
        }
    }
}
