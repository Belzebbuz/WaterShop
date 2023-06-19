namespace WaterShop;

public class WaterFormData
{
	public IFormFile? Image { get; set; }
	public string Title { get; set; } = default!;
	public int Cost { get; set; } = default!;
}

