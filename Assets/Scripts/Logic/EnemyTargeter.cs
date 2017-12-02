using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(PeriodicDamager))]
public class EnemyTargeter : MonoBehaviour
{
	private PeriodicDamager damageDealer;
	private PeriodicDamager PeriodicDamager
	{
		get
		{
			return damageDealer ?? (damageDealer = GetComponent<PeriodicDamager>());
		}
	}

	List<IDamageReceiver> possibleTargets = new List<IDamageReceiver>();

	void TrySwitchTarget()
	{
		if (PeriodicDamager.Target != null &&
			possibleTargets.Contains(PeriodicDamager.Target))
		{
			return;
		}

		if (possibleTargets.Count == 0)
		{
			return;
		}

		var index = Random.Range(0, possibleTargets.Count);
		PeriodicDamager.Target = possibleTargets[index];
	}

	void OnTriggerEnter(Collider other)
	{
		var enemy = other.GetComponent<IDamageReceiver>();
		if (enemy != null && IsPossibleTarget(other))
		{
			possibleTargets.Add(enemy);
		}
		TrySwitchTarget();
	}

	void OnTriggerExit(Collider other)
	{
		var enemy = other.GetComponent<IDamageReceiver>();
		if (enemy != null && possibleTargets.Contains(enemy))
		{
			possibleTargets.Remove(enemy);
		}
		TrySwitchTarget();
	}

	bool IsPossibleTarget(Component target)
	{
		return target.GetComponent<Enemy>() != null;
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.white;
		foreach(var target in possibleTargets)
		{
			var comp = target as Component;
			if (comp != null)
			{
				Gizmos.DrawLine(transform.position, comp.transform.position);
			}
		}
	}
}
