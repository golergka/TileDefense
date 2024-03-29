using UnityEngine;

public class PeriodicDamager : Damager
{
	[SerializeField] float damagePeriod;

	float damageStartTime;

	Health target;
	public Health Target 
	{ 
		get { return target; }

		set
		{
			target = value;
			damageStartTime = Time.time;
		}
	}

	void Update()
	{
		if (Target == null)
		{
			return;
		}

		var damageElapsedTime = Time.time - damageStartTime;
		
		/* If Time.deltaTime has been very long, we may have passed 
		 * more than one period during the same update.
		 */
		var periodsPassed = Mathf.FloorToInt(damageElapsedTime / 
				(float) damagePeriod);

		damageStartTime += damagePeriod * periodsPassed;

		for(int i = 0; i < periodsPassed; i++)
			DamageInstantly(Target);
	}

	void OnEnable()
	{
		damageStartTime = Time.time;
	}

	void OnDrawGizmosSelected()
	{
		if (target != null)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawLine(transform.position, target.transform.position);
		}
	}
}
