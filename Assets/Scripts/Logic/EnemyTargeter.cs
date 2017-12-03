using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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

	List<Health> possibleTargets = new List<Health>();

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

		possibleTargets = possibleTargets.Where(p => p != null).ToList();

		var index = Random.Range(0, possibleTargets.Count);
		PeriodicDamager.Target = possibleTargets[index];
	}

	void OnTriggerEnter(Collider other)
	{
		var enemy = other.GetComponent<Health>();
		if (enemy != null && IsPossibleTarget(other))
		{
			Debug.DrawLine(transform.position, other.transform.position, Color.green, 1f);
			possibleTargets.Add(enemy);
		}
		TrySwitchTarget();
	}

	void OnTriggerExit(Collider other)
	{
		var enemy = other.GetComponent<Health>();
		if (enemy != null && possibleTargets.Contains(enemy))
		{
			Debug.DrawLine(transform.position, other.transform.position, Color.blue, 1f);
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
			if (target != null)
			{
				Gizmos.DrawLine(transform.position, target.transform.position);
			}
		}
	}
}
