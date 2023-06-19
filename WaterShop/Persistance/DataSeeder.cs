using WaterShop.ConfigOptions;
using WaterShop.Contracts;
using WaterShop.Domain;

namespace WaterShop.Persistance;

public class DataSeeder
{
	private readonly AppDbContext _context;
	private readonly SaveImageOptions _options;
	private readonly string _envRootPath;

	public DataSeeder(AppDbContext context, IWebHostEnvironment env, SaveImageOptions options)
    {
		_context = context;
		_options = options;
		_envRootPath = env.WebRootPath;
	}

	public async Task SeedAsync()
	{
		if (_context.Waters.Any())
			return;

		var waterSaint = Water.Create("Святой источник 0.5", 30);
		var waterPiligrim = Water.Create("Пилигрим 0.5", 30);
		_context.Waters.Add(waterSaint);
		_context.Waters.Add(waterPiligrim);
		await _context.SaveChangesAsync();

		var fullImagesFolderPath = Path.Combine(_envRootPath, _options.ImagesPath);
		if(!Directory.Exists(fullImagesFolderPath)) 
			Directory.CreateDirectory(fullImagesFolderPath);

		var fileSaintName = Path.Combine(_options.ImagesPath, waterSaint.Id.ToString() + ".jpg");
		var fileSaintPath = Path.Combine(_envRootPath, fileSaintName);
		using var saintTestImage = File.OpenRead(Path.Combine("TestData", "saint.jpg"));
		using var saintImage = File.OpenWrite(fileSaintPath);
		await saintTestImage.CopyToAsync(saintImage);
		waterSaint.SetImagePath(fileSaintName);

		var filePiligrimName = Path.Combine(_options.ImagesPath, waterPiligrim.Id.ToString() + ".png");
		var filePiligrimPath = Path.Combine(_envRootPath, filePiligrimName);
		using var piligrimTestImage = File.OpenRead(Path.Combine("TestData", "piligrim.png"));
		using var piligrimImage = File.OpenWrite(filePiligrimPath);
		await piligrimTestImage.CopyToAsync(piligrimImage);
		waterPiligrim.SetImagePath(filePiligrimName);
		waterPiligrim.AddCount(10);
		await _context.SaveChangesAsync();
	}
}
