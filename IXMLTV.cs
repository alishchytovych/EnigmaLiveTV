using System.Threading.Tasks;

namespace EnigmaLiveTV {
	public interface IXMLTV {
		public Task<string> GetXMLTVAsync(STBEPG epg);
	}
}