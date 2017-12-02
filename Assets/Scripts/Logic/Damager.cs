using UnityEngine;

public class Damager : MonoBehaviour
{
	[SerializeField] int damageAmount; 

	public void DamageInstantly(IDamageReceiver target)
	{
		target.ReceiveDamage(damageAmount);
	}
}
