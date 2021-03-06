namespace EnigmaLiveTV {
	public class Discover {
		public int TunerCount { get; set; }
		public string DeviceAuth { get; set; }
		public string DeviceUUID { get; set; }
		public string BaseURL { get; set; }
		public string ModelNumber { get; set; }
		public string DeviceID { get; set; }
		public string FriendlyName { get; set; }
		public string LineupURL { get; set; }
		public string FirmwareName { get; set; }
		public string FirmwareVersion { get; set; }
		public string Manufacturer { get; set; }
	}

	public class Lineup {
		public string GuideNumber { get; set; }
		public string GuideName { get; set; }
		public string URL { get; set; }
	}

	public class LineupStatus {
		public int ScanInProgress {get;set;} = 0;
		public int ScanPossible {get;set;} = 1;
		public string Source {get;set;} = "Antenna";
		public string[] SourceList {get;set;} = { "Antenna" };
	}

}