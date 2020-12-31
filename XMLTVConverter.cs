using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EnigmaLiveTV
{
	public class XMLTVConverter : IXMLTVConverter
	{
		public async Task<string> ConvertChannelsAsync(List<TVChannel> list)
		{
			var ret = new StringBuilder();
			
			ret.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
			ret.Append("<!DOCTYPE tv SYSTEM \"xmltv.dtd\">\n");
			ret.Append("<tv source-info-url=\"http://stb1.sors.local/\" source-info-name=\"VU+ box\" generator-info-name=\"EnigmaLiveTV\" generator-info-url=\"http://google.com\">\n");
			foreach(var item in list) {
				ret.Append($"<channel id=\"{item.GuideNumber}\">\n");
				ret.Append($"<display-name>{item.GuideName}</display-name>\n");
				ret.Append($"<icon src=\"{item.Picon}\" />\n");
				ret.Append("</channel>\n");
			}
			ret.Append("</tv>");
			return ret.ToString();

		}
	}
}