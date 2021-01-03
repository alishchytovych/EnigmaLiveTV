using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EnigmaLiveTV.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class FileController : ControllerBase
	{
		private readonly ILogger<FileController> _logger;
		private readonly ISTBClient _stb;
		private readonly IHomerun _homerun;
		private readonly IXMLTV _xmltv;

		public FileController(ILogger<FileController> logger, ISTBClient stb, IHomerun homerun, IXMLTV xmltv)
		{
			_logger = logger;
			_stb = stb;
			_homerun = homerun;
			_xmltv = xmltv;
		}

		[HttpGet]
		[Route("/xmltv")]
		public async Task<IActionResult> GetXMLTV()
		{
			_logger.LogInformation("xmltv requested");

			var epg = await _stb.GetEPGAsync();
			var xmltv = await _xmltv.GetXMLTVAsync(epg);

			return new ContentResult {
				Content = xmltv,
				ContentType = "application/xml",
				StatusCode = (int)HttpStatusCode.OK
			};
		}

		[HttpGet]
		[Route("/lineup.json")]
		public async Task<IActionResult> GetLineup()
		{
			_logger.LogInformation("lineup.json requested");
			var epg = await _stb.GetEPGAsync();
			var lineup = await _homerun.GetLineupAsync(epg);

			return new ContentResult {
				Content = JsonSerializer.Serialize(lineup),
				ContentType = "application/json",
				StatusCode = (int)HttpStatusCode.OK
			};
		}

		[HttpGet]
		[Route("/lineup_status.json")]
		public async Task<IActionResult> GetLineupStatus()
		{
			_logger.LogInformation("lineup_status.json requested");
			var lineupStatus = await _homerun.GetLineupStatusAsync();

			return new ContentResult {
				Content = JsonSerializer.Serialize(lineupStatus),
				ContentType = "application/json",
				StatusCode = (int)HttpStatusCode.OK
			};
		}


		[HttpGet]
		[Route("/discover.json")]
		public async Task<IActionResult> GetDiscover()
		{
			_logger.LogInformation("discover.json requested");
			var deviceinfo = await _stb.GetDeviceInfoAsync();
			var discover = await _homerun.GetDiscoverAsync(deviceinfo);

			return new ContentResult {
				Content = JsonSerializer.Serialize(discover),
				ContentType = "application/json",
				StatusCode = (int)HttpStatusCode.OK
			};
		}


		[Route("/{**catchAll}")]
		public async Task<IActionResult> UnknownRoute(string catchAll)
		{
			_logger.LogInformation("Unknown API requested: "+catchAll);
			var ret = await _stb.GetAnyURL(catchAll);
			return new ContentResult {
				Content = ret.Item2,
				ContentType = ret.Item1.ToString(),
				StatusCode = (int)HttpStatusCode.OK
			};

		}
	}
}
