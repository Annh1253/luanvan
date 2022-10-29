using UserService.Dtos;
using UserService.Response;

namespace UserService.Contracts.InterfaceContracts
{
    public interface IUserService
    {
        ServiceResponse<List<UserDtoResponse>> GetUsers();
        ServiceResponse<UserDtoResponse> GetById(int id);
 
        ServiceResponse<UserDtoResponse> AddUser(int RoleId, UserDtoRequest user);

        ServiceResponse<UserDtoResponse> UpdateUser(int UserId, UserDtoRequest user);
        public ServiceResponse<UserDtoResponse> GetByEmail(string email);
        ServiceResponse<UserDtoResponse> DeleteUser(int UserId);

        bool Exist(int id);

        bool Exist(string email);
    }
}