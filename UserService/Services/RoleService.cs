using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using UserService.Contracts.InterfaceContracts;
using UserService.Contracts.RepositoryContracts;
using UserService.Dtos;
using UserService.Models;
using UserService.Response;

namespace UserService.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleService(IRoleRepository roleRepository, IMapper mapper)
        {
            this._mapper = mapper;
            this._roleRepository = roleRepository;
            
        }
        public ServiceResponse<RoleDtoResponse> AddRole(RoleDtoRequest role)
        {
            Role roleEntity = _mapper.Map<Role>(role);
            bool SavedSucceeded = _roleRepository.AddRole(roleEntity);   
            
            // if guard
            if (!SavedSucceeded) return new ServiceResponse<RoleDtoResponse>(){
                Success = false,
                StatusCode = HttpStatusCode.InternalServerError,
                Message = "Some thing went wrong when saving"
            };
            
            RoleDtoResponse roleReadDto = _mapper.Map<RoleDtoResponse>(roleEntity);
            var serviceResponse = new ServiceResponse<RoleDtoResponse>();
            serviceResponse.Data = roleReadDto;
            return serviceResponse;
        }

        public bool Exist(int id)
        {
            return _roleRepository.Exist(id);
        }

    
        public ServiceResponse<RoleDtoResponse> GetById(int id)
        {
            Role roleEntity = _roleRepository.GetById(id);
         
            
            // if guard
            if (roleEntity != null) return new ServiceResponse<RoleDtoResponse>(){
                Success = false,
                StatusCode = HttpStatusCode.NotFound,
                Message = "Role not found"
            };
            
            RoleDtoResponse roleReadDto = _mapper.Map<RoleDtoResponse>(roleEntity);
            var serviceResponse = new ServiceResponse<RoleDtoResponse>();
            serviceResponse.Data = roleReadDto;
            return serviceResponse;
        }

        public ServiceResponse<List<RoleDtoResponse>> GetRoles()
        {
            List<Role> roleList = (List<Role>) _roleRepository.GetAllRoles();
            List<RoleDtoResponse> roleReadDtoList = _mapper.Map<List<RoleDtoResponse>>(roleList);
             var serviceResponse = new ServiceResponse<List<RoleDtoResponse>>();
            serviceResponse.Data = roleReadDtoList;
            return serviceResponse;
        }

    }
}