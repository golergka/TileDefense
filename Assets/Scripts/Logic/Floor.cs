using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using System;

public class Floor : MonoBehaviour, IPointerClickHandler
{
	public event Action SnakeActivated = delegate{};

	private Transform sourceTransform;
	private Transform targetTransform;
	private Action rebuildNavMesh;

	[SerializeField] Color activeColor;
	[SerializeField] Color passiveColor;

	public void Init(Action rebuildNavMesh, Transform sourceTransform, Transform targetTransform)
	{
		this.rebuildNavMesh = rebuildNavMesh;
		this.sourceTransform = sourceTransform;
		this.targetTransform = targetTransform;
		DeactivateSnake();
	}

	private NavMeshModifier modifier;

	public void DeactivateSnake()
	{
		if (modifier != null)
		{
			modifier.enabled = false;
			rebuildNavMesh();
		}
		GetComponent<Renderer>().material.color = passiveColor;
	}

	public void TryActivateSnake()
	{
		if (modifier == null)
		{
			modifier = gameObject.AddComponent<NavMeshModifier>();
		}
		modifier.overrideArea = true;
		modifier.area = (int) NavArea.TestingBuilding;
		modifier.enabled = true;

		rebuildNavMesh();
		
		var path = new NavMeshPath();
		var mask = (int) (NavArea.Walkable | NavArea.TestingBuilding);

		if (NavMesh.CalculatePath(sourceTransform.position, targetTransform.position, mask, path) && path.status == NavMeshPathStatus.PathComplete)
		{
			modifier.area = (int) NavArea.NonWalkable;
			GetComponent<Renderer>().material.color = activeColor;
		}
		else
		{
			modifier.enabled = false;
		}

		rebuildNavMesh();
		SnakeActivated();
	}

	public void OnPointerClick(PointerEventData pointerEventData)
	{
		TryActivateSnake();
	}
}
