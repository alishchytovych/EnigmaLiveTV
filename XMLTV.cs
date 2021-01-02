using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EnigmaLiveTV {
	public class XMLTV : IXMLTV {
		private readonly ILogger<XMLTV> _logger;
		private readonly IOptions<Settings> _settings;

		public XMLTV(ILogger<XMLTV> logger, IOptions<Settings> settings) {
			_logger = logger;
			_settings = settings;
		}

		public async Task<string> GetXMLTVAsync(STBEPG epg) {
			var ret = new StringBuilder();
			int offset = 0;
			var time = (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

			ret.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
			ret.Append("<!DOCTYPE tv SYSTEM \"xmltv.dtd\">\n");
			ret.Append("<tv source-info-url=\"https://github.com/E2OpenPlugins/e2openplugin-OpenWebif\" source-info-name=\"OpenWebif\" generator-info-name=\"EnigmaLiveTV\" generator-info-url=\"https://github.com/alishchytovych/EnigmaLiveTV\">\n");

			foreach (var item in _settings.Value.AdditionalChannels) {
				ret.Append($"<channel id=\"{++offset}\">\n");
				ret.Append($"<display-name>{item.Name}</display-name>\n");
				try {
					var test = new Uri(item.Icon);
					ret.Append($"<icon src=\"{item.Icon}\" />\n");
				} catch {
					ret.Append($"<icon src=\"{_settings.Value.STB}/picon/{item.Icon}\" />\n");
				}
				ret.Append("</channel>\n");


				for (int i = 0; i < 7 * 24; i++) {
					var prog = new STBEvent {
						BeginTimestamp = (int)time + i * 60 * 60,
						DurationSec = 60 * 60,
						Title = item.Name,
						ShortDescription = "",
						LongDescription = "",
						GenreId = 0,
						Genre = "Other"
					};
					ret.Append(MakeProgramme(prog, offset));
				}
			}
			foreach (var item in epg.Services) {
				ret.Append($"<channel id=\"{item.Position + offset}\">\n");
				ret.Append($"<display-name>{item.ServiceName}</display-name>\n");
				ret.Append($"<icon src=\"{_settings.Value.STB}/picon/{item.ServiceReference.Substring(0, item.ServiceReference.Length - 1).Replace(':', '_')}.png\" />\n");
				ret.Append("</channel>\n");
			}

			foreach (var item in epg.Events) {
				var channel = epg.Services.Single(x => x.ServiceReference == item.ServiceReference);
				channel.hasProgramme = true;
				ret.Append(MakeProgramme(item, channel.Position + offset));
			}

			// fix for the channel without programmes
			var missed = epg.Services.Where(x => x.hasProgramme == false);
			foreach (var item in missed) {
				for (int i = 0; i < 7 * 24; i++) {
					var prog = new STBEvent {
						BeginTimestamp = (int)time + i * 60 * 60,
						DurationSec = 60 * 60,
						Title = item.ServiceName,
						ShortDescription = "",
						LongDescription = "",
						GenreId = 0,
						Genre = "Other"
					};
					ret.Append(MakeProgramme(prog, item.Position + offset));
				}
			}

			ret.Append("</tv>");
			return ret.ToString();
		}

		private string MakeProgramme(STBEvent item, int id) {
			var ret = new StringBuilder();

			DateTime start = (new DateTime(1970, 1, 1, 0, 0, 0, 0)).AddSeconds(item.BeginTimestamp);
			DateTime stop = start.AddSeconds(item.DurationSec);

			ret.Append($"<programme start=\"{start.ToString("yyyyMMddHHmmss")}\" stop=\"{stop.ToString("yyyyMMddHHmmss")}\" channel=\"{id}\">\n");
			ret.Append($"<title lang=\"en\">{item.Title}</title>\n");
			ret.Append($"<sub-title lang=\"en\">{item.ShortDescription}</sub-title>\n");
			ret.Append($"<desc lang=\"en\">{item.LongDescription}</desc>\n");
			ret.Append($"<category lang=\"en\" id=\"{item.GenreId}\">{item.Genre}</category>\n");
			ret.Append("</programme>\n");

			return ret.ToString();
		}
	}
}