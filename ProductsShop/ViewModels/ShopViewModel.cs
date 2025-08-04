using Newtonsoft.Json;
using ProductsShop.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ProductsShop.ViewModels
{
    public class ShopViewModel : INotifyPropertyChanged
    {
        private const string ProductsFilePath = @"Data/products.json";
        public ObservableCollection<Product> Products { get; } = new();

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task LoadProductsAsync()
        {
            try
            {
                if (!File.Exists(ProductsFilePath))
                    return;

                var json = await File.ReadAllTextAsync(ProductsFilePath);
                var products = JsonConvert.DeserializeObject<List<Product>>(json);
                if(products != null)
                {
                    Products.Clear();
                    foreach (var p in products)
                        Products.Add(p);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
