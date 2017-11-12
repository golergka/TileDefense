using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
	private NavMeshAgent navMeshAgent;
	private NavMeshAgent NavMeshAgent
	{
		get
		{
			return navMeshAgent ?? (navMeshAgent = GetComponent<NavMeshAgent>());
		}
	}

	Vector3 target;

	public void Init(Vector3 target)
	{
		this.target = target;
		RebuildPath();
	}

	public void RebuildPath()
	{
		NavMeshAgent.SetDestination(target);
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawLine(transform.position, target);
	}
}
