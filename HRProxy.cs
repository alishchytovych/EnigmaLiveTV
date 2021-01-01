using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EnigmaLiveTV
{
	public class HRProxy : IHRProxy
	{
		private readonly ILogger<HRProxy> _logger;
		private readonly HttpClient _client;
		private readonly IConfiguration _config;

		public HRProxy(ILogger<HRProxy> logger, HttpClient client, IConfiguration config)
		{
			_logger = logger;
			_client = client;
			_config = config;
		}

		public async Task<(MediaTypeHeaderValue, string)> GetArbitraryFileAsync(string path)
		{
			var response = await _client.GetAsync("/" + path);
			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync();
			var type = response.Content.Headers.ContentType;
			return (type, content);
		}

		public async Task<Discover> GetDiscoverAsync() {
			var response = await _client.GetAsync("/discover.json");
			response.EnsureSuccessStatusCode();
			var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
			var discover = JsonSerializer.Deserialize<Discover>(await response.Content.ReadAsStringAsync(), options);
			discover.BaseURL = _config.GetValue<string>("Kestrel:EnigmaLiveTV:ExternalUrl");
			discover.LineupURL = discover.BaseURL + "/lineup.json";
			return discover;
		}

		public async Task<List<TVChannel>> GetChannelsAsync()
		{
			var ret = new List<TVChannel>();
			var cctvs = _config.GetSection("CCTVChannels").Get<List<TVChannel>>();
			int i = 1;

			foreach(var item in cctvs)  
			{
				item.GuideNumber = (i++).ToString();
				item.Picon = _client.BaseAddress.Scheme + "://" + _client.BaseAddress.Host + "/picon/" + item.Picon;
				item.SRef = "ExternalChannel"+item.GuideName;
				item.External = true;
				ret.Add(item);
			}

			var response = await _client.GetAsync("/lineup.json");
			response.EnsureSuccessStatusCode();
			var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
			var channels = JsonSerializer.Deserialize<List<TVChannel>>(await response.Content.ReadAsStringAsync(), options);

			foreach (var item in channels)
			{
				item.GuideNumber = (i++).ToString();
				var uri = new Uri(item.URL);
				item.SRef = uri.LocalPath.Substring(1, uri.LocalPath.Length - 2);
				item.Picon = uri.Scheme + "://" + uri.Host + "/picon/" + item.SRef.Replace(':', '_') + ".png";
				item.SRef+=":";
				ret.Add(item);
			}
			return ret;
		}
	}
}