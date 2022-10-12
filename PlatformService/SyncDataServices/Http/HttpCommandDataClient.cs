using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using PlatformService.Dtos;

namespace PlatformService.SyncDataServices.Http
{
    public class HttpCommandDataClient : ICommandDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpCommandDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            this._configuration = configuration;
            this._httpClient = httpClient;
        }
        public async Task SendPlatformToCommand(PlatformReadDto platform)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(platform),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync($"{_configuration["CommandService"]}/api/Command/Platform", httpContent );
            if(response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync Post to Command Service was OK!");
            }
            else
            {
                Console.WriteLine("--> Sync Post to Command Service was NOT OK!");
            }
        }
    }
}