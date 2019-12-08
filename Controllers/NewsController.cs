using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TurboApi.Models;
using TurboApi.Services;

namespace TurboApi.Controllers
{
	[Route("news")]
	[ApiController]
	public class NewsController : ControllerBase
	{
		private readonly DbLocalContext db;

		public NewsController(
			DbLocalContext context)
		{
			db = context;
		}
		
		[HttpGet("")]
		public async Task<IEnumerable<News>> News()
		{
			return db.News.Select(x => x);
		}
	}
}
