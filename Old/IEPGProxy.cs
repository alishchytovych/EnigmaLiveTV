using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace EnigmaLiveTV {
	public interface IEPGProxy {
		public Task<List<TVProgramme>> GetEPGAsync();
	}
}