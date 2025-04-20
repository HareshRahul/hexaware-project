using EcommerceApp.entity;

namespace EcommerceApp.dao
{
    public interface IProductRepository
    {
        bool CreateCategory(Category category);
        List<Category> GetAllCategories();

        bool AddProduct(Product product);
        List<Product> GetAllProducts();
        List<Product> GetProductsByCategory(int categoryId);
        bool DeleteProduct(int productId);

       
    }
}
