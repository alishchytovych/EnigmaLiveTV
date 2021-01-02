using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EnigmaLiveTV {
	public class EPGProxy : IEPGProxy {
		private readonly ILogger<EPGProxy> _logger;
		private readonly HttpClient _client;
		private readonly IConfiguration _config;

		public EPGProxy(ILogger<EPGProxy> logger, HttpClient client, IConfiguration config) {
			_logger = logger;
			_client = client;
			_config = config;
		}
		public async Task<List<TVProgramme>> GetEPGAsync() {
			var response = await _client.GetAsync("");
			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync();
			var type = response.Content.Headers.ContentType;
			var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
			var epgNowNext = JsonSerializer.Deserialize<EPGNowNext>(await response.Content.ReadAsStringAsync(), options);
			return epgNowNext.Events;
		}
	}
}