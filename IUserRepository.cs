using EcommerceApp.entity;

namespace EcommerceApp.dao
{
    public interface IUserRepository
    {
        bool RegisterUser(User user);
        bool Login(string email, string password, out User loggedUser);  // out param for session
        User GetUserById(int userId);
        List<User> GetAllUsers();
        bool DeleteCustomerByAdmin(int deleteId, string enteredPassword);
        bool DeleteUser(int userId);
    }
}
