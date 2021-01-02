using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EnigmaLiveTV {
	public class STBClient : ISTBClient {
		private readonly ILogger<STBClient> _logger;
		private readonly HttpClient _client;
		private readonly IOptions<Settings> _settings;

		public STBClient(ILogger<STBClient> logger, HttpClient client, IOptions<Settings> settings) {
			_logger = logger;
			_client = client;
			_settings = settings;

			_client.BaseAddress = new Uri(_settings.Value.STB);
		}

		public async Task<STBDeviceInfo> GetDeviceInfoAsync() {
			try {
				var response = await _client.GetAsync("/api/deviceinfo");
				response.EnsureSuccessStatusCode();
				var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
				var content = await response.Content.ReadAsStringAsync();
				var ret = JsonSerializer.Deserialize<STBDeviceInfo>(content, options);
				_logger.LogDebug("/api/deviceinfo returned: " + content);
				return ret;
			} catch (Exception ex) {
				_logger.LogError("Can't read device info: " + ex.Message);
				throw;
			}
		}

		public async Task<STBEPG> GetEPGAsync() {
			try {
				var response = await _client.GetAsync("/api/epgxmltv" + "?" + _settings.Value.Filter);
				response.EnsureSuccessStatusCode();
				var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
				var content = await response.Content.ReadAsStringAsync();
				var ret = JsonSerializer.Deserialize<STBEPG>(content, options);
				_logger.LogDebug("/api/epgxmltv returned: " + content);
				return ret;
			} catch (Exception ex) {
				_logger.LogError("Can't read epg: " + ex.Message);
				throw;
			}
		}

		public async Task<(MediaTypeHeaderValue, string)> GetAnyURL(string path) {
			try {
			var response = await _client.GetAsync("/" + path);
			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync();
			var type = response.Content.Headers.ContentType;
			return (type, content);
			} catch (Exception ex) {
				_logger.LogError($"Can't read {path}: " + ex.Message);
				throw;
			}
		}
	}
}