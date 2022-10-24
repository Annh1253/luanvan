using AutoMapper;
using UserService.Dtos;
using System.Text.Json;
using UserService.Models;
using UserService.Ultils;
using UserService.Contracts.RepositoryContracts;

namespace UserService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("---> Determine Event");
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);
            Console.WriteLine("---> Event is: "+eventType.Event);
            switch(eventType.Event)
            {
                case "NewCredentialRegisted":
                    Console.WriteLine("---> Platform Published Event Detected");
                    return EventType.NewCredentialRegisted;
                default:
                    Console.WriteLine("---> Unknown Event");
                    return EventType.Undetermined;
            }
        }

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
           
            this._mapper = mapper;
            this._scopeFactory = scopeFactory;
            var scope = _scopeFactory.CreateScope();
     
            this._userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            this._roleRepository = scope.ServiceProvider.GetRequiredService<IRoleRepository>();
        
            
        }
        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);
            switch(eventType)
            {
                case EventType.NewCredentialRegisted:
                    AddUser(message);
                    break;
                default:
                    break;
            }
        }

        private void AddUser(string message)
        {
            using(var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                var credentialPublishedDto = JsonSerializer.Deserialize<CredentialPublishedDto>(message);
                Console.WriteLine($"Email: {credentialPublishedDto.Email}, Password: {credentialPublishedDto.Password}");
               
                try
                {
                    var userDtoRequest = _mapper.Map<UserDtoRequest>(credentialPublishedDto);
                    var userEntity = _mapper.Map<User>(userDtoRequest);
                    foreach(string role in credentialPublishedDto.Roles)
                    {
                        Role roleEntity =  _roleRepository.GetByName(role);
                        if(roleEntity == null)
                        {   
                            Console.WriteLine($"Role {role} not found");
                        }
                        userEntity.Roles.Add(roleEntity);
                    }
                    bool SavedSucceeded = _userRepository.AddUser(userEntity); 
                    if(!SavedSucceeded)
                    {
                        throw new InvalidOperationException();
                    }
                    Console.WriteLine("Save new User Successfully");
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"---> Could not add User to DB: {ex.Message}");
                }
            }
        }

    }
}