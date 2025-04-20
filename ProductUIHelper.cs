using System;
using System.Linq;
using EcommerceApp.dao;
using EcommerceApp.entity;
using EcommerceApp.exception;

namespace EcommerceApp.util
{
    public static class ProductUIHelper
    {
        public static void BrowseProductsByCategory(
            IProductRepository productRepo,
            ICustomerRepository customerRepo,
            int userId)
        {
            try
            {
                var categories = productRepo.GetAllCategories();
                if (categories.Count == 0)
                {
                    Console.WriteLine("No categories found.");
                    return;
                }

                categories = categories.OrderBy(cat => cat.CategoryId).ToList();
                Console.WriteLine("\nAvailable Categories:");
                categories.ForEach(cat =>
                    Console.WriteLine($"ID: {cat.CategoryId} \nName: {cat.CategoryName}"));

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

                        Console.WriteLine("\nWant to add to wishlist? \nType ProductId followed by 'W' (e.g., 2W). Or press Enter to skip.");
                        string input = Console.ReadLine();
                        CustomerUIHelper.PromptWishlistAdd(customerRepo, userId, input);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid category ID.");
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e);
            }
        }
    }
}
