using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnigmaLiveTV
{
	public interface IXMLTVConverter
	{
		public Task<string> ConvertChannelsAsync(List<TVChannel> list);
	}
}