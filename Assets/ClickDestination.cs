using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ClickDestination : MonoBehaviour
{
	private NavMeshAgent navMeshAgent;
	private NavMeshAgent NavMeshAgent
	{
		get
		{
			return navMeshAgent ?? (navMeshAgent = GetComponent<NavMeshAgent>());
		}
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			var mainCamera = Camera.main;
			if (mainCamera == null)
			{
				Debug.LogError("Main camera is null!");
				return;
			}
			var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				NavMeshAgent.SetDestination(hit.point);
			}
		}
	}
}
