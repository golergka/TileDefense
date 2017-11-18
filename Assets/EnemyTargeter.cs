using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(DamageDealer))]
public class EnemyTargeter : MonoBehaviour
{
	private DamageDealer damageDealer;
	private DamageDealer DamageDealer
	{
		get
		{
			return damageDealer ?? (damageDealer = GetComponent<DamageDealer>());
		}
	}

	List<IDamageReceiver> possibleTargets = new List<IDamageReceiver>();

	void TrySwitchTarget()
	{
		if (DamageDealer.Target != null &&
			possibleTargets.Contains(DamageDealer.Target))
		{
			return;
		}

		if (possibleTargets.Count == 0)
		{
			return;
		}

		var index = Random.Range(0, possibleTargets.Count);
		DamageDealer.Target = possibleTargets[index];
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
			Gizmos.DrawLine(transform.position, (target as Component).transform.position);
		}
	}
}
