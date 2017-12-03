using UnityEngine;

public class Damager : MonoBehaviour
{
	[SerializeField] int damageAmount; 

	public void DamageInstantly(Health target)
	{
		target.ReceiveDamage(damageAmount);
	}
}
