using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace EnigmaLiveTV
{
	public class HRProxy : IHRProxy
	{
		private readonly ILogger<HRProxy> _logger;
		private readonly HttpClient _client;

		public HRProxy(ILogger<HRProxy> logger, HttpClient client)
		{
			_logger = logger;
			_client = client;
		}

		public async Task<(MediaTypeHeaderValue, string)> GetArbitraryFileAsync(string path)
		{
			var response = await _client.GetAsync("/"+path);
			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync();
			var type = response.Content.Headers.ContentType;
			return (type, content);
		}

		//   [{
		//     "GuideNumber": "2",
		// 	"GuideName": "Animal Planet HD",
		// 	"URL": "http://10.0.0.215:8001/1:0:19:89A:16:70:1680000:0:0:0:"
		// 	}]

		public async Task<List<TVChannel>> GetChannelsAsync()
		{
			var response = await _client.GetAsync("/lineup.json");
			response.EnsureSuccessStatusCode();

			var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
			var ret = JsonSerializer.Deserialize<List<TVChannel>>(await response.Content.ReadAsStringAsync(), options);
			foreach (var item in ret)
			{
				var uri = new Uri(item.URL);
				item.Picon = uri.Scheme + "://" + uri.Host + "/picon" + uri.LocalPath.Substring(0, uri.LocalPath.Length - 1).Replace(':', '_') + ".png";
			}
			return ret;
		}
	}
}