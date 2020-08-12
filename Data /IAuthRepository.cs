using System.Threading.Tasks;
using DatingApp.API.Modles;

namespace DatingApp.API.Data_
{
    public interface IAuthRepository
    {
         Task<User> Register(User user, string password);
         Task<User> Login(string username, string password);
         Task<bool> UserExists(string username);
    }
}

// We are using the Repository Pattern here which is an another layer of abstraction beyond an ORM (most likely Entity Framworks) 
// to an Interfaces methods that are implemented in concrete rpository. 
// In other words the Controllers does not have to both declare and implement the logic and even have to duplicate a piece of logic that 
// will most likely need to be used in another controller. We can just use methods that we know exists via inheriting the reposity Interface and using 
// the method stubs. The actual Implementation of these methods will exist in a Reposiory which can be update or changed out completely and the controller 
// would not care less about this as long as the Repository implements the methods from the Interface.