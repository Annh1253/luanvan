using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UserService.Contracts.InterfaceContracts;
using UserService.Dtos;
using UserService.Response;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public RoleController(IRoleService roleService, IMapper mapper)
        {
            this._roleService = roleService;
            this._mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetRoles()
        {
            ServiceResponse<List<RoleDtoResponse>> serviceResponse = _roleService.GetRoles();
            ControllerResponse<List<RoleDtoResponse>> controllerResponse = _mapper.Map<ControllerResponse<List<RoleDtoResponse>>>(serviceResponse);
            return StatusCode((int)serviceResponse.StatusCode, controllerResponse);
        }

        
    }
}