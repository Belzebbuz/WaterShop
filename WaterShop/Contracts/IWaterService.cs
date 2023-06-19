using WaterShop.Dto;

namespace WaterShop.Contracts;

public interface IWaterService
{
	public Task<List<WaterDto>> GetAllAsync();
	public Task<WaterDto> GetAsync(Guid id);
	public Task<Guid> CreateAsync(CreateWaterCommand command);
	public Task UpdateAsync(UpdateWaterCommand command);
	public Task AddCountAsync(AddCountCommand command);
	public Task RemoveCountAsync(RemoveCountCommand command);
	public Task DeleteAsync(Guid id);
	public Task SetImagePathAsync(Guid id, string path);
	public Task ImportAsync(List<WaterImportDto> exportData);
}

public record CreateWaterCommand(string Title, int Cost);
public record UpdateWaterCommand(Guid Id, string Title, int Cost);
public record RemoveCountCommand(Guid Id, int Count);
public record AddCountCommand(Guid Id, int Count);
