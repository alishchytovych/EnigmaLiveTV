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
	public class CatchController : ControllerBase
	{
		private readonly ILogger<CatchController> _logger;
		private readonly IHRProxy _proxy;
		private readonly IEPGProxy _epg;
		private readonly IXMLTVConverter _converter;

		public CatchController(ILogger<CatchController> logger, IHRProxy proxy, IEPGProxy epg, IXMLTVConverter converter)
		{
			_logger = logger;
			_proxy = proxy;
			_epg = epg;
			_converter = converter;
		}

		[HttpGet]
		[Route("/epg.xml")]
		public async Task<IActionResult> GetEPG()
		{
			var channels = await _proxy.GetChannelsAsync();
			var programmes = await _epg.GetEPGAsync();
			var ret = await _converter.ConvertChannelsAsync(channels, programmes);
			return new ContentResult {
				Content = ret,
				ContentType = "application/xml",
				StatusCode = (int)HttpStatusCode.OK
			};
		}

		[HttpGet]
		[Route("/lineup.json")]
		public async Task<IActionResult> GetLineup()
		{
			var channels = await _proxy.GetChannelsAsync();
			return new ContentResult {
				Content = JsonSerializer.Serialize(channels),
				ContentType = "application/json",
				StatusCode = (int)HttpStatusCode.OK
			};
		}

		[HttpGet]
		[Route("/discover.json")]
		public async Task<IActionResult> GetDiscover()
		{
			var channels = await _proxy.GetDiscoverAsync();
			return new ContentResult {
				Content = JsonSerializer.Serialize(channels),
				ContentType = "application/json",
				StatusCode = (int)HttpStatusCode.OK
			};
		}


		[Route("/{**catchAll}")]
		public async Task<IActionResult> UnknownRoute(string catchAll)
		{
			var ret = await _proxy.GetArbitraryFileAsync(catchAll);
			return new ContentResult {
				Content = ret.Item2,
				ContentType = ret.Item1.ToString(),
				StatusCode = (int)HttpStatusCode.OK
			};

		}
	}
}
