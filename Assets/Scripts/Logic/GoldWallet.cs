using System;

public class GoldWallet
{
	int current;
	public int Current 
	{ 
		get { return current; }
		private set
		{
			if (current == value) return;
			current = value;
			OnCurrentChange();
		}
	}

	public GoldWallet(int initialAmount)
	{
		Current = initialAmount;
	}

	public void Add(int amount)
	{
		if (amount < 0)
		{
			throw new ArgumentException("Got negative reward amount: " + amount);
		}
		if (amount == 0) return;

		Current += amount;
	}

	public bool CanSpend(int amount)
	{
		return Current >= amount;
	}

	public void Spend(int amount)
	{
		if (amount < 0)
		{
			throw new ArgumentException("Can't spend negative amount: " + amount);
		}
		if (amount == 0) return;
		if (!CanSpend(amount))
		{
			throw new ArgumentException("Can't spend: " + amount + " when I only have " + Current);
		}
		Current -= amount;
	}

	public event Action OnCurrentChange = delegate{}; 
}
