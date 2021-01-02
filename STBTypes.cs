using System.Text.Json.Serialization;

// https://github.com/E2OpenPlugins/e2openplugin-OpenWebif/wiki/OpenWebif-API-documentation
namespace EnigmaLiveTV {
	public class STBService {
		public string ServiceReference {get;set;}
		public int Program {get;set;}
		public string ServiceName {get;set;}
		[JsonPropertyName("pos")]
		public int Position {get;set;}

		[JsonIgnore]
		public bool hasProgramme {get;set;} = false;
	}

	public class STBEvent {
		[JsonPropertyName("sname")]
		public string ServiceName {get;set;}
		public string Title {get;set;}
		[JsonPropertyName("begin_timestamp")]
		public int BeginTimestamp {get;set;}
		[JsonPropertyName("now_timestamp")]
		public int NowTimestamp {get;set;}
		[JsonPropertyName("sref")]
		public string ServiceReference {get;set;}
		public int Id {get;set;}
		public string Genre {get;set;}
		public int GenreId {get;set;}
		[JsonPropertyName("duration_sec")]
		public int DurationSec {get;set;}
		[JsonPropertyName("shortdesc")]
		public string ShortDescription {get;set;}
		[JsonPropertyName("longdesc")]
		public string LongDescription {get;set;}

	}

	public class STBOffset {
		public string UTCOffset { get; set; }
	}

	public class STBTuner {
		public string Name {get;set;}
		public string Type {get;set;}
	}

	public class STBInterface {
		[JsonPropertyName("gw")]
		public string Gateway {get;set;}
		public string MAC {get;set;}
		public string Name {get;set;}
		public string FriendlyName {get;set;}
		public string IP {get;set;}
		public bool DHCP {get;set;}
		public string IPv4Method {get;set;}
		[JsonPropertyName("v4prefix")]
		public int NetPrefix {get;set;}
		public string IPv6 {get;set;}
		[JsonPropertyName("mask")]
		public string NetMask {get;set;}
	}

	public class STBHDD {
		public string Model {get;set;}
		public string Capacity {get;set;}
		[JsonPropertyName("labelled_capacity")]
		public string LabelledCapacity {get;set;}
		public string Free {get;set;}
	}

	public class STBDeviceInfo {
		public string Uptime {get;set;}
		public string EnigmaVer {get;set;}
		public string DriverDate {get;set;}
		public string ImageVer {get;set;}

		public string Brand {get;set;}
		public string BoxType {get;set;}
		public string MachineBuild {get;set;}
		public string FriendlyImageDistro {get;set;}
		public string ImageDistro {get;set;}
		public string OEVer {get;set;}
		
		public string Mem1 {get;set;}
		public string Mem2 {get;set;}
		public string Chipset {get;set;}
		public string Model {get;set;}
		public bool Transcoding  {get;set;}
		public string WebIfVer {get;set;}
		public string KernelVer {get;set;}

		[JsonPropertyName("ifaces")]
		public STBInterface[] Interfaces {get;set;}

		public STBTuner[] Tuners {get;set;}

		public STBHDD[] HDD {get;set;}

	}

	public class STBEPG {
		public string Language { get; set; }
		public STBOffset Offset { get; set; }
		public STBService[] Services { get; set; }
		public STBEvent[] Events { get; set; }

	}
}