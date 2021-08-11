using AutoMapper;
using BusinessLogicLayer.Model;
using DataAccessLayer;
using DataAccessLayer.Common;
using DataAccessLayer.Entities;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class AuthenticationBLL
    {
        private readonly UserDAL userDAL;
        private readonly Mapper mapper;
        private readonly string KEY = "This is my test key";
        public AuthenticationBLL()
        {
            userDAL = new UserDAL(new JwtAuthenticationManager(KEY));
            var _configUser = new MapperConfiguration(configure => configure.CreateMap<User, UserModel>().ReverseMap());
            mapper = new Mapper(_configUser);
        }
        public object GetUserDetails(string authorization, ref string message)
        {
            return userDAL.GetUserDetails(authorization, ref message);
            
        }
        public string RegisterUser(UserModel userModel)
        {
            return userDAL.RegisterUser(mapper.Map<UserModel, User>(userModel));
        }

        public string AuthenticateUser(string email, string password)
        {
            return userDAL.AuthenticateUser(email, password);
        }
    }
}
