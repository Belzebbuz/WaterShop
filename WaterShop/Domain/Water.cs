using System;
using WaterShop.Exceptions;

namespace WaterShop.Domain;

public sealed class Water
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = default!;
    public string? ImagePath { get; private set; }
    public int Cost { get; private set; }
    public int Count { get; private set; }
    private Water()
    {
    }

	public static Water Create(string title, int cost)
    {
        return new()
        {
            Id = Guid.NewGuid(),
            Title = title ?? throw new ValidationDataException(nameof(title)),
            Cost = cost == 0
            ? throw new ValidationDataException($"Water must be grater than zero, parameter: {nameof(cost)}")
            : cost,
		};
    }

	public void Update( string title, int cost)
    {
        Title = title ?? throw new ValidationDataException(nameof(title));
        Cost = cost == 0
            ? throw new ValidationDataException($"Water must be grater than zero, parameter: {nameof(cost)}")
            : cost;
    }

    public void RemoveCount(int count)
    {
        if(count < 0)
			throw new ValidationDataException(nameof(count));
        var nextCount = Count - count;
        if (nextCount < 0)
            throw new ValidationDataException("Not enough water count!");

        Count = nextCount;
	}
	public void AddCount(int count)
    {
        if(count < 0) 
            throw new ValidationDataException(nameof(count));
        Count += count;
    }
    public void SetImagePath(string path) => ImagePath = path ?? throw new InvalidDataException(nameof(path));
}
