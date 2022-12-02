using System.Net;
using AutoMapper;
using ExamService.Contracts.RepositoryContracts;
using ExamService.Contracts.ServiceContracts;
using ExamService.Dtos;
using ExamService.Helpers;
using ExamService.Models;
using ExamService.Response;

namespace ExamService.Services
{
    public class AttempService : IAttempService
    {
        private readonly IAttempRepository _attempRepository;
        private readonly IUserRepository _userRepository;
        private readonly IExamRepository _examRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IOptionRepository _optionRepository;
        private readonly IMapper _mapper;

        public AttempService(IAttempRepository attempRepository, IUserRepository userRepository, IExamRepository examRepository, IQuestionRepository questionRepository, IOptionRepository optionRepository, IMapper mapper)
        {
            this._attempRepository = attempRepository;
            this._userRepository = userRepository;
            this._examRepository = examRepository;
            this._questionRepository = questionRepository;
            this._optionRepository = optionRepository;
            this._mapper = mapper;
        }
        public ServiceResponse<AttempResponseDto> AddAttemp(AttempRequestDto AttempRequestDto)
        {

            Attemp NewAttemp = _mapper.Map<Attemp>(AttempRequestDto);
            throw new NotImplementedException();
        }

        public ServiceResponse<AttempResponseDto> AddAttemps(List<AttempRequestDto> AttempRequestDto)
        {
            try
            {
                List<Attemp> AttempList = new List<Attemp>();
                foreach (var AttempDto in AttempRequestDto)
                {
                    Exam exam = _examRepository.GetById(AttempDto.ExamId);

                    User user = _userRepository.GetByEmail(AttempDto.Email);

                    var AnswerList = new List<Answer>();

                    foreach (var Answer in AttempDto.Answers)
                    {

                        Question question = _questionRepository.GetById(Answer.QuestionId);
                        Option option = _optionRepository.GetById(Answer.OptionId);
                        Answer NewAnswer = new Answer()
                        {
                            Question = question,
                            Option = null,
                            TotalTime = Answer.TotalTime,
                            Bonus = Answer.Bonus
                        };
                        if (option != null)
                        {
                            NewAnswer.Option = option;
                        }
                        AnswerList.Add(NewAnswer);
                    }

                    Attemp NewAttemp = new Attemp()
                    {
                        TotalScore = AttempDto.TotalScore,
                        Mode = AttempDto.Mode,
                        TotalBonusScore = AttempDto.TotalBonusScore,
                        Exam = exam,
                        User = user,
                        StartTime = AttempDto.StartTime,
                        FinishTime = AttempDto.FinishTime,
                        MaxCorrectStreak = AttempDto.MaxCorrectStreak,
                        Answers = AnswerList
                    };
                    AttempList.Add(NewAttemp);
                }
                _attempRepository.AddAttemps(AttempList);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<AttempResponseDto>()
                {
                    Data = null,
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
            return new ServiceResponse<AttempResponseDto>()
            {
                Data = null,
                Success = true,
                Message = "Save attemps successfully",
                StatusCode = HttpStatusCode.Created
            }; ;
        }

        public bool Exist(int id)
        {
            throw new NotImplementedException();
        }

        public ServiceResponse<List<AttempResponseDto>> GetAttemps(AttempRequestFilter AttempFilter)
        {
            if (AttempFilter != null)
            {
                return GetAttempsByCriteria(AttempFilter);
            }
            List<Attemp> attempList = _attempRepository.GetAllAttemps();
            List<AttempResponseDto> attempReadDtoList = _mapper.Map<List<AttempResponseDto>>(attempList);
            var serviceResponse = new ServiceResponse<List<AttempResponseDto>>();
            serviceResponse.Success = true;
            serviceResponse.Data = attempReadDtoList;
            return serviceResponse;
        }

        public ServiceResponse<List<AttempResponseDto>> GetAttempsByCriteria(AttempRequestFilter AttempFilter)
        {
            var Predicate = PredicateBuilder.True<Attemp>();
            try
            {
                foreach (var property in AttempFilter.GetType().GetProperties())
                {
                    var val = property.GetValue(AttempFilter);
                    if (val != null)
                    {
                        if (property.Name == "Email")
                            Predicate = Predicate.And(attemp => val.Equals(attemp.User.Email));
                        else if (property.Name == "ExamId")
                            Predicate = Predicate.And(attemp => val.Equals(attemp.Exam.Id));
                    }
                }

                List<Attemp> AttempList = _attempRepository.GetAllAttempsWithPredicate(Predicate);
                List<AttempResponseDto> AttempListResponseDtoList = _mapper.Map<List<AttempResponseDto>>(AttempList);

                return new ServiceResponse<List<AttempResponseDto>>()
                {
                    Data = AttempListResponseDtoList,
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Success"
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<AttempResponseDto>>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = ex.Message
                };
            }


        }

        public ServiceResponse<AttempResponseDto> GetById(int id)
        {
            Attemp attempEntity = _attempRepository.GetById(id);


            // if guard
            if (attempEntity == null) return new ServiceResponse<AttempResponseDto>()
            {
                StatusCode = HttpStatusCode.NotFound,
                Message = "Attemp not found"
            };

            AttempResponseDto attempReadDto = _mapper.Map<AttempResponseDto>(attempEntity);
            var serviceResponse = new ServiceResponse<AttempResponseDto>();
            serviceResponse.Data = attempReadDto;
            serviceResponse.Success = true;
            return serviceResponse;
        }

        public ServiceResponse<AttempResponseDto> RemoveAttemp(int AttempId)
        {
            if (!_attempRepository.Exist(AttempId))
            {
                return new ServiceResponse<AttempResponseDto>()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Attemp not found"
                };
            }

            try
            {
                Attemp attempToDelete = _attempRepository.GetById(AttempId);
                bool isSuceeded = _attempRepository.RemoveAttemp(attempToDelete);
                if (!isSuceeded)
                {
                    return new ServiceResponse<AttempResponseDto>()
                    {
                        Message = "Nothing change",
                        StatusCode = HttpStatusCode.OK,
                        Success = true
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResponse<AttempResponseDto>()
                {
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Success = true
                };
            }

            var serviceResponse = new ServiceResponse<AttempResponseDto>()
            {
                Success = true,
                Message = "Delete Successfully",
                StatusCode = HttpStatusCode.OK
            };
            return serviceResponse;
        }
    }
}