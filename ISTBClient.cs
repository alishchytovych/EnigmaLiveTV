using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace EnigmaLiveTV
{
	public interface ISTBClient {
		public Task<STBDeviceInfo> GetDeviceInfoAsync();
		public Task<STBEPG> GetEPGAsync();

		public Task<(MediaTypeHeaderValue, string)> GetAnyURL(string path);
	}
}