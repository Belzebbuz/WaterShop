namespace WaterShop.Dto;

public class WaterDto
{
	public WaterDto(Guid id, string title, string? imagePath, int cost, int count)
	{
		Id = id;
		Title = title;
		ImagePath = imagePath;
		Cost = cost;
		Count = count;
	}

	public Guid Id { get; set; }
    public string Title { get; set; }
    public string? ImagePath { get; set; }
    public int Cost { get; set; }
    public int Count { get; set; }
}
