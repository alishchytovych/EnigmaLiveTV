using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnigmaLiveTV {
	public interface IHomerun {
		public Task<Discover> GetDiscoverAsync(STBDeviceInfo info);
		public Task<List<Lineup>> GetLineupAsync(STBEPG epg);
	}
}