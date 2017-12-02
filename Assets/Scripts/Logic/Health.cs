using UnityEngine;
using System;

public class Health : MonoBehaviour, IDamageReceiver
{
	[SerializeField] int max;

	public int Max { get { return max; } }
	public int Current { get; private set; }
	public bool Alive { get { return Current > 0; } }

	void Awake()
	{
		Current = max;
	}

	public void ReceiveDamage(int amount)
	{
		if (!Alive) return;
		if (amount == 0) return;
		if (amount < 0)
		{
			throw new ArgumentException("Negative damage amount" + amount);
		}
		
		var old = Current;

		Current -= amount;

		// Variable used so that events are called in corrent order
		var dead = false;
		if (Current <= 0)
		{
			Current = 0;
			dead = true;
		}

		OnCurrentChange(Current - old);

		if (dead)
		{
			OnDie();
		}
	}

	public event Action<int> OnCurrentChange = delegate{};
	public event Action OnDie = delegate{};
}
