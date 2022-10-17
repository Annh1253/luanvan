using UserService.Dtos;
using UserService.Response;

namespace UserService.Contracts.InterfaceContracts
{
    public interface IUserService
    {
        ServiceResponse<List<UserDtoResponse>> GetUsers();
        ServiceResponse<UserDtoResponse> GetById(int id);
 
        ServiceResponse<UserDtoResponse> AddUser(int RoleId, UserDtoRequest user);

        ServiceResponse<UserDtoResponse> UpdateUser(UserDtoRequest user);
        bool Exist(int id);

        bool Exist(string email);
    }
}