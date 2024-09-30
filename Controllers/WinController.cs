using BE_CodeTest.BL;
using BE_CodeTest.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BE_CodeTest.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class WinController : ControllerBase
	{
		private readonly ICasinoService _casinoService;

		public WinController(ICasinoService casinoService) => _casinoService = casinoService;

		[HttpPost]
		public async Task<TransactionResponse> Post([FromBody] WinRequest request)
		{
			return await _casinoService.Win(request.PlayerId, request.Game, request.TransactionId, request.RoundId, new Money(request.Currency, request.Amount));
		}
	}
}