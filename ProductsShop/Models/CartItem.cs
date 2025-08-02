namespace ProductsShop.Models
{
    internal class CartItem
    {
        public Product Product { get; set; }

        public int Quantity;

        public decimal TotalPrice => Product.Price * Quantity;
    }
}
