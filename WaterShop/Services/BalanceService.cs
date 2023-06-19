namespace WaterShop;

public class BalanceService : IBalanceService
{
    private readonly List<int> _avalibleValues = new() { 1, 2, 5, 10};
    private int _currentValue;
    public int CurrentValue => _currentValue;

    public void Add(int value)
    {
        if(!_avalibleValues.Any(x => x == value))
            throw new ArgumentException("The machine only accepts coins: {0}", string.Join(" ", _avalibleValues));
        _currentValue += value;
    }

	public void Clear()
	{
		_currentValue = 0;
	}

	public void Remove(int value)
	{
		var nextValue = _currentValue - value;
        if (nextValue < 0)
            throw new ArgumentException("Funds are not enough!");
		_currentValue = nextValue;
	}

    public List<int> GetChange()
    {
        var change = new List<int>();
        if(_currentValue == 0)
            return change;
        _avalibleValues.Sort();
        _avalibleValues.Reverse();


		foreach (int coin in _avalibleValues)
		{
			while (_currentValue >= coin)
			{
				change.Add(coin);
				_currentValue -= coin;
			}
		}
		return change;
    }
}
