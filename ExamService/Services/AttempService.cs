using System.Net;
using AutoMapper;
using ExamService.Contracts.RepositoryContracts;
using ExamService.Contracts.ServiceContracts;
using ExamService.Dtos;
using ExamService.Models;
using ExamService.Response;

namespace ExamService.Services
{
    public class AttempService : IAttempService
    {
        private readonly IAttempRepository _attempRepository;
        private readonly IUserRepository _userRepository;
        private readonly IExamRepository examRepository;
        private readonly IMapper _mapper;

        public AttempService(IAttempRepository attempRepository, IUserRepository userRepository, IExamRepository examRepository, IMapper mapper)
        {
            this._attempRepository = attempRepository;
            this._userRepository = userRepository;
            this.examRepository = examRepository;
            this._mapper = mapper;
        }
        public ServiceResponse<AttempResponseDto> AddAttemp(AttempRequestDto AttempRequestDto)
        {
            
            Attemp NewAttemp = _mapper.Map<Attemp>(AttempRequestDto);
            throw new NotImplementedException();
        }

        public ServiceResponse<AttempResponseDto> AddAttemps(List<AttempRequestDto> AttempRequestDto)
        {
           try{
             List<Attemp> AttempList = new List<Attemp>();
            foreach(var AttempDto in AttempRequestDto)
            {
                Exam exam = examRepository.GetById(AttempDto.ExamId);

                User user = _userRepository.GetByEmail(AttempDto.Email);
                
                Attemp NewAttemp = new Attemp(){
                    TotalScore = AttempDto.TotalScore,
                    Exam = exam,
                    User = user
                };
                AttempList.Add(NewAttemp);
            }
            _attempRepository.AddAttemps(AttempList);
           }catch(Exception ex){
                return new ServiceResponse<AttempResponseDto>(){
                    Data = null,
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError
                };
           }
            return  new ServiceResponse<AttempResponseDto>(){
                    Data = null,
                    Success = true,
                    Message = "Save attemps successfully",
                    StatusCode = HttpStatusCode.Created
                };;
        }

        public bool Exist(int id)
        {
            throw new NotImplementedException();
        }

        public ServiceResponse<List<AttempResponseDto>> GetAttemps()
        {
            List<Attemp> attempList = _attempRepository.GetAllAttemps();
            List<AttempResponseDto> attempReadDtoList = _mapper.Map<List<AttempResponseDto>>(attempList);
            var serviceResponse = new ServiceResponse<List<AttempResponseDto>>();
            serviceResponse.Success = true;
            serviceResponse.Data = attempReadDtoList;
            return serviceResponse;
        }

        public ServiceResponse<AttempResponseDto> GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}