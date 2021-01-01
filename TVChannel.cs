using System.Text.Json.Serialization;

namespace EnigmaLiveTV
{
	public class TVChannel
	 {
		public string GuideNumber { get; set; }
		public string GuideName { get; set; }
		public string URL { get; set; }
		[JsonIgnore]
		public string Picon { get; set; }
		[JsonIgnore]
		public string SRef { get; set; }
		[JsonIgnore]
		public bool External { get; set; } = false;
	}
}