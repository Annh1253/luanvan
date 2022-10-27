using UserService.Dtos;
using UserService.Response;

namespace UserService.Contracts.InterfaceContracts
{
    public interface IRoleService
    {
        ServiceResponse<List<RoleDtoResponse>> GetRoles();
        ServiceResponse<RoleDtoResponse> GetById(int id);
 
        ServiceResponse<RoleDtoResponse> AddRole(RoleDtoRequest role);

        bool Exist(int id);

    }
}