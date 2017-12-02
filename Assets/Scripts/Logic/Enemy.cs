using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(PeriodicDamager))]
public class Enemy : MonoBehaviour
{
	#region Components

	private NavMeshAgent navMeshAgent;
	private NavMeshAgent NavMeshAgent
	{
		get
		{
			return navMeshAgent ?? (navMeshAgent = GetComponent<NavMeshAgent>());
		}
	}

	private PeriodicDamager damageDealer;
	private PeriodicDamager PeriodicDamager
	{
		get
		{
			return damageDealer ?? (damageDealer = GetComponent<PeriodicDamager>());
		}
	}

	#endregion

	public int GoldReward;

	Transform targetTransform;

	public void Init<T>(T target)
		where T : Component, IDamageReceiver
	{
		targetTransform = target.transform;
		PeriodicDamager.Target = target;
		RebuildPath();
	}

	public void RebuildPath()
	{
		NavMeshAgent.SetDestination(targetTransform.position);
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawLine(transform.position, targetTransform.position);
	}

	void Update()
	{
		var targetDelta = targetTransform.position - transform.position;
		var closeEnough = targetDelta.sqrMagnitude <= Mathf.Pow(NavMeshAgent.stoppingDistance, 2f);

		PeriodicDamager.enabled = closeEnough;
	}
}
