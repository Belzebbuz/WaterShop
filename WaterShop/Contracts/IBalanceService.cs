namespace WaterShop;

public interface IBalanceService
{
    public int CurrentValue { get; }
    public void Add(int value);
    public void Remove(int value);
    public void Clear();
	List<int> GetChange();
}
