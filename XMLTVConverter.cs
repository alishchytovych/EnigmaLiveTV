using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnigmaLiveTV {
	public class XMLTVConverter : IXMLTVConverter {
		public async Task<string> ConvertChannelsAsync(List<TVChannel> channels, List<TVProgramme> programmes) {
			var ret = new StringBuilder();

			ret.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
			ret.Append("<!DOCTYPE tv SYSTEM \"xmltv.dtd\">\n");
			ret.Append("<tv source-info-url=\"http://stb1.sors.local/\" source-info-name=\"VU+ box\" generator-info-name=\"EnigmaLiveTV\" generator-info-url=\"http://google.com\">\n");
			foreach (var item in channels) {
				ret.Append($"<channel id=\"{item.GuideNumber}\">\n");
				ret.Append($"<display-name>{item.GuideName}</display-name>\n");
				ret.Append($"<icon src=\"{item.Picon}\" />\n");
				ret.Append("</channel>\n");
			}
			foreach (var item in programmes) {
				DateTime start = item.BeginTimestamp != null ? ((new DateTime(1970, 1, 1, 0, 0, 0, 0)).AddSeconds((double)(item.BeginTimestamp))) : DateTime.Now;
				DateTime stop = start.AddSeconds((double)(item.DurationSec != null ? item.DurationSec : 60 * 60 * 24 * 7));
				var channel = channels.Single(x => x.SRef == item.SRef);

				if (!channel.External) {
					ret.Append($"<programme start=\"{start.ToString("yyyyMMddHHmmss")}\" stop=\"{stop.ToString("yyyyMMddHHmmss")}\" channel=\"{channel.GuideNumber}\">\n");
					ret.Append($"<title lang=\"en\">{item.Title}</title>\n");
					ret.Append($"<desc lang=\"en\">{ (item.LongDesc != null ? item.LongDesc : item.ShortDesc) }</desc>\n");
					ret.Append("</programme>\n");
				}
			}

			var cctvs = channels.Where(x => x.External);
			foreach (var item in cctvs) {
				DateTime start = DateTime.UtcNow;
				var channel = channels.Single(x => x.SRef == item.SRef);
				for (int i = 0; i < 24 * 7; i++) {
					var stop = start.AddHours(1);
					ret.Append($"<programme start=\"{start.ToString("yyyyMMddHHmmss")}\" stop=\"{stop.ToString("yyyyMMddHHmmss")}\" channel=\"{channel.GuideNumber}\">\n");
					ret.Append($"<title lang=\"en\">CCTV</title>\n");
					ret.Append($"<desc lang=\"en\">CCTV</desc>\n");
					ret.Append("</programme>\n");
					start = stop;
				}
			}
			ret.Append("</tv>");
			return ret.ToString();

		}
	}
}
//   <programme start="20080715040000 -0600" stop="20080715043000 -0600" channel="I10436.labs.zap2it.com">
//     <title lang="en">BBC World News</title>
//     <desc lang="en">International issues.</desc>
//     <category lang="en">News</category>
//     <category lang="en">Series</category>
//     <episode-num system="dd_progid">SH00315789.0000</episode-num>
//     <previously-shown />
//     <subtitles type="teletext" />
//   </programme>