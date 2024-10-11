using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace YourNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly string API_KEY = "621f21e9-d05f-45df-b1ac-f7db7e0699b0";

        [HttpGet("{playerName}")]
        public async Task<IActionResult> GetPlayerInfo2(string playerName)
        {
            if (string.IsNullOrEmpty(playerName))
            {
                return BadRequest("Veuillez entrer un pseudo de joueur.");
            }

            try
            {
                string uuid = await GetUUIDFromPlayerName(playerName);
                string playerInfo = await GetPlayerInfo(uuid);
                string friendsList = await GetPlayerFriendList(uuid);
                string playerJob = await GetPlayerJobs(uuid);
                string playerClicker = await GetPlayerClicker(uuid);

                var result = new
                {
                    playerInfo,
                    friendsList,
                    playerJob,
                    playerClicker
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur : {ex.Message}");
            }
        }

        private async Task<string> GetUUIDFromPlayerName(string playerName)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {API_KEY}");
                string API_URL = $"https://api.paladium.games/v1/paladium/player/profile/{playerName}";

                HttpResponseMessage response = await client.GetAsync(API_URL);
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var jsonDoc = JObject.Parse(jsonResponse);
                    return jsonDoc["uuid"].ToString();
                }
                else
                {
                    throw new Exception($"Erreur : {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
        }

        private async Task<string> GetPlayerInfo(string uuid)
        {
            return await CallApi($"https://api.paladium.games/v1/paladium/player/profile/{uuid}");
        }

        private async Task<string> GetPlayerFriendList(string uuid)
        {
            return await CallApi($"https://api.paladium.games/v1/paladium/player/profile/{uuid}/friends");
        }

        private async Task<string> GetPlayerJobs(string uuid)
        {
            return await CallApi($"https://api.paladium.games/v1/paladium/player/profile/{uuid}/jobs");
        }

        private async Task<string> GetPlayerClicker(string uuid)
        {
            return await CallApi($"https://api.paladium.games/v1/paladium/player/profile/{uuid}/clicker");
        }

        private async Task<string> CallApi(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {API_KEY}");
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    throw new Exception($"Erreur : {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
        }
    }
}
