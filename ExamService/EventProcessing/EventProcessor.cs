using AutoMapper;
using ExamService.Dtos;
using System.Text.Json;
using ExamService.Models;
using ExamService.Ultils;
using ExamService.Contracts.RepositoryContracts;
using ExamService.EventProcessing;
using ExamService.Dtos;

namespace ExamService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IExamRepository _examRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUserRepository _userRepository;

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
                    Console.WriteLine("---> New Credential Published Event Detected");
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
     
            this._examRepository = scope.ServiceProvider.GetRequiredService<IExamRepository>();
            this._questionRepository = scope.ServiceProvider.GetRequiredService<IQuestionRepository>();
            this._userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            
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
                Console.WriteLine($"Email: {credentialPublishedDto.Email}");
               
                try
                {
                    var userDtoRequest = _mapper.Map<UserRequestDto>(credentialPublishedDto);
                    var userEntity = _mapper.Map<User>(userDtoRequest);
                   
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