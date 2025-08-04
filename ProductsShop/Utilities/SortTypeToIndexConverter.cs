using System;
using Windows.UI.Xaml.Data;
using ProductsShop.ViewModels;

namespace ProductsShop.Utilities
{
    public partial class SortTypeToIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is SortType sortType)
            {
                return sortType switch
                {
                    SortType.ByName => 0,
                    SortType.ByPriceAsc => 1,
                    SortType.ByPriceDesc => 2,
                    _ => 0,
                };
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is int index)
            {
                return index switch
                {
                    0 => SortType.ByName,
                    1 => SortType.ByPriceAsc,
                    2 => SortType.ByPriceDesc,
                    _ => SortType.ByName,
                };
            }
            return SortType.ByName;
        }
    }
}
