using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace EnigmaLiveTV {
	public class Homerun : IHomerun {

		private readonly ILogger<Homerun> _logger;
		private readonly IOptions<Settings> _settings;

		public Homerun(ILogger<Homerun> logger, IOptions<Settings> settings)
		{
			_logger = logger;
			_settings = settings;
		}

		public async Task<Discover> GetDiscoverAsync(STBDeviceInfo info) {
			var id = new System.GuidEx(_settings.Value.STB).ToString();
			var ret = new Discover {
				TunerCount = info.Tuners.Length,
				DeviceAuth = "no",
				DeviceUUID = id,
				BaseURL = _settings.Value.ServiceAddress, //_settings.Value.STB,
				ModelNumber = info.Model,
				DeviceID = id.Substring(id.Length-8),
				FriendlyName = info.BoxType,
				LineupURL = _settings.Value.ServiceAddress+"/lineup.json",
				FirmwareName = info.FriendlyImageDistro,
				FirmwareVersion = info.ImageVer,
				Manufacturer = info.Brand
			};
			_logger.LogDebug($"Object discover: {ret}");
			return ret;
		}

		public async Task<List<Lineup>> GetLineupAsync(STBEPG epg) {
			int offset = 0;
			var list = new List<Lineup>();
			foreach(var item in _settings.Value.AdditionalChannels) {
				list.Add(new Lineup {
					GuideNumber = (++offset).ToString(),
					GuideName = item.Name,
					URL = item.URL
				});
			}

			foreach(var item in epg.Services) {
				list.Add(new Lineup {
					GuideNumber = (item.Position + offset).ToString(),
					GuideName = item.ServiceName,
					URL = _settings.Value.STB+":"+_settings.Value.StreamingPort+"/"+item.ServiceReference
				});
			}
			_logger.LogDebug($"Object lineup: {list}");
			return list;
		}

		public async Task<LineupStatus> GetLineupStatusAsync() {
			return new LineupStatus();
		}

	}
}