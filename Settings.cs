namespace EnigmaLiveTV {

	public class Channel {
		public string Name { get; set; }
		public string URL { get; set; }
		public string Icon { get; set; }
	}

	public class Settings {
		public string STB { get; set; }
		public string StreamingPort { get; set; }
		public string Filter { get; set; }
		public string ServiceAddress {get;set;}
		public Channel[] AdditionalChannels { get; set; }
	}
}