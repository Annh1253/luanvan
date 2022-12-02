using AutoMapper;
using ExamService.Dtos;
using System.Text.Json;
using ExamService.Models;
using ExamService.Ultils;
using ExamService.Contracts.RepositoryContracts;
using ExamService.Dtos.ExamDone;
using ExamService.Contracts.ServiceContracts;
using ExamService.Response;
using ExamService.Dtos.Answer;

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
            Console.WriteLine("---> Event is: " + eventType.Event);
            switch (eventType.Event)
            {
                case "NewCredentialRegisted":
                    Console.WriteLine("---> New Credential Published Event Detected");
                    return EventType.NewCredentialRegisted;
                case "ExamDone":
                    Console.WriteLine("---> New Exam Done Published Event Detected");
                    return EventType.ExamDone;
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
            switch (eventType)
            {
                case EventType.NewCredentialRegisted:
                    Console.WriteLine("Processing new credential create...");
                    Console.WriteLine("Data: " + message);
                    AddUser(message);
                    break;
                case EventType.ExamDone:
                    Console.WriteLine("Processing new exam done...");
                    Console.WriteLine("Data: " + message);
                    proccessNewExamDone(message);
                    break;
                default:
                    break;
            }
        }

        private void AddUser(string message)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                var credentialPublishedDto = JsonSerializer.Deserialize<CredentialPublishedDto>(message);
                Console.WriteLine($"Email: {credentialPublishedDto.Email}");

                try
                {
                    var userDtoRequest = _mapper.Map<UserRequestDto>(credentialPublishedDto);
                    var userEntity = _mapper.Map<User>(userDtoRequest);

                    bool SavedSucceeded = _userRepository.AddUser(userEntity);
                    if (!SavedSucceeded)
                    {
                        throw new InvalidOperationException();
                    }
                    Console.WriteLine("Save new User Successfully");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"---> Could not add User to DB: {ex.Message}");
                }
            }
        }

        private void proccessNewExamDone(string message)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                try
                {
                    var service = scope.ServiceProvider.GetRequiredService<IAttempService>();
                    var examDoneDto = JsonSerializer.Deserialize<ExamDoneDto>(message);

                    Console.WriteLine(examDoneDto.ToString());
                    List<AttempRequestDto> attempList = new List<AttempRequestDto>();

                    foreach (var attemp in examDoneDto.Attemps)
                    {
                        

                        AttempRequestDto attempRequest = new AttempRequestDto()
                        {
                            Mode = attemp.mode,
                            TotalScore = attemp.totalScore,
                            TotalBonusScore = attemp.totalBonusScore,
                            Email = attemp.user,
                            ExamId = examDoneDto.ExternalExamId,
                            StartTime = attemp.startTime,
                            FinishTime = attemp.finishTime,
                            MaxCorrectStreak = attemp.maxCorrectStreak
                        };
                        attempRequest.Answers = new List<AnswerRequestDto>();
                        foreach (var answer in attemp.answers)
                        {
                            Console.WriteLine("Bonus score is : " + answer.bonus);
                            AnswerRequestDto answerRequestDto = new AnswerRequestDto()
                            {
                                OptionId = answer.optionId != null ? answer.optionId : 0,
                                Bonus = answer.bonus,
                                QuestionId = answer.questionId,
                                TotalTime = answer.totalTime != null ? answer.totalTime : 0
                            };
                            attempRequest.Answers.Add(answerRequestDto);
                        }

                        attempList.Add(attempRequest);
                    }
                    ServiceResponse<AttempResponseDto> response = service.AddAttemps(attempList);
                    Console.WriteLine(response.Message);
                    // var AttempRequestDto = _mapper.Map<AttempRequestDto>(examDoneDto);
                    // var userEntity = _mapper.Map<User>(userDtoRequest);

                    // bool SavedSucceeded = _userRepository.AddUser(userEntity); 
                    // if(!SavedSucceeded)
                    // {
                    //     throw new InvalidOperationException();
                    // }
                    // Console.WriteLine("Save new attemps Successfully");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"---> Could not add Attemps to DB: {ex.Message}");
                }
            }
        }

    }
}