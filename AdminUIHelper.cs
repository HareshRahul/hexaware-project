using EcommerceApp.dao;
using EcommerceApp.entity;

namespace EcommerceApp.util
{
    public static class AdminUIHelper
    {
        public static void AddProduct(IProductRepository productRepo)
        {
            Console.Write("Name: ");
            string name = Console.ReadLine();

            Console.Write("Price: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price))
            {
                Console.WriteLine("Invalid price.");
                return;
            }

            Console.Write("Description: ");
            string desc = Console.ReadLine();

            Console.Write("Stock Quantity: ");
            if (!int.TryParse(Console.ReadLine(), out int qty))
            {
                Console.WriteLine("Invalid quantity.");
                return;
            }

            Console.Write("Category Id: ");
            if (!int.TryParse(Console.ReadLine(), out int cid))
            {
                Console.WriteLine("Invalid category ID.");
                return;
            }

            Product prod = new Product(0, name, price, desc, qty, cid);
            bool success = productRepo.AddProduct(prod);
            Console.WriteLine(success ? " Product added." : " Failed to add product.");
        }
        public static void ShowProductsByCategory(IProductRepository productRepo)
        {
            var categories = productRepo.GetAllCategories()
                                .OrderBy(cat => cat.CategoryId)
                                .ToList();
            if (categories.Count == 0)
            {
                Console.WriteLine("No categories found.");
                return;
            }

            Console.WriteLine("\nAvailable Categories:");
            categories.ForEach(cat => Console.WriteLine($"ID: {cat.CategoryId}, Name: {cat.CategoryName}"));

            Console.Write("Enter Category ID to view products: ");
            if (int.TryParse(Console.ReadLine(), out int catId))
            {
                var products = productRepo.GetAllProducts()
                                          .Where(p => p.CategoryId == catId)
                                          .ToList();
                if (products.Count == 0)
                {
                    Console.WriteLine("No products found for this category.");
                }
                else
                {
                    Console.WriteLine($"\nProducts in Category ID {catId}:");
                    products.ForEach(Console.WriteLine);
                }
            }
            else
            {
                Console.WriteLine("Invalid category ID.");
            }
        }

    }
}
