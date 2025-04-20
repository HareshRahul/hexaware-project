using System;
using System.IO;

namespace EcommerceApp.exception
{
    public static class ExceptionHandler
    {
        private static string logFilePath = @"C:\EcommerceApp\Logs\error_log.txt"; // You can modify this path as per your needs

        // Method to handle exceptions
        public static void HandleException(Exception ex)
        {
            // Log the exception to a file (you can also use NLog or Serilog for advanced logging)
            LogException(ex);

            // Display user-friendly message
            Console.WriteLine($"An error occurred: {ex.Message}");

            // You can add specific handling depending on exception type
            if (ex is ProductNotFoundException)
            {
                Console.WriteLine("The product you're looking for doesn't exist.");
            }
            else if (ex is OrderNotFoundException)
            {
                Console.WriteLine("Your order could not be found.");
            }
            else if (ex is DatabaseConnectionException)
            {
                Console.WriteLine("There was an issue connecting to the database. Please try again later.");
            }
            else
            {
                Console.WriteLine("An unexpected error occurred.");
            }
        }

        // Method to log exception details into a file
        private static void LogException(Exception ex)
        {
            try
            {
                string logMessage = $"{DateTime.Now}: {ex.GetType().Name} - {ex.Message} \nStack Trace: {ex.StackTrace}\n";
                File.AppendAllText(logFilePath, logMessage);
            }
            catch (Exception logEx)
            {
                Console.WriteLine($"Error logging exception: {logEx.Message}");
            }
        }
    }
}
