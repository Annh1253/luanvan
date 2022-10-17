using UserService.Dtos;
using UserService.Response;

namespace UserService.Contracts.InterfaceContracts
{
    public interface IRoleService
    {
        ServiceResponse<List<RoleDtoRequest>> GetRoles();
        ServiceResponse<RoleDtoRequest> GetById(int id);
 
        ServiceResponse<RoleDtoRequest> AddRole(RoleDtoRequest role);

        bool Exist(int id);

    }
}