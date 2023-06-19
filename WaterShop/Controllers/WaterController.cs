using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using WaterShop.ConfigOptions;
using WaterShop.Contracts;

namespace WaterShop;

[Route("api/water")]
[ApiController]
public class WaterController : ControllerBase
{
	private readonly IWaterService _waterService;
	private readonly SaveImageOptions _imageOptions;
	private readonly IBalanceService _balanceService;
	private readonly string _envPath;
	public WaterController(
						IWaterService waterService,
						IWebHostEnvironment env,
						SaveImageOptions imageOptions, 
						IBalanceService balanceService)
	{
		_waterService = waterService;
		_imageOptions = imageOptions;
		_balanceService = balanceService;
		_envPath = env.WebRootPath;
	}
	[HttpGet]
	public async Task<IActionResult> GetAsync()
	{
		var waters = await _waterService.GetAllAsync();
		return Ok(new { Waters = waters, Balance = _balanceService.CurrentValue });
	}

	[HttpGet("{id:guid}")]
	public async Task<IActionResult> GetAsync(Guid id)
	{
		var water = await _waterService.GetAsync(id);
		if(water == null)
			return NotFound(id.ToString());
		return Ok(new { Water = water, Balance = _balanceService.CurrentValue });
	}

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> DeleteAsync(Guid id)
	{
		await _waterService.DeleteAsync(id);
		return Ok();
	}

	[HttpPost]
	public async Task<IActionResult> CreateAsync([FromForm] WaterFormData data)
	{
		if (data.Image == null)
			return BadRequest("Image is required field");
		var id = await _waterService.CreateAsync(new(data.Title, data.Cost));
		var path = await SaveImageAsync(data.Image, id);
		await _waterService.SetImagePathAsync(id, path);
		return Ok();
	}

	[HttpPut("{id:guid}")]
	public async Task<IActionResult> UpdateAsync(Guid id, [FromForm] WaterFormData data)
	{
		if (id == Guid.Empty)
			return BadRequest("Id was empty");
		await _waterService.UpdateAsync(new(id, data.Title, data.Cost));
		if(data.Image != null)
		{
			var path = await SaveImageAsync(data.Image, id);
			await _waterService.SetImagePathAsync(id, path);
		}
		return Ok();
	}

	[HttpPost("add-count")]
	public async Task<IActionResult> AddCountAsync(AddCountCommand command)
	{
		await _waterService.AddCountAsync(command);
		return Ok();
	}

	[HttpPost("remove-count")]
	public async Task<IActionResult> RemoveCountAsync(RemoveCountCommand command)
	{
		await _waterService.RemoveCountAsync(command);
		return Ok();
	}

	[HttpGet("{id:guid}/buy")]
	public async Task<IActionResult> BuyAsync(Guid id)
	{
		var water = await _waterService.GetAsync(id);
		if (water == null)
			return NotFound(id.ToString());

		await _waterService.RemoveCountAsync(new(id, 1));
		_balanceService.Remove(water.Cost);

		return Ok();
	}

	[HttpPost("import")]
	public async Task<IActionResult> ImportAsync([FromForm]IFormFile file)
	{
		if (file == null || file.FileName == null)
			return BadRequest("File or file name was null!");
		if (!string.Equals(Path.GetExtension(file.FileName), ".json", StringComparison.OrdinalIgnoreCase))
			return BadRequest("File must be .json!");
		using (var streamReader = new StreamReader(file.OpenReadStream()))
		{
			var jsonString = await streamReader.ReadToEndAsync();

			try
			{
				var options = new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				};

				var data = JsonSerializer.Deserialize<List<WaterImportDto>>(jsonString, options);
				if(data == null)
					return BadRequest("Invalid JSON format!");
				await _waterService.ImportAsync(data);
				return Ok();
			}
			catch (JsonException)
			{
				return BadRequest($"Invalid JSON format!");
			}
		}
	}

	private async Task<string> SaveImageAsync(IFormFile file, Guid id)
	{
		var folderPath = Path.Combine(_envPath, _imageOptions.ImagesPath);
		if(!Directory.Exists(folderPath))
			Directory.CreateDirectory(folderPath);
		string fileName = Path.Combine(_imageOptions.ImagesPath, id.ToString() + Path.GetExtension(file.FileName));
		string filePath = Path.Combine(_envPath, fileName);
		var fileStream = new FileStream(filePath, FileMode.Create);
		await file.CopyToAsync(fileStream);
		return fileName;
	}
}

public class WaterImportDto
{
    public string Title { get; set; }
    public int Cost { get; set; }
}
