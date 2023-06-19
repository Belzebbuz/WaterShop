using Microsoft.AspNetCore.Mvc;

namespace WaterShop;

[Route("api/balance")]
[ApiController]
public class BalanceController : ControllerBase
{
	private readonly IBalanceService _balanceService;
	public BalanceController(IBalanceService balanceService) => _balanceService = balanceService;
	[HttpGet]
	public IActionResult Get()
	{
		return Ok(_balanceService.CurrentValue);
	}

	[HttpPost]
	public IActionResult AddValue([FromBody] int value)
	{
		_balanceService.Add(value);
		return Ok(_balanceService.CurrentValue);
	}

	[HttpGet("change")]
	public IActionResult GiveBackChange()
	{
		return Ok(new { Monets = _balanceService.GetChange(), Balance = _balanceService.CurrentValue }) ;
	}
}
