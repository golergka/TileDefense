using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using System;

public class Floor : MonoBehaviour, IPointerClickHandler
{

	private Action rebuildNav;
	private Transform sourceTransform;
	private Transform targetTransform;

	public void Init(Action rebuildNav, Transform sourceTransform, Transform targetTransform)
	{
		this.rebuildNav = rebuildNav;
		this.sourceTransform = sourceTransform;
		this.targetTransform = targetTransform;
	}

	private NavMeshModifier modifier;

	public void OnPointerClick(PointerEventData pointerEventData)
	{
		if (modifier == null)
		{
			modifier = gameObject.AddComponent<NavMeshModifier>();
		}
		modifier.overrideArea = true;
		modifier.area = (int) NavArea.TestingBuilding;
		modifier.enabled = true;
		rebuildNav();
		
		var path = new NavMeshPath();
		var mask = (int) (NavArea.Walkable | NavArea.TestingBuilding);

		if (NavMesh.CalculatePath(sourceTransform.position, targetTransform.position, mask, path) && path.status == NavMeshPathStatus.PathComplete)
		{
			modifier.area = (int) NavArea.NonWalkable;
			GetComponent<Renderer>().material.color = Color.red;
		}
		else
		{
			modifier.enabled = false;
		}

		rebuildNav();
	}
}
