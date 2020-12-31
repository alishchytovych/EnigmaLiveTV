using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EnigmaLiveTV.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CatchController : ControllerBase
	{
		private readonly ILogger<CatchController> _logger;
		private readonly IHRProxy _proxy;
		private readonly IXMLTVConverter _converter;

		public CatchController(ILogger<CatchController> logger, IHRProxy proxy, IXMLTVConverter converter)
		{
			_logger = logger;
			_proxy = proxy;
			_converter = converter;
		}

		[HttpGet]
		[Route("/epg.xml")]
		public async Task<IActionResult> Get()
		{
			var channels = await _proxy.GetChannelsAsync();
			var ret = await _converter.ConvertChannelsAsync(channels);
			return new ContentResult {
				Content = ret,
				ContentType = "application/xml",
				StatusCode = (int)HttpStatusCode.OK
			};
		}
		
		[Route("/{**catchAll}")]
		public async Task<IActionResult> CatchAll(string catchAll)
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
