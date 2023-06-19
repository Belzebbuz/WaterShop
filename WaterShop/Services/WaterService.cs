using Microsoft.EntityFrameworkCore;
using WaterShop.Domain;
using WaterShop.Dto;
using WaterShop.Exceptions;
using WaterShop.Persistance;

namespace WaterShop.Contracts;

public class WaterService : IWaterService
{
	private readonly AppDbContext _context;

	public WaterService(AppDbContext context)
    {
		_context = context;
	}
    public async Task AddCountAsync(AddCountCommand command)
	{
		var water = await _context.Waters.FindAsync(command.Id);
		if (water == null) 
			throw new NotFoundException(command.Id.ToString());
		water.AddCount(command.Count);
		await _context.SaveChangesAsync();
	}

	public async Task<Guid> CreateAsync(CreateWaterCommand command)
	{
		var water = Water.Create(command.Title, command.Cost);
		await _context.Waters.AddAsync(water);
		await _context.SaveChangesAsync();
		return water.Id;
	}

	public async Task DeleteAsync(Guid id)
	{
		var water = await _context.Waters.FindAsync(id);
		if (water == null)
			throw new NotFoundException(id.ToString());
		_context.Waters.Remove(water);
		await _context.SaveChangesAsync();

	}

	public async Task ImportAsync(List<WaterImportDto> exportData)
	{
		var data = new List<Water>();
		foreach (var dto in exportData)
		{
			var entity = Water.Create(dto.Title, dto.Cost);
			data.Add(entity);
		}
		await _context.Waters.AddRangeAsync(data);
		await _context.SaveChangesAsync();
	}

	public async Task<List<WaterDto>> GetAllAsync()
	{
		return await _context.Waters
			.Select(x => new WaterDto(x.Id, x.Title, x.ImagePath, x.Cost, x.Count))
			.ToListAsync();
	}

	public async Task<WaterDto> GetAsync(Guid id)
	{
		var water = await _context.Waters.FindAsync(id);
		if (water == null)
			throw new NotFoundException(id.ToString());
		return new WaterDto(water.Id, water.Title, water.ImagePath, water.Cost, water.Count);
	}

	public async Task RemoveCountAsync(RemoveCountCommand command)
	{
		var water = await _context.Waters.FindAsync(command.Id);
		if (water == null)
			throw new NotFoundException(command.Id.ToString());
		water.RemoveCount(command.Count);
		await _context.SaveChangesAsync();
	}

	public async Task SetImagePathAsync(Guid id, string path)
	{
		var water = await _context.Waters.FindAsync(id);
		if (water == null)
			throw new NotFoundException(id.ToString());
		water.SetImagePath(path);
		await _context.SaveChangesAsync();
	}

	public async Task UpdateAsync(UpdateWaterCommand command)
	{
		var water = await _context.Waters.FindAsync(command.Id);
		if (water == null)
			throw new NotFoundException(command.Id.ToString());
		water.Update(command.Title, command.Cost);
		await _context.SaveChangesAsync();
	}
}
