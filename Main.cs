
using System;
using System.Linq;
using EcommerceApp.dao;
using EcommerceApp.entity;
using EcommerceApp.util;
using System.Collections.Generic;

namespace EcommerceApp.main
{
    class MainModule
    {
        static void Main(string[] args)
        {
            IUserRepository userRepo = new UserRepositoryImpl();
            IProductRepository productRepo = new ProductRepositoryImpl();
            ICustomerRepository customerRepo = new CustomerRepositoryImpl();

            User currentUser = null;
            Console.WriteLine("                            =========== Welcome to Game of Thrones E-Shop ===========\n");

            while (true)
            {
                Console.WriteLine("1. Register\n2. Login\n3. Exit");
                Console.Write("Enter choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        UserUIHelper.RegisterUser(userRepo);
                        break;


                    case "2":
                        currentUser = UserUIHelper.LoginUser(userRepo);
                        if (currentUser != null)
                        {
                            if (currentUser.Role == UserRole.Admin)
                                AdminMenu(productRepo, userRepo);
                            else
                                CustomerMenu(productRepo, customerRepo, currentUser);
                        }
                        break;


                    case "3":
                        Console.WriteLine("Thanks for visiting Westeros Market! Goodbye.");
                        return;

                    default:
                        Console.WriteLine("Invalid choice.\n");
                        break;
                }
            }
        }

        static void AdminMenu(IProductRepository productRepo, IUserRepository userRepo)

        {
            while (true)
            {
                Console.WriteLine("\n--- Admin Panel ---");
                Console.WriteLine("1. Add Category\n2. Add Product\n3. View All Products\n4. View Categories\n5.Delete Customer\n6. Exit to Main Menu");
                Console.Write("Choose: ");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        Console.Write("Enter Category Name: ");
                        string catName = Console.ReadLine();
                        bool added = productRepo.CreateCategory(new Category(0, catName));
                        Console.WriteLine(added ? "Category added." : "Failed to add.");
                        break;

                    case "2":
                        AdminUIHelper.AddProduct(productRepo);
                        break;


                    case "3":
                        AdminUIHelper.ShowProductsByCategory(productRepo);
                        break;


                    case "4":
                        var categories = productRepo.GetAllCategories()
                               .OrderBy(cat => cat.CategoryId)
                               .ToList();
                        categories.ForEach(cat => Console.WriteLine($"ID: {cat.CategoryId}, Name: {cat.CategoryName}"));
                        break;
                    case "5":
                        Console.Write("Enter admin password to proceed: ");
                        string enteredPassword = Console.ReadLine();

                        Console.Write("Enter User ID of customer to delete: ");
                        if (int.TryParse(Console.ReadLine(), out int deleteId))
                        {
                            bool deleted = userRepo.DeleteCustomerByAdmin(deleteId, enteredPassword);
                            Console.WriteLine(deleted ? "Customer deleted successfully." : "Failed to delete customer.");
                        }
                        else
                        {
                            Console.WriteLine("Invalid ID entered.");
                        }
                        break;


                    case "6":
                        return;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        static void CustomerMenu(IProductRepository productRepo, ICustomerRepository customerRepo, User user)
        {
            while (true)
            {
                Console.WriteLine("\n--- Customer Dashboard ---");
                Console.WriteLine("1. View Products\n2. Add to Cart\n3. View Cart\n4. Place Order\n5. View Orders\n6. View Categories\n7. View Wishlist\n8. Remove from Wishlist\n9. Logout");
                Console.Write("Select: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ProductUIHelper.BrowseProductsByCategory(productRepo, customerRepo, user.UserId);
                        break;

                    case "2":
                        Console.Write("Enter Product Id: ");
                        int pid = int.Parse(Console.ReadLine());
                        Console.Write("Quantity: ");
                        int quantity = int.Parse(Console.ReadLine());
                        Console.WriteLine(customerRepo.AddToCart(user.UserId, pid, quantity) ? "Added to cart." : "Failed to add.");
                        break;

                    case "3":
                        customerRepo.DisplayCartItems(user.UserId, productRepo);
                        break;

                    case "4":
                        CustomerUIHelper.ProcessOrder(customerRepo, productRepo, user);
                        break;

                    case "5":
                        CustomerUIHelper.DisplayCustomerOrders(customerRepo, user.UserId);
                        break;



                    case "6":
                        var allCategories = productRepo.GetAllCategories();
                        allCategories.ForEach(cat => Console.WriteLine($"ID: {cat.CategoryId}, Name: {cat.CategoryName}"));
                        break;

                    case "7":
                        CustomerUIHelper.ViewWishlist(customerRepo, productRepo, user.UserId);
                        break;

                    case "8":
                        CustomerUIHelper.RemoveFromWishlist(customerRepo, user.UserId);
                        break;

                    case "9":
                        return;


                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

       
    }
}