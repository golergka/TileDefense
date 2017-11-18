using UnityEngine;

public class DamageDealer : MonoBehaviour
{
	[SerializeField] float damagePeriod;
	[SerializeField] int damageAmount; 

	float damageStartTime;
	IDamageReceiver target;

	public void Init(IDamageReceiver target)
	{
		this.target = target;
		damageStartTime = Time.time;
	}

	void Update()
	{
		if (target == null) return;

		var damageElapsedTime = Time.time - damageStartTime;
		
		/* If Time.deltaTime has been very long, we may have passed 
		 * more than one period during the same update.
		 */
		var periodsPassed = Mathf.FloorToInt(damageElapsedTime / 
				(float) damagePeriod);
		damageStartTime += damagePeriod * periodsPassed;
		target.ReceiveDamage(damageAmount * periodsPassed);
	}

	void OnEnable()
	{
		damageStartTime = Time.time;
	}

}
