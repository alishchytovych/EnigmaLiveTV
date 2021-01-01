using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EnigmaLiveTV {
	public class TVProgramme {
		public string SName {get;set;}		
		public string? Title {get;set;}
		[JsonPropertyName("begin_timestamp")]
		public int? BeginTimestamp {get;set;}
		[JsonPropertyName("now_timestamp")]
		public int NowTimestamp {get;set;}
		public string SRef {get;set;}
		public int? Id {get;set;}
		public string Genre {get;set;}
		[JsonPropertyName("duration_sec")]
		public int? DurationSec {get;set;}
		public string? ShortDesc {get;set;}
		public int GenreId {get;set;}
		public string? LongDesc {get;set;}
	}

	public class EPGNowNext {
		public List<TVProgramme> Events {get;set;}
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

	//   "sname": "Amedia Premium HD",
    //   "title": "ВИКИНГИ (18+)",
    //   "begin_timestamp": 1609510800,
    //   "now_timestamp": 1609512534,
    //   "sref": "1:0:19:6593:9:70:1680000:0:0:0:",
    //   "id": 30540,
    //   "genre": "",
    //   "duration_sec": 2700,
    //   "shortdesc": " - приключения. 6-й сезон. 16-я серия.  Реж.: Киаран Доннелли, Кен Джиротти, Йохан Ренк.  В роляx: Трэвис Фиммел, Кэтрин Уинник, Клайв Стэнден, Д",
    //   "genreid": 0,
    //   "longdesc": ""