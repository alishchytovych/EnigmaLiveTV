using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace EnigmaLiveTV
{
	public interface IHRProxy
	{
		public Task<List<TVChannel>> GetChannelsAsync();
		public Task<(MediaTypeHeaderValue, string)> GetArbitraryFileAsync(string path);
	}
}